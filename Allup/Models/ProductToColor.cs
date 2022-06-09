﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models
{
    public class ProductToColor
    {
        public int Id { get; set; }
        public Color Color { get; set; }
        public int ColorId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
