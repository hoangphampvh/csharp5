using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Cart;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServiece _cartServiece;

        public CartController(ICartServiece cartServiece)
        {
            _cartServiece = cartServiece;
        }

        [HttpGet("GetAllCart")]

        public async Task<IActionResult> GetAllAsync()
        {
            var cart = await _cartServiece.GetAllCart();
            return Ok(cart);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cart = await _cartServiece.GetCartById(id);
            if (cart != null) return Ok(cart);
            return BadRequest();
        }


        [HttpPost("CreatCart")]
        public async Task<IActionResult> CreatCartAsync([FromBody]CartVN cartt)
        {
            var car = await _cartServiece.CreatCart(cartt);
            return Ok(car);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            var result = await _cartServiece.DelCart(Id);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CartVN ca)
        {
            var result = await _cartServiece.EditCart(id, ca);
            return Ok(result);
        }
    }
}
