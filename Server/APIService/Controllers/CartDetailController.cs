using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Cart;
using DemoRedis.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private readonly ICartDetailServiece _cartDetail;

        public CartDetailController(ICartDetailServiece  cartDetailServiece)
        {
            _cartDetail = cartDetailServiece;
        }
        [AuthorizeUser]
        [Cache]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var cartde = await _cartDetail.GetAllCartDetail();
            return Ok(cartde);
        }
        [HttpPost("Creat")]
        public async Task<IActionResult> CreateCartDetailDetailAsync([FromBody] CartDetailVM cartde)
        {
            var cartdetais = await _cartDetail.CreatCartDetail(cartde);
            return Ok(cartdetais);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteCarrtDetailAsync(Guid Id)
        {
            var result = await _cartDetail.DelCartDetail(Id);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _cartDetail.GetCartDetailById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CartDetailVM cartvm)
        {
            var result = await _cartDetail.EditCartDetail(id, cartvm);
            return Ok(result);
        }
        [HttpGet("EditCartDetailPaied/{id}")]
        public async Task<IActionResult> EditCartDetailPaied(Guid id)
        {
            var result = await _cartDetail.EditCartDetailPaied(id);
            return Ok(result);
        }
    }
}
