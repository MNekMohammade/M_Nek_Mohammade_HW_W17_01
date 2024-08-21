namespace M_Nek_Mohammade_HW_W17_01.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequesteDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public Staff Staff { get; set; }

    }
}
