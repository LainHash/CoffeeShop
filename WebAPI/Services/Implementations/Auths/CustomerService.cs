using AutoMapper;
using Azure;
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
            var query = _context.Customers;
            var customer = await query
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
            {
                throw new UnauthorizedAccessException("Sai mật khẩu hoặc tên đăng nhập!");
            }
            if (customer.IsActive)
            {
                throw new UnauthorizedAccessException("Email chưa được xác nhận!");
            }

            return GenerateJwtToken(customer);
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            var query = _context.Customers;
            var isExistedEmail = await query
                .AnyAsync(c => c.Email == dto.Email);
            if(isExistedEmail)
            {
                throw new UnauthorizedAccessException("Email này đã được sử dụng!");
            }
            var customer = new Customer()
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = false,
                Phone = dto.Phone,
                FullName = dto.FullName
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return "Đăng ký thành công!";

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

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
