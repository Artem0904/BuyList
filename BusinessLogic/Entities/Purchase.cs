namespace BusinessLogic.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty; 
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public BotUser? User { get; set; } 
    }
}
