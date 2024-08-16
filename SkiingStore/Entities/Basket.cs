namespace SkiingStore.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> items { get; set; } = new();
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public void AddItem(Product product, int quantity)
        {
            if (items.All(item => item.ProductId != product.Id))
            {
                items.Add(new BasketItem { ProductId = product.Id, Quantity = quantity });
            }
            else
            {

            var item = items.FirstOrDefault(item => item.ProductId == product.Id);
                item.Quantity += quantity;
            }
        }
        public void RemoveItem(Product product, int quantity)
        {
            var item = items.FirstOrDefault(item => item.ProductId == product.Id);
            if (item == null) return;
            item.Quantity -= quantity;
            if (item.Quantity <= 0)
            {
                items.Remove(item);
            }
        }
    }
}
