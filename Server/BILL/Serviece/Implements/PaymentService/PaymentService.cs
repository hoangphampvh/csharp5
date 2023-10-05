using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using BLL.Serviece.Implements.PaymentService.VnPay.Config;
using BLL.Serviece.Implements.PaymentService.VnPay.Pin;
using BLL.Serviece.Implements.PaymentService.VnPay.Request;
using BLL.Serviece.Implements.PaymentService.VnPay.Response;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using BLL.ViewModel.PaymentConfiguration.Payment;
using BLL.ViewModel.PaymentConfiguration.PaymentDestinationVM;
using BLL.ViewModel.PaymentConfiguration.PaymentVM.Payment;
using Catel.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct.Library.Settings;
using XAct.Messages;

namespace BLL.Serviece.Implements.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ASMDBContext _context;
        private readonly VnpayConfig vnpayConfig;
        public PaymentService(ICurrentUserProvider currentUserProvider, IOptions<VnpayConfig> vnpayConfigOptions)
        {
            _context = new ASMDBContext();
            _currentUserProvider = currentUserProvider;
            vnpayConfig = vnpayConfigOptions.Value;
        }


        // url payment
        public async Task<PaymentLinkVM> Create(PaymentCreateVM p)
        {
            try
            {
                var payment = new Payment()
                {
                    Id = Guid.NewGuid().ToString(),
                    PaymentContent = p.PaymentContent,
                    PaymentCurrency = p.PaymentCurrency,
                    PaymentRefId = p.PaymentRefId,
                    RequiredAmount = p.RequiredAmount,
                    PaymentDate = p.PaymentDate,
                    ExpireDate = p.ExpireDate,
                    PaymentLanguage = p.PaymentLanguage,
                    MerchantId = p.MerchantId,
                    PaymentDestinationId = p.PaymentDestinationId,
                    Signature = p.Signature,
                   
                };
                var PaymentLink = new PaymentLinkVM();
                var paymentUrl = string.Empty;
                switch (p.PaymentDestinationId)
                {
                   // thông báo kết quả GD (Return URL)
                    case "VNPAY":
                        var vnpayPayRequest = new VnpayPayRequest(
                            vnpayConfig.Version,
                            vnpayConfig.TmnCode, 
                            DateTime.Now,
                            _currentUserProvider.IpAddress ?? string.Empty,
                            p.RequiredAmount ?? 0,
                            p.PaymentCurrency ?? string.Empty,
                            "other",
                            p.PaymentContent ?? string.Empty,
                            vnpayConfig.ReturnUrl,
                            payment.Id ?.ToString() ?? string.Empty);
                        paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                        break;

                }
                if (payment.Id !=null)
                {
                    PaymentLink.PaymentUrl = paymentUrl;
                    PaymentLink.PaymentId = payment.Id;
                    await _context.AddAsync(payment);
                    await _context.SaveChangesAsync();
                }
               
                return PaymentLink;
            }
            catch (Exception)
            {
                return new PaymentLinkVM();
            }
        }

        public Task<PagedList<PaymentVM>> GetAll(ProductListSearch productListSearch)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentVM> GetById(Guid id)
        {
            throw new NotImplementedException();
        }


        // url pin
        //public async Task<VnpayPayIpnResponseVM> VnpayPin(VnpayPayResponse vnpayPay)
        //{
        //    var resultData = new VnpayPayIpnResponseVM();

        //    if (vnpayPay != null)
        //    {
        //        string vnp_HashSecret = vnpayConfig.HashSecret; //Secret key
        //        if (vnpayPay.IsValidSignature(vnp_HashSecret))
        //        {
        //            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == vnpayPay.vnp_TxnRef);
        //            if (payment != null)
        //            {

        //                {
        //                    if (payment.RequiredAmount == (vnpayPay.vnp_Amount / 100))
        //                    {
        //                        if (payment.PaymentStatus != "0")
        //                        {
        //                            string message = string.Empty;
        //                            string status = string.Empty;

        //                            if (vnpayPay.vnp_ResponseCode == "00" &&
        //                               vnpayPay.vnp_TransactionStatus == "00")
        //                            {
        //                                status = "0";
        //                                message = "Tran success";
        //                            }
        //                            else
        //                            {
        //                                status = "-1";
        //                                message = "Tran error";
        //                            }
        //                            update database
        //                            var transPay = _context.PaymentTransactions.AsQueryable().Where(p => p.PaymentId == payment.Id);
        //                            if (transPay != null)
        //                            {

        //                            }
        //                            else
        //                            {
        //                                resultData.Set("99", "Input required data");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            resultData.Set("02", "Order already confirmed");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        resultData.Set("04", "Invalid amount");
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            resultData.Set("97", "Invalid signature");
        //        }
        //        return resultData;
        //    }
        //}


        // url return
        public async Task<PaymentReturnVM> VnpayReturn(VnpayPayResponse vnpayPay)
        {
         
            var resultData = new PaymentReturnVM();
            if (vnpayPay != null)
            {
                string vnp_HashSecret = vnpayConfig.HashSecret; //Secret key
                if (vnpayPay.IsValidSignature(vnp_HashSecret))
                {
                    var payment = await _context.Payments.FirstOrDefaultAsync(p=>p.Id == vnpayPay.vnp_TxnRef);
                    if (payment != null)
                    {
                        var merchant = await _context.Merchants.FirstOrDefaultAsync(p => p.Id == payment.MerchantId);
                        if (merchant != null)
                        {
                            resultData.UrlReturnVnPay = merchant.MerchantReturnUrl ?? string.Empty; ;
                            resultData.PaymentId = payment.Id;
                        }
                    }                                   
                    else
                    {
                        resultData.PaymentStatus = "11";
                        resultData.PaymentMessage = "Can't find payment at payment service";
                    }

                    if (vnpayPay.vnp_ResponseCode == "00")
                    {
                        resultData.PaymentStatus = "00";
                        
                        ///TODO: Make signature
                        resultData.Signature = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        resultData.PaymentStatus = "10";
                        resultData.PaymentMessage = "Payment process failed";
                    }
                }
                return resultData;
            }
            return resultData;
        }


    }
}
