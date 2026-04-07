using AutoMapper;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using OtpNet;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthService(
            ApplicationDbContext db,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.db = db;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        //  REGISTER 

        public async Task<string> Register(RegisterDTO dto)
        {
            var existingUser = await db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null) { 
                return "Email already exists";
            }
            var data = mapper.Map<UserMaster>(dto);

            data.PassWord = BCrypt.Net.BCrypt.HashPassword(dto.PassWord);

            var key = KeyGeneration.GenerateRandomKey(20);

            data.TwoFactorSecretKey =
                Base32Encoding.ToString(key);

            data.IsTwoFactorEnabled = false;

            db.Users.Add(data);

            await db.SaveChangesAsync();

            SendEmail(dto.Email);

            return "Registered Successfully";
        }

        // LOGIN 

        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null) { 
                return null;
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password,user.PassWord);

            if (!validPassword)
                return null;

            if (user.IsTwoFactorEnabled)
            {
                return new AuthResponseDTO
                {
                   Message = "2FA_REQUIRED"
                };
            }

            var token = GenerateToken(user.Email);

            return new AuthResponseDTO
            {
                Token = token
            };
        }

        //  FORGOT PASSWORD 

        public async Task<string> ForgotPassword(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) { 
                return "Email not found";
            }

            var otp = new Random().Next(100000, 999999).ToString();

            user.ResetOtp = otp;

            await db.SaveChangesAsync();

            SendEmailOTP(email, otp);

            return "OTP sent successfully";
        }

        // VERIFY RESET OTP 

        public async Task<string> VerifyResetOtp(string email, string otp)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.ResetOtp == otp);

            if (user == null)
                return "Invalid OTP";

            return "OTP verified";
        }

        //  RESET PASSWORD 

        public async Task<string> ResetPassword(
            ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return "Passwords do not match";

            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return "User not found";

            user.PassWord = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.ResetOtp = null;

            await db.SaveChangesAsync();

            return "Password reset successful";
        }

        //  SETUP 2FA 

        public async Task<string> Setup2FA(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            string issuer = "DDDCrypto";

            var otpUri =
                $"otpauth://totp/{issuer}:{email}" +
                $"?secret={user.TwoFactorSecretKey}" +
                $"&issuer={issuer}";

            using var qrGenerator = new QRCodeGenerator();

            var qrCodeData =
                qrGenerator.CreateQrCode(
                    otpUri,
                    QRCodeGenerator.ECCLevel.Q);

            var qrCode = new Base64QRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }

        // VERIFY 2FA 

        public async Task<AuthResponseDTO> Verify2FA(
            string email,
            string code)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

            var keyBytes = Base32Encoding.ToBytes(user.TwoFactorSecretKey);

            var totp = new Totp(keyBytes);

            bool valid = totp.VerifyTotp(code,out long step);

            if (!valid)
            {
                return null;
            }
                

            user.IsTwoFactorEnabled = true;

            await db.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = GenerateToken(user.Email)
            };
        }

        //  JWT 

        private string GenerateToken(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes( configuration["Jwt:Key"]));

            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        //  EMAIL 

        private void SendEmail(string email)
        {
            SendMail(email,
                "Welcome",
                "Please setup your 2FA.");
        }

        private void SendEmailOTP(string email, string otp)
        {
            SendMail(email, "Password Reset OTP",$"Your OTP is {otp}");
        }

        private void SendMail( string toEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Crypto","shakuahmedshaikh@gmail.com"));

            message.To.Add( new MailboxAddress("", toEmail));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("shakuahmedshaikh@gmail.com", "zptoswrxyaqxvqbd");

            client.Send(message);
            client.Disconnect(true);
        }
    }
    }
