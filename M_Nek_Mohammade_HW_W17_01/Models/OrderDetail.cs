namespace M_Nek_Mohammade_HW_W17_01.Models
{
    public class OrderDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount {  get; set; }

    }
}
