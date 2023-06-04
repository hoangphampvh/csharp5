using BILL.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Role;

namespace ASMC5.Controllers
{
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient;
        public RoleController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateVM roleCreate)
        {
            var roleJson = JsonConvert.SerializeObject(roleCreate);
            HttpContent content = new StringContent(roleJson,Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7257/api/Roles/Add", content);
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {           
            var response = await _httpClient.GetFromJsonAsync<List<RolesVM>>($"https://localhost:7257/api/Roles/GetAllActive");
            return View(response);       

        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<RolesVM>($"https://localhost:7257/api/Roles/GetRoleById/{Id}");
            return View(response);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<RoleUpdateVM>($"https://localhost:7257/api/Roles/GetRoleById/{Id}");
            return View(response);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, RoleUpdateVM roleUpdateVM)
        {
            var roleJson = JsonConvert.SerializeObject(roleUpdateVM);
            HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7257/api/Roles/Update/{Id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var obj = await _httpClient.DeleteAsync($"https://localhost:7257/api/Roles/Delete/{Id}");
            if (obj.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("sai roi");
            }
        }
    }
}
