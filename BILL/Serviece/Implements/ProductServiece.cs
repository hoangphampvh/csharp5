using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class ProductServiece : IProductServiece
    {
        private readonly ASMDBContext _context;
        public ProductServiece()
        {
            _context = new ASMDBContext();
        }
        // task , async await là bất đồng bộ 
        public async Task<bool> CreatProduct(ProductVM p)
        {
            try
            {

                var product = new Product()
                {
                    ID = Guid.NewGuid(),
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CreateDate = DateTime.Now,
                    Status = 0, // 0 quy ước là kh đh
                    Supplier = p.Supplier,
                    Quantity = p.Quantity,
                    UrlImage = p.UrlImage,
                    // nếu 1 bảng có khóa ngoại chả hạn như productdetail thì ta chỉ cần gán IdProduct = p.IdProduct
                    // mấy cái thuộc tính có từ khóa virual thì kh cần cho vào đây 
                };

                // hàm AddAsync như này là 1 phương thức bất đồng bộ và có từ khóa await ở đầu và phương thức có Async ở cuối
                //p.ID = Guid.NewGuid();
                await _context.AddAsync(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelProduct(Guid id)
        {
            try
            {
                var list = await _context.Products.ToListAsync();
                var obj = list.FirstOrDefault(c => c.ID == id);
                obj.Status = 1; // ta sẽ kh xóa mà thay đổi trạng thái từ hđ sang kh hđ
                _context.Products.Update(obj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditProduct(Guid id, ProductVM p)
        {
            try
            {
                var listobj = await _context.Products.ToListAsync();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Name = p.Name;
                obj.Price = p.Price;
                obj.Quantity = p.Quantity;
                obj.Status = p.Status;
                obj.Supplier = p.Supplier;
                obj.Description = p.Description;
                obj.UrlImage = p.UrlImage;
                _context.Products.Update(obj);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateProductWhenConfirmBill(Guid id, ProductUpdateConfirmVM p)
        {
            try
            {
                var listobj = await _context.Products.ToListAsync();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.Quantity = p.Quantity;
                obj.Status = p.Status;
                _context.Products.Update(obj);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<List<Product>> GetAllProductActive()
        {
            return await _context.Products.Where(p=>p.Status !=0).ToListAsync();
        }
        public async Task<ProductVM> GetProductById(Guid id)
        {
            var list =await _context.Products.AsQueryable().ToListAsync();
            var product = list.FirstOrDefault(c => c.ID == id);
            if(product != null)
            {
                var ProductVM = new ProductVM();
                ProductVM.Price = product.Price;
                ProductVM.Quantity = product.Quantity;
                ProductVM.Supplier = product.Supplier;
                ProductVM.Name = product.Name;
                ProductVM.UrlImage = product.UrlImage;
                ProductVM.Description = product.Description;
                ProductVM.CreateDate = product.CreateDate;
                ProductVM.Status = product.Status;
                return ProductVM;
            }
            return new ProductVM();
        }
    }
}
