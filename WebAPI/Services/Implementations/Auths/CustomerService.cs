using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Data;
using WebAPI.DTOs.Accounts.Customers;
using WebAPI.DTOs.Auths.Customers;
using WebAPI.Models;
using WebAPI.Services.Interfaces.Auths;

namespace WebAPI.Services.Implementations.Auths
{
    public class CustomerService : ICustomerService
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public CustomerService(CoffeeShopDbContext context, IMapper mapper, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _emailService = emailService;
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
            {
                throw new UnauthorizedAccessException("Sai mật khẩu hoặc tên đăng nhập!");
            }

            if (!customer.IsActive)
            {
                throw new UnauthorizedAccessException("Email chưa được xác nhận! Vui lòng kiểm tra hộp thư.");
            }

            return GenerateJwtToken(customer);
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            var isExistedEmail = await _context.Customers
                .AnyAsync(c => c.Email == dto.Email);
            if (isExistedEmail)
            {
                throw new InvalidOperationException("Email này đã được sử dụng!");
            }

            var isExistedUsername = await _context.Customers
                .AnyAsync(c => c.Username == dto.Username);
            if (isExistedUsername)
            {
                throw new InvalidOperationException("Username này đã được sử dụng!");
            }

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                               .Replace("+", "-").Replace("/", "_").Replace("=", "");

            var customer = new Customer
            {
                Email = dto.Email,
                Username = dto.Username,
                FullName = dto.FullName,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = false,                        // Chờ xác nhận email
                ConfirmationToken = token,
                TokenExpiry = DateTime.UtcNow.AddHours(24) // Token hết hạn sau 24h
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Gửi email xác nhận
            var baseUrl = _config["AppSettings:BaseUrl"] ?? "https://localhost:7001";
            var confirmUrl = $"{baseUrl}/api/Customers/confirm-email?token={token}";
            var emailBody = BuildConfirmationEmail(dto.FullName, confirmUrl);

            await _emailService.SendEmailAsync(dto.Email, "Xác nhận tài khoản Coffee Shop", emailBody);

            return "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận tài khoản.";
        }

        public async Task<string> ConfirmEmailAsync(string token)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.ConfirmationToken == token);

            if (customer == null)
            {
                throw new KeyNotFoundException("Token xác nhận không hợp lệ!");
            }

            if (customer.TokenExpiry < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token đã hết hạn! Vui lòng đăng ký lại hoặc yêu cầu gửi lại email.");
            }

            if (customer.IsActive)
            {
                return "Email đã được xác nhận trước đó. Bạn có thể đăng nhập.";
            }

            customer.IsActive = true;
            customer.ConfirmationToken = null;  // Xóa token sau khi dùng
            customer.TokenExpiry = null;

            await _context.SaveChangesAsync();

            return "Xác nhận email thành công! Bạn có thể đăng nhập ngay.";
        }

        private string GenerateJwtToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Name, customer.Username)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string BuildConfirmationEmail(string fullName, string confirmUrl)
        {
            return $"""
                <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 24px; border: 1px solid #e0e0e0; border-radius: 8px;">
                    <h2 style="color: #c0392b;">☕ Coffee Shop – Xác nhận tài khoản</h2>
                    <p>Xin chào <strong>{fullName}</strong>,</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản. Vui lòng nhấn vào nút bên dưới để xác nhận email:</p>
                    <div style="text-align: center; margin: 32px 0;">
                        <a href="{confirmUrl}"
                           style="background-color: #c0392b; color: white; padding: 14px 28px;
                                  text-decoration: none; border-radius: 6px; font-size: 16px;">
                            Xác nhận Email
                        </a>
                    </div>
                    <p style="color: #888; font-size: 13px;">Link có hiệu lực trong <strong>24 giờ</strong>. Nếu bạn không đăng ký, hãy bỏ qua email này.</p>
                </div>
                """;
        }
    }
}
