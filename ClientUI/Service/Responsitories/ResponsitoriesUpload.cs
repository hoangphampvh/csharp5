using BILL.ViewModel.Account;
using BILL.ViewModel.Product;
using ClientUI.Service.IResponsitories;
using Newtonsoft.Json;

namespace ClientUI.Service.Responsitories
{
    public class ResponsitoriesUpload : IResponsitoriesUpload
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponsitoriesUpload(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> UploadFile(MultipartFormDataContent content, CancellationToken cancellationToken)
        {
                var response = await _httpClient.PostAsync("api/UploadFile/Upload", content, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
 
                    return true;
                }
                else
                {
                   
                    return false;
                }         
        }
        public async Task<List<ProductVM>> UploadFileExcel(MultipartFormDataContent content, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync("api/UploadFile/Excel", content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductVM>>(result);
                return products;
            }
            else
            {

                return new List<ProductVM>();
            }
        }

    }
}
