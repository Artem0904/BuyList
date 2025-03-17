namespace BusinessLogic.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty; 
        public decimal Price { get; set; }
    }
}
