using BLL.Serviece.Interfaces;
using BLL.ViewModel.ModelConfiguration.mailConfig;
using MailKit;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendMailController : Controller
    {
            private readonly ISendMailService mailService;
            public SendMailController(ISendMailService mailService)
            {
                this.mailService = mailService;
            }

            [HttpPost("Send")]
            public async Task<IActionResult> Send([FromForm] MailRequest request)
            {
                try
                {
                    await mailService.SendEmailAsync(request);
                    return Ok();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }    
    }
}
