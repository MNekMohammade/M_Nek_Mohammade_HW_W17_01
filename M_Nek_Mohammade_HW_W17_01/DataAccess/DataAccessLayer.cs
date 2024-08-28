using M_Nek_Mohammade_HW_W17_01.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
namespace M_Nek_Mohammade_HW_W17_01.DataAccess
{
    public class DataAccessLayer
    {
        string connectionString = "Data Source=.;Initial Catalog=BikeStore;TrustServerCertificate=True;Integrated Security=SSPI";
        

        //-----------متد برای دریافت لیست کارمندان------------ 
        public List<Staff> GetStaffs(int? storeId = null , string storeName = null)
        {
            List<Staff> staffs = new List<Staff>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //کوئری 
                // left join s.manager_id = m.staff_id 
                //managare = staff 
                string query = @"
               SELECT s.staff_id, s.first_name, s.last_name, s.email, s.phone, s.store_id, s.manager_id, 
               st.store_name, m.first_name AS manager_first_name, m.last_name AS manager_last_name
               FROM sales.staffs s
               INNER JOIN sales.stores st ON s.store_id = st.store_id
               LEFT JOIN sales.staffs m ON s.manager_id = m.staff_id
               WHERE (@StoreId IS NULL OR s.store_id = @StoreId)
               AND (@StoreName IS NULL OR st.store_name LIKE '%' + @StoreName + '%')
            ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    //اضافه کردن دو پارمتر به sql
                    cmd.Parameters.AddWithValue("@StoreId", storeId ??(object) DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoreName", storeName ?? (object)DBNull.Value);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        staffs.Add(new Staff
                        {
                            StaffId = Convert.ToInt32(reader["staff_id"]),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Email = reader["email"].ToString(),
                            Phone = reader["phone"].ToString(),
                            StoreId = Convert.ToInt32(reader["store_id"]),
                            Store = new Store
                            {
                                StoreId = Convert.ToInt32(reader["store_id"]),
                                StoreName = reader["store_name"].ToString()
                            },
                            Manager = reader["manager_id"] == DBNull.Value ? null : new Staff
                            {
                                StaffId = Convert.ToInt32(reader["manager_id"]),
                                FirstName = reader["manager_first_name"].ToString(),
                                LastName = reader["manager_last_name"].ToString()
                            }

                        });
                        /*foreach (var staff in staffs)
                        {
                            Console.WriteLine($"Staff: {staff.FirstName} {staff.LastName}");
                            Console.WriteLine($"Store: {staff.Store.StoreName}");

                            if (staff.Manager != null)
                            {
                                Console.WriteLine($"Manager: {staff.Manager.FirstName} {staff.Manager.LastName}");
                            }
                            else
                            {
                                Console.WriteLine("Manager: No manager assigned");
                            }
                        }*/

                    }
                   
                }

            }

            return staffs;

        }
        // -----******--متد برای جستجو سفارش‌ها با شناسه سفارش---******----
        public Order GetOrderById(int orderId)
        {
            Order order = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT o.order_id, c.first_name, c.last_name,c.phone, 
                o.order_date, o.required_date, o.shipped_date, s.first_name AS staff_first_name, s.last_name AS staff_last_name
                FROM sales.orders o
                INNER JOIN sales.customers c ON o.customer_id = c.customer_id
                INNER JOIN sales.staffs s ON o.staff_id = s.staff_id
                WHERE o.order_id = @OrderId
            ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    //Invalid column name 'requested_date' ***خطا زدن سرچ***
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        order = new Order
                        {
                            OrderId = Convert.ToInt32(reader["order_id"]),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            Phone = reader["phone"].ToString(),
                            OrderDate = Convert.ToDateTime(reader["order_date"]),
                            //اینجا
                            RequesteDate = reader["required_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["required_date"]),
                            ShippedDate = reader["shipped_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["shipped_date"]),
                            Staff = new Staff
                            {
                                FirstName = reader["staff_first_name"].ToString(),
                                LastName = reader["staff_last_name"].ToString()
                            }
                        };
                    }
                }
            }

            return order;
        }
        // -----*******--متد برای دریافت جزئیات سفارش----******------
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT od.product_id, p.product_name, od.quantity, od.list_price, od.discount
                FROM sales.order_items od
                INNER JOIN production.products p ON od.product_id = p.product_id
                WHERE od.order_id = @OrderId
            ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orderDetails.Add(new OrderDetail
                        {
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            ProductName = reader["product_name"].ToString(),
                            Quantity = Convert.ToInt32(reader["quantity"]),
                            ListPrice = Convert.ToDecimal(reader["list_price"]),
                            Discount = Convert.ToDecimal(reader["discount"])
                        });
                    }
                }
            }

            return orderDetails;
        }

    }
}
