﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.ViewModel.Product
{
    public class ProductView
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string? Supplier { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public DateTime? CreateDate { get; set; }
      
    }
}
