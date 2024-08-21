using M_Nek_Mohammade_HW_W17_01.DataAccess;
using M_Nek_Mohammade_HW_W17_01.Models;
using Microsoft.AspNetCore.Mvc;

namespace M_Nek_Mohammade_HW_W17_01.Controllers
{
    public class OrderController : Controller
    {
        private DataAccessLayer getdata;

        public OrderController()
        {
            getdata = new DataAccessLayer();
        }
        
        // GET: Order/Search
        [HttpGet]
        public IActionResult Search(int? orderId)
        {
            if (orderId.HasValue)
            {
                Order order = getdata.GetOrderById(orderId.Value);
                return View(order);
            }
            return View();
        }

        // GET: Order/Details
        [HttpGet]
        public IActionResult Details(int orderId)
        {
            List<OrderDetail> orderDetails = getdata.GetOrderDetails(orderId);
            ViewBag.TotalAmount = CalculateTotalAmount(orderDetails);
            return View(orderDetails);
        }

        private decimal CalculateTotalAmount(List<OrderDetail> orderDetails)
        {
            decimal totalAmount = 0;
            foreach (var item in orderDetails)
            {
                totalAmount += (item.ListPrice - item.Discount) * item.Quantity;
            }
            return totalAmount;
        }
    }

}
