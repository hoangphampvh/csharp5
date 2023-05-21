using ASMC5.data;
using ASMC5.Models;
using BILL.Serviece.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.Serviece.Implements
{
    public class PositionServiece : IPositionServiece
    {
        private readonly ASMDBContext _context;
        private readonly RoleManager<Position> _roleManager;
        public PositionServiece(RoleManager<Position> roleManager)
        {
            _roleManager = roleManager;
            _context = new ASMDBContext();
        }
        public async Task<bool> CreatPosition(Position p)
        {
            try
            {
                var Role = new Position()
                {
                    Name = p.Name,
                    Id = Guid.NewGuid(),
                    status = 0,
                    ConcurrencyStamp = p.ConcurrencyStamp,
                    NormalizedName = p.NormalizedName,
                };
                var result = await _roleManager.CreateAsync(Role);
                if (result.Succeeded)
                    return true;
                else return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DelPosition(Guid id)
        {
            try
            {
                var obj = await _roleManager.FindByIdAsync(id.ToString());
                obj.status = 1;
                await _roleManager.UpdateAsync(obj);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> EditPosition(Guid id, Position p)
        {
            try
            {
                var obj = await _roleManager.FindByIdAsync(id.ToString());
                obj.status = p.status;
                obj.Name = p.Name;
                obj.NormalizedName = p.NormalizedName;
                obj.ConcurrencyStamp = p.ConcurrencyStamp;
                await _roleManager.UpdateAsync(obj);
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Position>> GetAllPosition()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<List<Position>> GetAllPositionActive()
        {
            return await _context.Roles.Where(p=>p.status!=1).ToListAsync();
        }
        public async Task<Position> GetPositionById(Guid id)
        {
           
            return await _roleManager.FindByIdAsync(id.ToString());
        }
    }
}
