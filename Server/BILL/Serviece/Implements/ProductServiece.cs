﻿using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Models.SeedWork;
using XAct;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BILL.Serviece.Implements
{
    public class ProductServiece : IProductServiece
    {
        private readonly ASMDBContext _context;
        private readonly ICurrentUserProvider _currentUserProvider;
        public ProductServiece(ICurrentUserProvider currentUserProvider)
        {
            _context = new ASMDBContext();
            _currentUserProvider = currentUserProvider;
        }
        
        public async Task<bool> CreatProduct(ProductVM p)
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    var product = new Product()
                    {
                        ID = Guid.NewGuid(),
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        CreateDate = DateTime.Now,
                        Status = 0, // 0 quy ước là đh
                        Supplier = p.Supplier,
                        Quantity = p.Quantity,
                        UrlImage = p.UrlImage,
                    };
                    await _context.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
               
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
                _context.Products.Remove(obj);
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
            var user = await _currentUserProvider.GetCurrentUserInfo();
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
                obj.CreateDate = p.CreateDate;
                obj.Status = p.Status;
                obj.CreateBy = user.Id;
                _context.Products.Update(obj);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<ProductVM>> GetAllProduct()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var query = from a in  _context.Products.AsQueryable()
                        where a.Status ==1
                        select new Product
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Price = a.Price,
                            Quantity = a.Quantity,
                            Status = a.Status,
                            Supplier = a.Supplier,
                            Description = a.Description,
                            UrlImage = a.UrlImage,
                            CreateDate = a.CreateDate,

                        };
            stopwatch.Stop();
            var ss = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Thời gian thực hiện: {stopwatch.ElapsedMilliseconds} ms");


            var stopwatch2 = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var query2 = from a in _context.Products.AsQueryable()
                       
                        select new Product
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Price = a.Price,
                            Quantity = a.Quantity,
                            Status = a.Status,
                            Supplier = a.Supplier,
                            Description = a.Description,
                            UrlImage = a.UrlImage,
                            CreateDate = a.CreateDate,

                        };
            stopwatch.Stop();
            var ss2 = stopwatch.ElapsedMilliseconds;
          


            var products = await _context.Products.AsQueryable().ToListAsync();
            var data = products.ToList();
            var count = products.Count();
            var productVMList = new List<ProductVM>();
            foreach (var product in data)
            {
                var productVM = new ProductVM
                {
                    Id = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Supplier = product.Supplier,
                    Description = product.Description,
                    UrlImage = product.UrlImage,
                    CreateDate = product.CreateDate,
                    CreateBy = product.CreateBy
                };

                productVMList.Add(productVM);
            }
            return productVMList;
        }

        public async Task<PagedList<ProductVM>> GetAllProductActive(ProductListSearch productListSearch)
        {
            var products = await _context.Products.AsQueryable().Where(p => p.Status != 1).ToListAsync();
            var data =  products.OrderByDescending(x => x.CreateDate)
                .Skip((productListSearch.PageNumber - 1) * productListSearch.PageSize)
                .Take(productListSearch.PageSize)
                .ToList();
            if (!string.IsNullOrEmpty(productListSearch.Name))
            {
               data = data.Where(p => p.Name.Contains(productListSearch.Name)).ToList();
            }
            var count =  products.Count();
            var productVMList = new List<ProductVM>();
            foreach (var product in data)
            {
                var productVM = new ProductVM
                {
                    Id = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Supplier = product.Supplier,
                    Description = product.Description,
                    UrlImage = product.UrlImage,
                    CreateDate = product.CreateDate,
                    CreateBy = product.CreateBy
                };

                productVMList.Add(productVM);
            }
           
            return new PagedList<ProductVM>(productVMList, count, productListSearch.PageNumber, productListSearch.PageSize);
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
    }
}