using ASMC5.Models;
using BILL.ViewModel.Product;
using ClientUI.Service.IResponsitories;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        public async Task<bool> UploadFiles(List<Stream> files)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                foreach (var fileStream in files)
                {
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "files",
                        FileName = "your-file-name.ext" // Thay đổi tên tệp tin cần tải lên.
                    };

                    formData.Add(fileContent);
                }

                var response = await _httpClient.PostAsync("Upload", formData);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có.
                Console.WriteLine($"Lỗi: {ex.Message}");
            }

            return false;
        }
        public List<ProductVM> GetListProductFromExcel(string fileName)
        {
            List<ProductVM> products = new List<ProductVM>();
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int i = 1;
            using (var stream = System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductVM
                        {
                            STT = i++,
                            Name = reader.GetValue(0).ToString(),
                            Price = Decimal.Parse(reader.GetValue(1).ToString()),
                            Quantity = int.Parse(reader.GetValue(2).ToString()),
                            Status = int.Parse(reader.GetValue(3).ToString()),
                            Supplier = reader.GetValue(4).ToString(),
                            Description = reader.GetValue(5).ToString(),
                            UrlImage = reader.GetValue(6).ToString(),
                          
                        }) ;
                    }
                }
            }
            return products;
        }
        public bool UpdateProductBySTT(int stt, ProductVM updatedProduct,string fileName)
        {
            var products = GetListProductFromExcel(fileName);
            // Tìm sản phẩm dựa trên STT
            var productToUpdate = products.FirstOrDefault(p => p.STT == stt);

            if (productToUpdate != null)
            {
                // Cập nhật thông tin sản phẩm
                productToUpdate.Name = updatedProduct.Name;
                productToUpdate.Price = updatedProduct.Price;
                productToUpdate.Quantity = updatedProduct.Quantity;
                productToUpdate.Status = updatedProduct.Status;
                productToUpdate.Supplier = updatedProduct.Supplier;
                productToUpdate.Description = updatedProduct.Description;
                productToUpdate.UrlImage = updatedProduct.UrlImage;
                productToUpdate.CreateDate = updatedProduct.CreateDate;

                return true; // Trả về true nếu cập nhật thành công
            }

            return false; // Trả về false nếu không tìm thấy sản phẩm dựa trên STT
        }

    }
}
