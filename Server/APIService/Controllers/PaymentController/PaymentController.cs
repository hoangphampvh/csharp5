using Microsoft.AspNetCore.Mvc;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.PaymentConfiguration.Payment;
using BLL.Serviece.Implements.PaymentService.VnPay.Response;
using BLL.ViewModel.PaymentConfiguration.PaymentVM.Payment;
using System.Reflection;
using Mapster;
using BLL.Serviece.Implements.PaymentService;
using System.Net;
using Ultils.Extensions;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _PaymentService;
    //    private readonly IMediator mediator;
        public PaymentsController(IPaymentService IMerchantService)
        {
            _PaymentService = IMerchantService;
         //   this.mediator = mediator;
        }
        //  [AuthorizeUser]
        [HttpPost("Creat")]
        // [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreatProductAsync([FromBody] PaymentCreateVM PaymentCreateVM)
        {
            var pro = await _PaymentService.Create(PaymentCreateVM);
            if(pro!=null) return Ok(pro);
            return BadRequest();
        }
        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayPayResponse response)
        {
            string returnUrl = string.Empty;
            var returnModel =await _PaymentService.VnpayReturn(response);
            if (returnModel != null && returnModel.UrlReturnVnPay!= "")
            {
                returnUrl = returnModel.UrlReturnVnPay;
                var a = returnModel.ToQueryString();
                var b = $"{returnUrl}?{returnModel.ToQueryString()}";
                if (returnUrl.EndsWith("/"))
                    returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
                return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");
            }
            return BadRequest();
            
        }
    }
}
