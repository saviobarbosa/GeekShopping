using GeekShopping.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekShopping.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string code, string token);
    }
}
