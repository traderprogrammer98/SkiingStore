using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiingStore.Data;
using SkiingStore.Dtos;
using SkiingStore.Entities;
using SkiingStore.Entities.OrderAggregate;
using SkiingStore.Repositories.Interface;

namespace SkiingStore.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IBasketRepository basketRepository, IProductRepository productRepository, UserManager<User> userManager, AppDbContext dbContext, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet()]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return Ok(ordersDto);
        }
        [HttpGet("{id:int}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }
        [HttpPost()]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            var basket = await _basketRepository.GetBasketAsync(User.Identity.Name);
            if (basket == null)
            {
                return BadRequest(new ProblemDetails { Title = "unable to locate the basket." });
            }
            var items = new List<OrderItem>();
            foreach (var item in basket.items)
            {
                var productItem = await _productRepository.GetAsync(p => p.Id == item.ProductId);
                var productItemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    itemOrdered = productItemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }
            var subtotal = items.Sum(i => i.Quantity * i.Price);
            var deliveryFee = subtotal > 10000 ? 0 : 500;
            var order = new Order
            {
                BuyerId = User.Identity.Name,
                ShippingAddress = createOrderDto.ShippingAddress,
                OrderItems = items,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
            };
            await _orderRepository.AddAsync(order);
            await _basketRepository.RemoveAsync(basket);
            if (createOrderDto.SaveAddress)
            {
                var user = await _userManager.Users.Where(u => u.UserName == User.Identity.Name)
                    .Include(u => u.Address)
                    .FirstOrDefaultAsync();
                user.Address = new UserAddress
                {
                    FullName = createOrderDto.ShippingAddress.FullName,
                    Address1 = createOrderDto.ShippingAddress.Address1,
                    Address2 = createOrderDto.ShippingAddress.Address2,
                    City = createOrderDto.ShippingAddress.City,
                    Zip = createOrderDto.ShippingAddress.Zip,
                    Country = createOrderDto.ShippingAddress.Country,
                    State = createOrderDto.ShippingAddress.State,
                };
                await _userManager.UpdateAsync(user);
            }
            return CreatedAtRoute(nameof(GetOrder), new { id = order.Id }, order.Id);
        }
    }
}
