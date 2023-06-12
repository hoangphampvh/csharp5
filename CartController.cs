using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.ViewModel.Cart;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Role;

namespace ASMC5.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CartDetail>>($"https://localhost:7257/api/Cart/GetAllCart");
            return View(response);

        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
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
                // Xử lý trường hợp không thành công
                // return StatusCode((int)response.StatusCode);
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
