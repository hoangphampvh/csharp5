﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILL.ViewModel.Cart
{
    public class CartVN
    {
        public  Guid UserId { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
    }
}
