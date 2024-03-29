﻿using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Cart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class CartServiece : ICartServiece
    {
        private readonly ASMDBContext _context;
        public CartServiece()
        {
            _context = new ASMDBContext();
        }
        public async Task< bool> CreatCart(CartVN p)
        {
            try
            {
               
                var cart = new Cart()
                {
                    UserId = p.UserId,
                    Description = p.Description,
                    Status = p.Status,
                    
                };
                await _context.AddAsync(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task <bool> DelCart(Guid id)
        {
            try
            {
                var list = _context.carts.ToList();
                var obj = list.FirstOrDefault(c => c.UserId == id);
                _context.carts.Remove(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< bool> EditCart(Guid id, CartVN p)
        {
            try
            {
                var listobj = _context.carts.ToList();
                var obj = listobj.FirstOrDefault(c => c.UserId == id);
                obj.Description = p.Description;
                obj.Status = p.Status;
                _context.carts.Update(obj);
                _context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< List<Cart>> GetAllCart()
        {
            var listCart = await _context.carts.ToListAsync();
            var listUser = await _context.Users.ToListAsync();
            var list = from a in listCart
                       join b in listUser on a.UserId equals b.Id
                       select new Cart
                       {
                           Description = a.Description,
                           Status = b.Status,
                           UserId = b.Id,
                           

                       };

            return list.ToList();
        }
    

        public async Task <Cart> GetCartById(Guid id)
        {
            var list = _context.carts.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.UserId == id);
        }
        
    }
}
