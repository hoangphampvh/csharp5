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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize("ADMIN")]
        public async Task<IActionResult> Create(ProductVM productcreat)
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
