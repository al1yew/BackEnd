using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Hubs
{
    public class Chat : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            
        }
    }
}
