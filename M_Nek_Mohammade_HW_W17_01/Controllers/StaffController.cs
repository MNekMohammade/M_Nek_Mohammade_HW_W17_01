using M_Nek_Mohammade_HW_W17_01.DataAccess;
using M_Nek_Mohammade_HW_W17_01.Models;
using Microsoft.AspNetCore.Mvc;

namespace M_Nek_Mohammade_HW_W17_01.Controllers
{
    public class StaffController : Controller
    {
        private DataAccessLayer getdata;

        public StaffController()
        {
            getdata = new DataAccessLayer();
        }

        // GET: Staff/List
        //name , id in list<staff> >>> view  
        [HttpGet]
        public IActionResult List(int? storeId, string storeName)
        {
            List<Staff> staffs = getdata.GetStaffs(storeId, storeName);
            return View(staffs);
        }
    }

}
