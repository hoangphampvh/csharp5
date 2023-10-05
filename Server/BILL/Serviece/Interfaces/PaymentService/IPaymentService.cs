using BILL.ViewModel.Product;
using BLL.Serviece.Implements.PaymentService.VnPay.Response;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using BLL.ViewModel.PaymentConfiguration.Payment;
using BLL.ViewModel.PaymentConfiguration.PaymentVM.Payment;

namespace BLL.Serviece.Interfaces.PaymentService
{
    public interface IPaymentService
    {
        public Task<PaymentLinkVM> Create(PaymentCreateVM p);
        public Task<PagedList<PaymentVM>> GetAll(ProductListSearch productListSearch);
        public Task<PaymentVM> GetById(Guid id);
        public Task<PaymentReturnVM> VnpayReturn(VnpayPayResponse vnpayPay);
    }
}
