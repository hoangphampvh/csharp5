//using ASMC5.data;
//using ASMC5.Models;
//using BILL.Serviece.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BILL.Serviece.Implements
//{
//    internal class PositionServiece : IPositionServiece
//    {
//        ASMDBContext context;
//        public PositionServiece()
//        {
//            this.context = new ASMDBContext();
//        }
//        public bool CreatPosition(Position p)
//        {
//            try
//            {
//                p.Id = Guid.NewGuid();
//                context.Add(p);
//                context.SaveChanges();
//                return true;
//            }
//            catch (Exception)
//            {

//                return false;
//            }
//        }

//        public bool DelPosition(Guid id)
//        {
//            try
//            {
//                var list = context.positions.ToList();
//                var obj = list.FirstOrDefault(c => c.Id == id);
//                context.positions.Remove(obj);
//                context.SaveChanges();
//                return true;
//            }
//            catch (Exception)
//            {

//                return false;
//            }
//        }

//        public bool EditPosition(Guid id, Position p)
//        {
//            try
//            {
//                var listobj = context.positions.ToList();
//                var obj = listobj.FirstOrDefault(c => c.Id == id);
//                obj.status = p.status;
//                context.positions.Update(obj);
//                context.SaveChanges();
//                return true;

//            }
//            catch (Exception)
//            {

//                return false;
//            }
//        }

//        public List<Position> GetAllPosition()
//        {
//            return context.positions.ToList();
//        }

//        public Position GetPositionById(Guid id)
//        {
//            var list = context.positions.AsQueryable().ToList();
//            return list.FirstOrDefault(c => c.Id == id);
//        }
//    }
//}
