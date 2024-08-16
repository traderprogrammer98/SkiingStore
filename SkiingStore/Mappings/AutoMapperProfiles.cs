using AutoMapper;
using SkiingStore.Dtos;
using SkiingStore.DTOs;
using SkiingStore.Entities;
using SkiingStore.Entities.OrderAggregate;

namespace SkiingStore.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(des => des.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl))
                .ForMember(des => des.Brand, opt => opt.MapFrom(src => src.Product.Brand))
                .ForMember(des => des.Type, opt => opt.MapFrom(src => src.Product.Type))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(des => des.Price, opt => opt.MapFrom(src => src.Product.Price)).ReverseMap();
            CreateMap<Basket, BasketDto>().ForMember(des => des.BasketItems, opt => opt.MapFrom(src => src.items)).ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(des => des.ProductId, opt => opt.MapFrom(src => src.itemOrdered.ProductId))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.itemOrdered.Name))
                .ForMember(des => des.PictureUrl, opt => opt.MapFrom(src => src.itemOrdered.PictureUrl)).ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(des => des.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(des => des.Total, opt => opt.MapFrom(src => src.GetTotal())).ReverseMap();

        }
    }
}
                


