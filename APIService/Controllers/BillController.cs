using BILL.Serviece.Implements;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using BILL.ViewModel.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillServiece _bill;

        public BillController(IBillServiece billServiece)
        {
            _bill = billServiece;
        }
        [HttpGet("GetAllBill")]
        public async Task<IActionResult> GetAllAsync()
        {
            var bills = await _bill.GetAllBill();
            return Ok(bills);

        }
        [HttpPost("CreatBill")]
        public async Task<IActionResult> CreatBillAsync([FromForm] BillVM bill)
        {
            var bills = await _bill.CreatBill(bill);
            return Ok(bills);
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteBillAsync(Guid Id)
        {
            var result = await _bill.DelBill(Id);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _bill.GetBillById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] BillVM product)
        {
            var result = await _bill.EditBill(id, product);
            return Ok(result);
        }
    }
}
