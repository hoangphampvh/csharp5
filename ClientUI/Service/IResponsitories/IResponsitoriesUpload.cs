using BILL.ViewModel.Account;
using BILL.ViewModel.Product;


namespace ClientUI.Service.IResponsitories
{
    public interface IResponsitoriesUpload
    {
        public Task<bool> UploadFile(MultipartFormDataContent file, CancellationToken cancellationtoken);
        public Task<List<ProductVM>> UploadFileExcel(MultipartFormDataContent content, CancellationToken cancellationToken);
    }
}
