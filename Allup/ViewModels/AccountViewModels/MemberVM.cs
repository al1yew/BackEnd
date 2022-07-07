using Allup.Models;
using Allup.ViewModels.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.ViewModels.AccountViewModels
{
    public class MemberVM
    {
        public ProfileVM ProfileVM { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
