
namespace user_panel2.Models
{
    public class Bought
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
