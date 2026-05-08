using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Data;
using WebAPI.DTOs.Accounts.Customers;
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
                .FirstOrDefaultAsync(c => c.Username == dto.Username);

            if (customer == null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
            {
                throw new UnauthorizedAccessException("Sai mật khẩu hoặc tên đăng nhập!");
            }

            return GenerateJwtToken(customer);
        }

        private string GenerateJwtToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
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
