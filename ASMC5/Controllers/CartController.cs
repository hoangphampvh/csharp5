using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Product;
using ViewModel.ViewModel.Role;
//dsdds
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
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _httpClient.GetFromJsonAsync<UserVM>($"https://localhost:7257/api/Users/GetByName/{User.Identity.Name}");
                if (user != null)
                {
                    var responseJson = await _httpClient.GetFromJsonAsync<ProductVM>($"https://localhost:7257/api/Product/GetById/{id}");
                    cartDetailVM.ProductName = responseJson.Name;
                    //if(cartDetailVM.Quantity == 0)
                    cartDetailVM.Quantity = 1;
          //          cartDetailVM.Quantity = responseJson.Quantity;
                    cartDetailVM.Price = responseJson.Price;
                    cartDetailVM.UrlImage = responseJson.UrlImage;
                    cartDetailVM.ProductID = id;
                    cartDetailVM.UserID = user.Id;
                }
                var ListCartDetail = await _httpClient.GetFromJsonAsync<List<CartDetailVM>>($"https://localhost:7257/api/CartDetail/GetAllCartDetail");
                if (ListCartDetail.Count() != 0)
                {
                    var productInCart = ListCartDetail.FirstOrDefault(p => p.ProductID == id);
                    if (productInCart != null)
                    {
                        productInCart.Quantity = productInCart.Quantity + cartDetailVM.Quantity;
                        var CartDetailUpdate = JsonConvert.SerializeObject(productInCart);
                        HttpContent contentCartDetailUpdate = new StringContent(CartDetailUpdate, Encoding.UTF8, "application/json");
                        await _httpClient.PutAsync($"https://localhost:7257/api/CartDetail/Update/{productInCart.ID}", contentCartDetailUpdate);
                    }
                    else
                    {
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

                }
                else
                {
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

            }
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obj = await _httpClient.DeleteAsync($"https://localhost:7257/api/CartDetail/Delete/{id}");
            if (obj.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Sai rồi");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid id,int Quantilty)
        {
            var cart = await _httpClient.GetFromJsonAsync<CartDetailVM>($"https://localhost:7257/api/CartDetail/GetById/{id}");
            cart.Quantity = Quantilty;
            var roleJson = JsonConvert.SerializeObject(cart);
            HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");
            var obj = await _httpClient.PutAsync($"https://localhost:7257/api/CartDetail/Update/{id}", content);
            if (obj.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Sai rồi");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CartDetailVM>>($"https://localhost:7257/api/CartDetail/GetAllCartDetail");
            if (response != null)
            {
                foreach (var item in response)
                {
                    var roleJson = JsonConvert.SerializeObject(item);
                    HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");
                    var result =  await _httpClient.PostAsync($"https://localhost:7257/api/CartDetail/Pay", content);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return BadRequest("sai rui");
         }
    }
}
