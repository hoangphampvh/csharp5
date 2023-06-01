using ASMC5.ViewModel;
using BILL.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceStack.Web;
using System.Net.Http.Json;
using System.Text;

namespace ASMC5.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HttpClient _httpClient;
        public RegisterController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpVM registerUser)
        {
            // Convert registerUser to JSON
            var registerUserJSON = JsonConvert.SerializeObject(registerUser);

            // Convert to string content
            HttpContent stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

            // Send POST request to register API
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7257/api/Users/SignUp", stringContent);

            if (response.IsSuccessStatusCode)
            {
                return Content("call api thanh cong");
            }
            else
            {
                return BadRequest();
                // Xử lý trường hợp không thành công
                // return StatusCode((int)response.StatusCode);
            }

        }

    }
}

