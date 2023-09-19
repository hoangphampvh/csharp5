using BILL.ViewModel.Account;
using BILL.ViewModel.Product;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using ClientUI.Service.IResponsitories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace ClientUI.Service.Responsitories
{
    public class ResponsitoriesProduct : IResponsitoriesProduct
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponsitoriesProduct(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedList<ProductVM>> GetAllProductActive(ProductListSearch ProductListSearch)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = ProductListSearch.PageNumber.ToString()
            };
            if (!string.IsNullOrEmpty(ProductListSearch.Name))
                queryStringParam.Add("name", ProductListSearch.Name);
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string url = QueryHelpers.AddQueryString("api/Product/GetAllProductActive", queryStringParam);
            var listProduct = await _httpClient.GetFromJsonAsync<PagedList<ProductVM>>(url);
            if(listProduct != null)
            {
                return listProduct;

            }
            return new PagedList<ProductVM>();
        }
        public async Task<List<ProductVM>> GetAllProduct()
        {

            string url = "api/Product/GetAllProduct";
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var listProduct = await _httpClient.GetFromJsonAsync<List<ProductVM>>(url);
           
            if (listProduct != null)
            {
                return listProduct;

            }
            return new List<ProductVM>();
        }
        public async Task<bool> CreateNewProduct(ProductVM product)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.PostAsJsonAsync("api/Product/CreatProduct",product);
            if (ProductNew.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<bool> UpdateNewProduct(ProductVM product, Guid Id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.PutAsJsonAsync($"api/Product/Update/{Id}", product);
            if (ProductNew.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<ProductVM> GetById(Guid Id)
        {
            var ProductNew = await _httpClient.GetFromJsonAsync<ProductVM>($"api/Product/GetById/{Id}");
            if (ProductNew !=null)
            {
                return ProductNew;

            }
            return new ProductVM();
        }
        public async Task<bool> DeleteById(Guid Id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("_tokenAuthorization");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ProductNew = await _httpClient.DeleteAsync($"api/Product/Delete/{Id}");
            return ProductNew.IsSuccessStatusCode;
        }
    }
}
