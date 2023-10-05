using BILL.ViewModel.Product;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using BLL.ViewModel.PaymentConfiguration.PaymentDestinationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serviece.Interfaces.PaymentService
{
    public interface IPaymentDestination
    {
        public Task<bool> Create(CreatePaymentDestinationVM p);
        public Task<bool> Edit(Guid id, UpdatePaymentDestinationVM p);
        public Task<bool> Del(Guid id);
        public Task<PagedList<PaymentDestinationVM>> GetAll(ProductListSearch productListSearch);
        public Task<PaymentDestinationVM> GetById(Guid id);
        public Task<SetActivePaymentDestinationVM> SetActive(string id, SetActivePaymentDestinationVM request);
    }
}
