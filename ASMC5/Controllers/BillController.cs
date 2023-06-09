﻿using BILL.ViewModel.Bill;
using Microsoft.AspNetCore.Mvc;
using BILL.ViewModel.Product;
using ASMC5.Models;
using ViewModel.ViewModel.Role;
using ViewModel.ViewModel.Bill;
using Microsoft.AspNetCore.Authorization;

namespace ASMC5.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class BillController : Controller
    {
        private readonly HttpClient _httpClient;
        public BillController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<List<BillView>>($"https://localhost:7257/api/Bill/GetAllList");
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var response = await _httpClient.GetFromJsonAsync<BillVM>($"https://localhost:7257/api/Bill/GetById/{Id}");
            return View(response);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteBill(Guid Id)
        {
            var obj = await _httpClient.DeleteAsync($"https://localhost:7257/api/Bill/Delete/{Id}");
            if (obj.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("xoá không thành công");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListBillDetail()
        {
            var response = await _httpClient.GetFromJsonAsync<List<BillDetailView>>($"https://localhost:7257/api/BillDetail/GetListBillDetail");
            return View(response);
        }
    }
}
