using SkiingStore.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkiingStore.DTOs
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }

    }
}