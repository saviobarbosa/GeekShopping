using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekShopping.Web.Models
{
    public class CouponViewModel
    {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
