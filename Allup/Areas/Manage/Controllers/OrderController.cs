﻿using Allup.DAL;
using Allup.Enums;
using Allup.Hubs;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{

    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<Chat> _hubContext;

        public OrderController(AppDbContext context, IHubContext<Chat> hubContext = null)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<Order> query = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }
            int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemCount").Value);

            ViewBag.Status = status;

            return View(PaginationList<Order>.Create(query, page, itemCount));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        //Order Rejectdise Accept Olmasin Mentiqi
        [HttpPost]
        public async Task<IActionResult> Update(int? id, OrderStatus orderstatus, string Comment)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders.Include(o => o.AppUser).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            order.OrderStatus = orderstatus;

            order.Comment = Comment;

            await _context.SaveChangesAsync();

            if (order.AppUser.ConnectionId != null && orderstatus == OrderStatus.Accepted)
            {
                await _hubContext.Clients.Client(order.AppUser.ConnectionId).SendAsync("orderaccepted");
            }

            return RedirectToAction("index");
        }
    }
}
