using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using BILL.CacheRedisData;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.PaymentDestinationVM;
using BLL.Serviece.Interfaces.PaymentService;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;

namespace APIServer.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDestinationController : ControllerBase
    {
        private readonly IPaymentDestination _PaymentDestinationService;
      //  protected CacheData CacheData = CacheData.Instance;
        public PaymentDestinationController(IPaymentDestination IPaymentDestinationService)
        {
            _PaymentDestinationService = IPaymentDestinationService;
        }
        //   [Cache]
        //   [AuthorizeUser]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProductListSearch taskListSearch)
        {
            var PaymentDestinations = await _PaymentDestinationService.GetAll(taskListSearch);
            var pagePaymentDestinations = PaymentDestinations.Items.Select(p => new PaymentDestinationVM()
            {

                Id = p.Id,
                DesName = p.DesName,
                DesLogo = p.DesLogo,
                SortIndex = p.SortIndex,
                DesShortName = p.DesShortName,
                DesParentId = p.DesParentId,
                IsActive = p.IsActive,

            });

            var resultJson = new PagedList<PaymentDestinationVM>(pagePaymentDestinations.ToList(),
            PaymentDestinations.MetaData.TotalCount,
                        PaymentDestinations.MetaData.CurrentPage,
                        PaymentDestinations.MetaData.PageSize);

            return Ok(resultJson);

        }
        //  [AuthorizeUser]
        [HttpPost("Creat")]
        // [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreatProductAsync([FromBody] CreatePaymentDestinationVM PaymentDestination)
        {
            var pro = await _PaymentDestinationService.Create(PaymentDestination);
            return Ok(pro);
        }

        //    //  [Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete/{Id}")]
        //   //   [AuthorizeUser]

        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var result = await _PaymentDestinationService.Del(Id);
            if (result) return Ok(result); else return BadRequest();
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _PaymentDestinationService.GetById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        // //     [Authorize(Roles = "ADMIN")]
        [HttpPut("Update/{id}")]
        ////      [AuthorizeUser]

        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePaymentDestinationVM product)
        {
            var result = await _PaymentDestinationService.Edit(id, product);
            return Ok(result);
        }

    }
}
