﻿using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using BILL.ViewModel.Bill;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class BillServiece : IBillServiece
    {
        ASMDBContext context;
        public BillServiece()
        {
            this.context = new ASMDBContext();
        }

        public async Task<bool> confirmBill(Guid id)
        {
            try
            {
                var list = await context.Bills.ToListAsync();
                var obj = list.FirstOrDefault(c => c.ID == id);

                obj.Status = 0;
                context.Bills.Update(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> CreatBill(BillVM p)
        {
            try
            {
                var user = await context.Users.FindAsync(p.UserID);

                if (user == null)
                {
                    return false;
                }
                var bill = new Bill() {
                    ID =  p.Id,
                    DateCreatBill = p.DateCreatBill,
                    DateOfPayment = p.DateOfPayment,
                    Status = 0,
                    UserID = user.Id,
                    User = user,
                };                
                context.Add(bill);
                context.SaveChanges();
                return true;
            
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< bool> DelBill(Guid id)
        {
            try
            {
                var list = context.Bills.ToList();
                var obj = list.FirstOrDefault(c => c.ID == id);
                context.Bills.Remove(obj);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task <bool> EditBill(Guid id, BillVM p)
        {
            try
            {
                var listobj = context.Bills.ToList();
                var obj = listobj.FirstOrDefault(c => c.ID == id);
                obj.DateCreatBill = p.DateCreatBill;
                obj.DateOfPayment = p.DateOfPayment;
                obj.Status = p.Status;
                context.Bills.Update(obj);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task< List<Bill>> GetAllBill()
        {
                return context.Bills.ToList();
        }

        public async Task< Bill> GetBillById(Guid id)
        {
            var list = context.Bills.AsQueryable().ToList();
            return list.FirstOrDefault(c => c.ID == id);
        }
    }
}
