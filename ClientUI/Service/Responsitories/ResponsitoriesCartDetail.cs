using ASMC5.Models;
using BILL.ViewModel.Account;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using ClientUI.Service.IResponsitories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;


namespace ClientUI.Service.Responsitories
{
    public class ResponsitoriesCartDetail : IResponsitoriesCartDetail
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponsitoriesCartDetail(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CartDetailVM>> GetAllCartDetail()
        {
          
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var listProduct = await _httpClient.GetFromJsonAsync<List<CartDetailVM>>("api/CartDetail/GetAll");
                if (listProduct != null)
                {
                    return listProduct;

                }
            }
          
            return new List<CartDetailVM>();
        }
        public async Task<bool> CreatCartDetail(CartDetailVM p)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.PostAsJsonAsync("api/CartDetail/Creat", p);
            if (ProductNew.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<bool> EditCartDetail(Guid id, CartDetailVM p)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.PutAsJsonAsync($"api/CartDetail/Update/{id}", p);
            if (ProductNew.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<bool> EditCartDetailPaied(Guid id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.GetAsync($"api/CartDetail/EditCartDetailPaied/{id}");
            if (ProductNew.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<CartDetailVM> GetCartDetailById(Guid id)
        {
            var ProductNew = await _httpClient.GetFromJsonAsync<CartDetailVM>($"api/CartDetail/GetById/{id}");
            if (ProductNew !=null)
            {
                return ProductNew;

            }
            return new CartDetailVM();
        }
        public async Task<bool> DelCartDetail(Guid Id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.DeleteAsync($"api/CartDetail/Delete/{Id}");
            return ProductNew.IsSuccessStatusCode;
        }
    }
}
