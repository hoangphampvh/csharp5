using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using BLL.ViewModel.PaymentConfiguration.PaymentDestinationVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serviece.Implements.PaymentService
{
    public class PaymentDestinationService : IPaymentDestination
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ASMDBContext _context;
        public PaymentDestinationService(ICurrentUserProvider currentUser, ASMDBContext context)
        {
            _currentUserProvider = currentUser;
            _context = context;
        }
        public async Task<bool> Create(CreatePaymentDestinationVM p)
        {
            var user = await _currentUserProvider.GetCurrentUserInfo();
            try
            {

                var payDes = new PaymentDestination();
                payDes.IsActive = true;
                payDes.DesName = p.DesName;
                payDes.DesParentId = p.DesParentId;
                payDes.DesLogo = p.DesLogo;
                payDes.SortIndex = p.SortIndex;
                payDes.CreatedAt = DateTime.Now;
                payDes.CreatedBy = user != null ? user.Id.ToString() : "";
                payDes.Id = Guid.NewGuid().ToString();
                await _context.AddAsync(payDes);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }

        }

        public async Task<bool> Del(Guid id)
        {
            var user = await _currentUserProvider.GetCurrentUserInfo();
            try
            {

                var payDes = _context.PaymentDestinations.FirstOrDefault(p => p.Id == id.ToString());
                payDes.IsActive = false;

                if (user != null)
                    payDes.CreatedBy = user.Id.ToString();
                payDes.Id = Guid.NewGuid().ToString();
                _context.Update(payDes);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }

        }

        public async Task<bool> Edit(Guid id, UpdatePaymentDestinationVM p)
        {
            var user = await _currentUserProvider.GetCurrentUserInfo();
            try
            {

                var payDes = _context.PaymentDestinations.FirstOrDefault(p => p.Id == id.ToString());

                payDes.DesName = p.DesName;
                payDes.DesParentId = p.DesParentId;
                payDes.DesLogo = p.DesLogo;
                payDes.SortIndex = p.SortIndex;
                payDes.LastUpdatedAt = DateTime.Now;
                if (user != null)
                    payDes.LastUpdatedByy = user.Id.ToString();
                payDes.IsActive = false;
                _context.Update(payDes);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public async Task<PagedList<PaymentDestinationVM>> GetAll(ProductListSearch PaymentDestinationName)
        {
            var products = await _context.PaymentDestinations.AsQueryable().Where(p => p.IsActive != false).ToListAsync();
            var data = products.OrderByDescending(x => x.CreatedAt)
                .Skip((PaymentDestinationName.PageNumber - 1) * PaymentDestinationName.PageSize)
                .Take(PaymentDestinationName.PageSize)
                .ToList();
            if (!string.IsNullOrEmpty(PaymentDestinationName.Name))
            {
                data = data.Where(p => p.DesName.Contains(PaymentDestinationName.Name)).ToList();
            }
            var count = products.Count();
            var PaymentDestinationVMList = new List<PaymentDestinationVM>();
            foreach (var p in data)
            {
                var PaymentDestinationVM = new PaymentDestinationVM
                {
                    Id = p.Id,
                    DesName = p.DesName,
                    DesLogo = p.DesLogo,
                    SortIndex = p.SortIndex,
                    DesShortName = p.DesShortName,
                    DesParentId = p.DesParentId,
                    IsActive = p.IsActive,
                };

                PaymentDestinationVMList.Add(PaymentDestinationVM);
            }

            return new PagedList<PaymentDestinationVM>(PaymentDestinationVMList, count, PaymentDestinationName.PageNumber, PaymentDestinationName.PageSize);
        }

        public async Task<PaymentDestinationVM> GetById(Guid id)
        {
            var list = await _context.PaymentDestinations.AsQueryable().ToListAsync();
            var PayDes = list.FirstOrDefault(c => c.Id == id.ToString());
            if (PayDes != null)
            {
                var PayDesVM = new PaymentDestinationVM()
                {
                    DesParentId = PayDes.DesParentId,
                    IsActive = PayDes.IsActive,
                    DesLogo = PayDes.DesLogo,
                    DesShortName = PayDes.DesShortName,
                    DesName = PayDes.DesName,
                    SortIndex = PayDes.SortIndex,
                    Id = PayDes.Id,
                };

                return PayDesVM;
            }
            return null;
        } 

        public async Task<SetActivePaymentDestinationVM> SetActive(string id, SetActivePaymentDestinationVM request)
        {
            var list = await _context.Merchants.AsQueryable().Where(p => p.IsActive == true).ToListAsync();
            var p = list.FirstOrDefault(c => c.Id == id.ToString());
            if (p != null)
            {
                var Merchant = new SetActivePaymentDestinationVM
                {
                    Id = request.Id,
                    IsActive = request.IsActive,
                };

                return Merchant;
            }
            return null;
        }
    }
}
