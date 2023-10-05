using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using BILL.CacheRedisData;
using BLL.ViewModel.ModelConfiguration.SeedWork;
using BLL.ViewModel.PaymentConfiguration.MerchantVM;
using BLL.Serviece.Interfaces;

namespace APIServer.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _MerchantService;
       // protected CacheData CacheData = CacheData.Instance;
        public MerchantController(IMerchantService IMerchantService)
        {
            _MerchantService = IMerchantService;
        }
        //   [Cache]
        //   [AuthorizeUser]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProductListSearch taskListSearch)
        {
            var Merchants = await _MerchantService.GetAllMerchant(taskListSearch);
            var pageMerchants = Merchants.Items.Select(x => new MerchantVM()
            {
                Id = x.Id,
                IsActive = x.IsActive,
                MerchantIpnUrl = x.MerchantIpnUrl,
                MerchantName = x.MerchantName,
                MerchantReturnUrl = x.MerchantReturnUrl,
                MerchantWebLink = x.MerchantWebLink,

            });

            var resultJson = new PagedList<MerchantVM>(pageMerchants.ToList(),
            Merchants.MetaData.TotalCount,
                        Merchants.MetaData.CurrentPage,
                        Merchants.MetaData.PageSize);

            return Ok(resultJson);

        }
        //  [AuthorizeUser]
        [HttpPost("Creat")]
        // [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreatProductAsync([FromBody] MerchantCreateVM Merchant)
        {
            var pro = await _MerchantService.CreateMerchant(Merchant);
            return Ok(pro);
        }

        //    //  [Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete/{Id}")]
        //   //   [AuthorizeUser]

        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var result = await _MerchantService.DelMerchant(Id);
            if (result) return Ok(result); else return BadRequest();
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _MerchantService.GetMerchantById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        // //     [Authorize(Roles = "ADMIN")]
        [HttpPut("Update/{id}")]
        ////      [AuthorizeUser]

        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] MerchantUpdateVM product)
        {
            var result = await _MerchantService.EditMerchant(id, product);
            return Ok(result);
        }

    }
}
