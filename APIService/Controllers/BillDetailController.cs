using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillDetailController : ControllerBase
    {
        private readonly IBillDetailServiece _billDetail;
        public BillDetailController(IBillDetailServiece billDetailServiece)
        {
            _billDetail = billDetailServiece;
        }
        [HttpGet("GetAllBillDetail")]
        public async Task<IActionResult> GetAllAsync()
        {
            var billdetail = await _billDetail.GetAllBillDetail();
            return Ok(billdetail);
        }
        [HttpPost("CreatBill")]
        public async Task<IActionResult> CreatBillDetailAsync([FromForm] BillDetailVM billDeatil)
        {
            var bills = await _billDetail.CreatBillDetail(billDeatil);
            return Ok(bills);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteBillDetailAsync(Guid Id)
        {
            var result = await _billDetail.DelBillDetail(Id);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pro = await _billDetail.GetBillDetailById(id);
            if (pro != null) return Ok(pro);
            return BadRequest();
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] BillDetailVM billdetail)
        {
            var result = await _billDetail.EditBillDetail(id, billdetail);
            return Ok(result);
        }
        [HttpGet("GetListBillDetail")]
        public async Task<IActionResult> GetListAsync()
        {
            var billDetails = await _billDetail.ListBillDetail();
            return Ok(billDetails);
        }
    }
}
