﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ViewModel.ModelConfiguration.SeedWork;

namespace BILL.ViewModel.Product
{
    public class ProductListSearch : PagingParameters
    {      
        public string? Name { get; set; }
    }
}
