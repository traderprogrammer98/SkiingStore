using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Dtos;
using SkiingStore.DTOs;
using SkiingStore.Entities;
using SkiingStore.Repositories.Interface;
using SkiingStore.Services;

namespace SkiingStore.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly IBasketRepository _basketRepository;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, TokenService tokenService, IBasketRepository basketRepository, AppDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _basketRepository = basketRepository;
           _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }
            var userBasket = await _basketRepository.GetBasketAsync(loginDto.Username);
            var anonBasket = await _basketRepository.GetBasketAsync(Request.Cookies["buyerId"]);
            if (anonBasket != null )
            {
                if (userBasket != null)
                {
                    await _basketRepository.RemoveAsync(userBasket);
                    anonBasket.BuyerId = user.UserName;
                    Response.Cookies.Append("buyerId", "", new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    anonBasket.BuyerId = user.UserName;
                    Response.Cookies.Append("buyerId", "", new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });
                    await _dbContext.SaveChangesAsync();
                }
            }
            var basket = anonBasket != null ? anonBasket : userBasket;
            var basketDto = _mapper.Map<BasketDto>(basket);
            var userDto = new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = basketDto
            };
            return Ok(userDto);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            return StatusCode(201);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var basket = await _basketRepository.GetBasketAsync(user.UserName);
            var userDto = new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = _mapper.Map<BasketDto>(basket)
            };
            return Ok(userDto);
        }
        [Authorize]
        [HttpGet("SavedAddress")]
        public async Task<IActionResult> GetSavedAddress()
        {
            var address = await _userManager.Users.Where(u => u.UserName == User.Identity.Name)
                 .Select(u => u.Address)
                 .FirstOrDefaultAsync();
            return Ok(address);
        }
    }
}
