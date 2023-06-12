using ASMC5.ViewModel;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Product;
using ViewModel.ViewModel.Role;

namespace ASMC5.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }       
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductView>>($"https://localhost:7257/api/Product/GetAllProduct");
            return View(response);

        }
        [HttpGet]
        [Authorize("ADMIN")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize("ADMIN")]
        public async Task<IActionResult> CreateProduct(ProductVM productcreat)
        {
            var productjson = JsonConvert.SerializeObject(productcreat);
            HttpContent content = new StringContent(productjson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7257/api/Product/CreatProduct", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductView>($"https://localhost:7257/api/Product/GetById/{Id}"); // sua ca thg nay nua nhe
            return View(response);

        }
        [HttpPost]
        public async Task<IActionResult> Details(Guid id, ProductView product, int count)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductView>($"https://localhost:7257/api/Product/GetById/{id}"); // sua ca thg nay nua nhe


            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
            //    var user = await _httpClient.GetFromJsonAsync<UserVM>($"https://localhost:7257/api/Users/GetByName/{User.Identity.Name}");
                //if (user != null)
                //{
                //    var responseJson = await _httpClient.GetFromJsonAsync<ProductVM>($"https://localhost:7257/api/Product/GetById/{Id}");
                //    cartDetailVM.Name = product.Name;
                //    if(cartDetailVM.Quantity == 0)
                //    cartDetailVM.Quantity = 1;
                //    cartDetailVM.Quantity = product.Number;
                //    cartDetailVM.Price = product.Price;
                //    cartDetailVM.UrlImage = product.UrlImage;
                //    cartDetailVM.Id = Id;
                 
                //}
                var ListCartDetail = await _httpClient.GetFromJsonAsync<List<CartDetailVM>>($"https://localhost:7257/api/CartDetail/GetAllCartDetail");
                if (ListCartDetail.Count() != 0)
                {
                    var productInCart = ListCartDetail.FirstOrDefault(p => p.ProductID == id);
                    if (productInCart != null)
                    {
                        productInCart.Quantity = productInCart.Quantity + count;
                        var CartDetailUpdate = JsonConvert.SerializeObject(productInCart);
                        HttpContent contentCartDetailUpdate = new StringContent(CartDetailUpdate, Encoding.UTF8, "application/json");
                        await _httpClient.PutAsync($"https://localhost:7257/api/CartDetail/Update/{productInCart.ID}", contentCartDetailUpdate);
                    }

                }
                else
                {
                    var cartVM = new CartDetailVM();
                    cartVM.Quantity = count;
                    cartVM.Status = 1;
                    cartVM.ProductName = response.Name;
                    cartVM.Price = response.Price;
                    cartVM.UrlImage = response.UrlImage;
                    cartVM.ProductID = response.Id;
                    var roleJson = JsonConvert.SerializeObject(cartVM);
                    HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");

                    var responses = await _httpClient.PostAsync("https://localhost:7257/api/CartDetail/CreatCartDetail", content);

                    if (responses.IsSuccessStatusCode)
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
        [HttpGet]
        [Authorize("ADMIN")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductVM>($"https://localhost:7257/api/Product/GetById/{Id}");
            return View(response);

        }
        [HttpPost] // lưu ý là cái mvc này chỉ dùng đc http post và http get ; api thì dùng đc hết
        [Authorize("ADMIN")]
        public async Task<IActionResult> Edit(Guid Id, ProductVM proVM)
        {
            var Json = JsonConvert.SerializeObject(proVM);
            HttpContent content = new StringContent(Json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7257/api/Product/Update/{Id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();

        }



        [HttpPost]
        [Authorize("ADMIN")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var pro = await _httpClient.DeleteAsync($"https://localhost:7257/api/Product/Delete/{Id}");
            if (pro.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("xoá không thành công");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(CartDetailVM cartDetailVM)
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
}
