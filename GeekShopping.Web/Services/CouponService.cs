using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GeekShopping.Web.Services
{
    public class CouponService : ICouponService
    {
        public const string BASE_PATH = "api/v1/coupon";

        private readonly HttpClient _client;

        public CouponService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CouponViewModel> GetCoupon(string code, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{BASE_PATH}/{code}");
            if (response.StatusCode != HttpStatusCode.OK) return new CouponViewModel();

            return await response.ReadContentAs<CouponViewModel>();
        }
    }
}
