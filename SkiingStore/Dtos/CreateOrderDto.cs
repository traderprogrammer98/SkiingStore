using SkiingStore.Entities.OrderAggregate;

namespace SkiingStore.Dtos
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}
