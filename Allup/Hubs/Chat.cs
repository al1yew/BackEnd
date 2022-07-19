using Allup.DAL;
using Allup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Hubs
{
    public class Chat : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public Chat(UserManager<AppUser> userManager, AppDbContext context = null)
        {
            _userManager = userManager;
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser appUser = _userManager.Users.FirstOrDefaultAsync(u => !u.IsAdmin && u.UserName == Context.User.Identity.Name).Result;

                if (appUser != null)
                {
                    appUser.ConnectionId = Context.ConnectionId;
                    appUser.ConnectedAt = null;

                    IdentityResult identityResult = _userManager.UpdateAsync(appUser).Result;

                    Clients.All.SendAsync("online", appUser.Id);
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser appUser = _userManager.Users.FirstOrDefaultAsync(u => !u.IsAdmin && u.UserName == Context.User.Identity.Name).Result;

                if (appUser != null)
                {
                    appUser.ConnectionId = null;
                    appUser.ConnectedAt = DateTime.UtcNow.AddHours(4);

                    IdentityResult identityResult = _userManager.UpdateAsync(appUser).Result;

                    Clients.All.SendAsync("offline", appUser.Id);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId, string message)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser sender = await _userManager.Users.FirstOrDefaultAsync(u => !u.IsAdmin && u.UserName == Context.User.Identity.Name);

                if (sender != null)
                {
                    AppUser reciever = await _userManager.Users.FirstOrDefaultAsync(u => !u.IsAdmin && u.Id == receiverId);

                    if (reciever != null)
                    {
                        Message msg = new Message
                        {
                            CreatedAt = DateTime.UtcNow.AddHours(4),
                            ReceiverId = receiverId,
                            SenderId = sender.Id,
                            Text = message
                        };

                        await _context.Messages.AddAsync(msg);
                        await _context.SaveChangesAsync();

                        if (reciever.ConnectionId != null)
                        {
                            await Clients.Client(reciever.ConnectionId).SendAsync("privatemessage", message, sender.Id, reciever.Id);
                        }
                    }
                }
            }
        }
    }
}
