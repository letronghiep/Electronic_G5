using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Electronic_G5.Models
{
    public class Customer
    {
        public int customer_id { get; set; }
        [DisplayName("Hình ảnh")]
        public string customer_image_url { get; set; }

        [DisplayName("Tên khách hàng")]
        public string customer_name { get; set; }
        [DisplayName("Email")]

        public string customer_email { get; set; }
        [DisplayName("Order")]

        public int customer_order_count { get; set; }
        [DisplayName("Tổng tiền")]

        public decimal customer_order_price { get; set; }
        [DisplayName("Số điện thoại")]

        public string customer_phone { get; set; }
        [DisplayName("Địa chỉ")]

        public string customer_city { get; set; }
        [DisplayName("Đặt hàng cuối cùng ")]

        public DateTime customer_last_order { get; set; }
    }
}