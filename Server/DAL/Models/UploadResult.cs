﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UploadResult
    {
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? ContentType { get; set; }
        public int Status { get; set; } 
    }
}
