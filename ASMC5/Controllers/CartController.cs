using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Role;

namespace ASMC5.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpContext _httpContext;
        public CartController(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _httpContext = _contextAccessor.HttpContext;


        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CartDetailVM>>($"https://localhost:7257/api/CartDetail/GetAllCartDetail");
            return View(response);

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> addToCart(Guid id)
        {
            var cartDetailVM = new CartDetailVM();
            if (User.Identity != null && User.Identity.Name != null)
            {
                var user = await _httpClient.GetFromJsonAsync<UserVM>($"https://localhost:7257/api/Users/GetByName/{User.Identity.Name}");
                if (user != null)
                {
                    var responseJson = await _httpClient.GetFromJsonAsync<ProductVM>($"https://localhost:7257/api/Product/GetById/{id}");
                    cartDetailVM.ProductName = responseJson.Name;
                    cartDetailVM.Quantity = 1;
                    cartDetailVM.Price = responseJson.Price;
                    cartDetailVM.UrlImage = responseJson.UrlImage;
                    cartDetailVM.ProductID = id;
                    cartDetailVM.UserID = user.Id;
                }
                
            }

            var roleJson = JsonConvert.SerializeObject(cartDetailVM);
            HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7257/api/CartDetail/CreatCartDetail", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obj = await _httpClient.DeleteAsync($"https://localhost:7257/api/Cart/Delete/{id}");
            if (obj.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Sai rồi");
            }
        }
    }
}
