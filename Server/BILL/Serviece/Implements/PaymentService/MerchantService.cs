using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using BLL.Serviece.Implements.PaymentService.VnPay.Config;
using BLL.Serviece.Implements.PaymentService.VnPay.Request;
using BLL.Serviece.Interfaces;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct.Users;

namespace BLL.Serviece.Implements.PaymentService
{
    public class MerchantService : IMerchantService
    {
        private readonly ASMDBContext _context;
        private readonly ICurrentUserProvider _currentUserProvider;
        public MerchantService(ICurrentUserProvider currentUserProvider)
        {
            _context = new ASMDBContext();
            _currentUserProvider = currentUserProvider;
        }

        public async Task<bool> CreateMerchant(MerchantCreateVM p)
        {
            var user =await _currentUserProvider.GetCurrentUserInfo();
            
            try
            {
                var Merchant = new Merchant()
                {
                    Id = Guid.NewGuid().ToString(),
                    MerchantName = p.MerchantName,
                    MerchantWebLink = p.MerchantWebLink,
                    MerchantIpnUrl = p.MerchantIpnUrl,
                    MerchantReturnUrl = p.MerchantReturnUrl,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user != null ? user.Id.ToString() : ""
                };
                
                await _context.AddAsync(Merchant);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelMerchant(Guid id)
        {
            try
            {
                var list = await _context.Merchants.ToListAsync();
                var obj = list.FirstOrDefault(c => c.Id == id.ToString());
                _context.Merchants.Remove(obj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditMerchant(Guid id, MerchantUpdateVM p)
        {
            var user = await _currentUserProvider.GetCurrentUserInfo();
            try
            {
                var Merchant = new Merchant();

                Merchant.Id = id.ToString();
                Merchant.MerchantName = p.MerchantName;
                Merchant.MerchantWebLink = p.MerchantWebLink;
                Merchant.MerchantIpnUrl = p.MerchantIpnUrl;
                Merchant.MerchantReturnUrl = p.MerchantReturnUrl;
                Merchant.SecretKey = p.SecretKey;
           //     Merchant.CreatedBy = user.Id.ToString();               
                _context.Update(Merchant);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<PagedList<MerchantVM>> GetAllMerchant(ProductListSearch MerchantName)
        {
            var products = await _context.Merchants.AsQueryable().Where(p=>p.IsActive!=false).ToListAsync();
            var data = products.OrderByDescending(x => x.CreatedAt)
                .Skip((MerchantName.PageNumber - 1) * MerchantName.PageSize)
                .Take(MerchantName.PageSize)
                .ToList();
            if (!string.IsNullOrEmpty(MerchantName.Name))
            {
                data = data.Where(p => p.MerchantName.Contains(MerchantName.Name)).ToList();
            }
            var count = products.Count();
            var productVMList = new List<MerchantVM>();
            foreach (var p in data)
            {
                var MerchantVM = new MerchantVM
                {
                    Id = p.Id,
                    MerchantName = p.MerchantName,
                    MerchantWebLink = p.MerchantWebLink,
                    MerchantIpnUrl = p.MerchantIpnUrl,
                    MerchantReturnUrl = p.MerchantReturnUrl,
                    IsActive = p.IsActive,
                };

                productVMList.Add(MerchantVM);
            }

            return new PagedList<MerchantVM>(productVMList, count, MerchantName.PageNumber, MerchantName.PageSize);
        }

        public async Task<MerchantVM?> GetMerchantById(Guid id)
        {
            var list = await _context.Merchants.AsQueryable().ToListAsync();
            var p = list.FirstOrDefault(c => c.Id == id.ToString());
            if (p != null)
            {
                var Merchant = new MerchantVM
                {
                    Id = id.ToString(),
                    MerchantName = p.MerchantName,
                    MerchantWebLink = p.MerchantWebLink,
                    MerchantIpnUrl = p.MerchantIpnUrl,
                    MerchantReturnUrl = p.MerchantReturnUrl,
                };

                return Merchant;
            }
            return null;
        }

        public async Task<SetActiveMerchantVM?> SetActive(string id, SetActiveMerchantVM request)
        {
            var list = await _context.Merchants.AsQueryable().Where(p=>p.IsActive == true).ToListAsync();
            var p = list.FirstOrDefault(c => c.Id == id.ToString());
            if (p != null)
            {
                var Merchant = new SetActiveMerchantVM
                {
                    Id =request.Id,
                    IsActive = request.IsActive,
                };

                return Merchant;
            }
            return null;
        }
    }
}
