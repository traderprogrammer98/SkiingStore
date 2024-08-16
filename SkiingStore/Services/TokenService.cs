﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SkiingStore.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkiingStore.Services
{
    public class TokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
           _configuration = configuration;
        }
        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
