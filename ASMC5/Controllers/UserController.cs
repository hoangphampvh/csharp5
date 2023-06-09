using ASMC5.Models;
using ASMC5.ViewModel;
using BILL.ViewModel.Account;
using BILL.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using ViewModel.ViewModel.Role;

namespace ASMC5.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateVM Create)
        {
            var jsonObj = JsonConvert.SerializeObject(Create);
            HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            var respones =await _httpClient.PostAsync("https://localhost:7257/api/Users/Create",content);
            if (respones.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {           
            var response = await _httpClient.GetFromJsonAsync<List<UserVM>>($"https://localhost:7257/api/Users/GetAllActive");
            return View(response);       

        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<User>($"https://localhost:7257/api/Users/GetById/{Id}");
            return View(response);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<UserEditVM>($"https://localhost:7257/api/Users/GetById/{Id}");
            var roles = await _httpClient.GetFromJsonAsync<List<RolesVM>>($"https://localhost:7257/api/Roles/GetAllActive");
            ViewBag.Roles = new SelectList(roles,"Name","Name");
            return View(response);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, UserEditVM UserUpdateVM)
        {
            var roleJson = JsonConvert.SerializeObject(UserUpdateVM);
            HttpContent content = new StringContent(roleJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7257/api/Users/Update/{Id}", content);
            var roles = await _httpClient.GetFromJsonAsync<List<RolesVM>>($"https://localhost:7257/api/Roles/GetAllActive");
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var obj = await _httpClient.DeleteAsync($"https://localhost:7257/api/Users/Delete/{Id}");
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
