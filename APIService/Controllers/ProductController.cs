using ASMC5.Models;
using BILL.ViewModel.Product;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServiece _productServiece;
        
        public ProductController(IProductServiece productServiece)
        {
            _productServiece = productServiece;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productServiece.GetAllProduct();
            return Ok(products);
            
        }

        [HttpPost("CreatProduct")]
        public async Task<IActionResult> CreatProductAsync([FromForm]ProductVM product)
        {
            var pro = await _productServiece.CreatProduct(product);
            return Ok(pro);
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            var result = await _productServiece.DelProduct(Id);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _productServiece.GetProductById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id,[FromBody]ProductVM product)
        {
            var result = await _productServiece.EditProduct(id,product);
            return Ok(result);
        }
    }
}
