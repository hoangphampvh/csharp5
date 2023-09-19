using BILL.ViewModel.Product;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BILL.CacheRedisData;
using Newtonsoft.Json;
using DemoRedis.Attributes;
using BLL.ViewModel.ModelConfiguration.SeedWork;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServiece _productServiece;
        protected CacheData CacheData = CacheData.Instance;
        public ProductController(IProductServiece productServiece)
        {
            _productServiece = productServiece;
        }
        [Cache]
     //   [AuthorizeUser]
        [HttpGet("GetAllProductActive")]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProductListSearch taskListSearch)
        {
            var products = await _productServiece.GetAllProductActive(taskListSearch);
            var pageProducts = products.Items.Select(x => new ProductVM()
            {
                Description = x.Description,
                Status = x.Status,
                Supplier = x.Supplier,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                UrlImage = x.UrlImage,

            });
            
            var resultJson = new PagedList<ProductVM>(pageProducts.ToList(),
            products.MetaData.TotalCount,
                        products.MetaData.CurrentPage,
                        products.MetaData.PageSize);

            return Ok(resultJson);
            
        }
      //  [AuthorizeUser]
        [HttpPost("CreatProduct")]
       // [Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> CreatProductAsync([FromBody]ProductVM product)
        {
            var pro = await _productServiece.CreatProduct(product);
            await CacheData.RemoveCacheResponseAsync("/api/Product/GetAllProductActive");
            return Ok(pro);
        }
        [HttpGet("GetAllProduct")]
        //[Authorize(Roles = "ADMIN")]
        //[Cache]
        //    [AuthorizeUser]

        public async Task<IActionResult> GetAllProduct()
        {
            var pro = await _productServiece.GetAllProduct();
            return Ok(pro);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete/{Id}")]
     //   [AuthorizeUser]

        public async Task<IActionResult> DeleteAsync(Guid Id)
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
        [Authorize(Roles = "ADMIN")]
        [HttpPut("Update/{id}")]
  //      [AuthorizeUser]

        public async Task<IActionResult> UpdateAsync(Guid id,[FromBody]ProductVM product)
        {
            var result = await _productServiece.EditProduct(id,product);
            return Ok(result);
        }
        
    }
}
