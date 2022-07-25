using FirstApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Interfaces
{
    public interface IJWTManager
    {
        string GenerateToken(AppUser appUser, IList<string> roles);
        string GetUserNameByToken(string token);
    }
}
