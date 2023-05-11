using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intus.Blazor.TestTask.Server.Data;
using Intus.Blazor.TestTask.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Intus.Blazor.TestTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDBContext _context;

        public OrderController(AppDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ords = await _context.Orders.ToListAsync();
            return Ok(ords);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ords = await _context.Orders.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(ords);
        }
        [HttpPost]
        public async Task<IActionResult> Post(List<OrderDetails> orderList)
        {
            Orders order = new Orders();
            //We assigned Order Info with the first element from Create Order page
            order.OrderName = orderList[0].OrderName;
            order.State = orderList[0].State;
            order.FullAddress = orderList[0].FullAddress;
            // Insert into Order Table
            _context.Add(order);
            await _context.SaveChangesAsync();
            int currentWindowID = 0;
            int currentOderWindowID = 0;
            foreach (OrderDetails details in orderList)
            {
                if (currentWindowID != details.WindowID)
                {
                    // insert into [OrderWindow] Table
                    OrderWindow ow = new OrderWindow();
                    ow.OrderId = order.Id;
                    ow.WindowID = details.WindowID;
                    ow.Quantity = details.Quantity;
                    _context.Add(ow);
                    await _context.SaveChangesAsync();
                    currentOderWindowID = ow.Id;
                    if(details.ElementID> 0)
                    {
                        //// insert into [OrderWindowElement] Table
                        OrderWindowElement owe = new OrderWindowElement();
                        owe.ElementID = details.ElementID;
                        owe.OrderWindowId = ow.Id;
                        _context.Add(owe);
                        await _context.SaveChangesAsync();
                    }
                    currentWindowID = details.WindowID;
                }
                else
                {
                    //// insert into [OrderWindowElement] Table
                    OrderWindowElement owe = new OrderWindowElement();
                    owe.ElementID = details.ElementID;
                    owe.OrderWindowId = currentOderWindowID;
                    _context.Add(owe);
                    await _context.SaveChangesAsync();
                }

            }
            return Ok(order.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(List<OrderDetails> orderList)
        {
            Orders order = new Orders();
            order.Id = orderList[0].OrderId;
            order.OrderName = orderList[0].OrderName;
            order.State = orderList[0].State;
            order.FullAddress = orderList[0].FullAddress;
            // Update Order Table
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            int currentWindowID = 0;
            int currentOderWindowID = 0;
            foreach (OrderDetails details in orderList)
            {
                if (currentWindowID != details.WindowID)
                {
                    var orderWindow = await _context.OrderWindow.FirstOrDefaultAsync(a => a.OrderId == order.Id && a.WindowID == details.WindowID);
                    if (orderWindow != null && orderWindow.Id > 0)
                    {
                        //Update [OrderWindow] Table
                        orderWindow.Quantity = details.Quantity;
                        // Update OrderWindow Table
                        _context.Entry(orderWindow).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        currentOderWindowID = orderWindow.Id;
                        if (details.ElementID > 0)
                        {
                            var orderWindowElement = await _context.OrderWindowElement.FirstOrDefaultAsync(a => a.OrderWindowId == orderWindow.Id && a.ElementID == details.ElementID);
                            if (orderWindowElement != null && orderWindowElement.Id > 0)
                            {
                                // Do nothing. Nothing to change here.
                            }
                            else
                            {
                                //// insert into [OrderWindowElement] Table
                                OrderWindowElement owe = new OrderWindowElement();
                                owe.ElementID = details.ElementID;
                                owe.OrderWindowId = orderWindow.Id;
                                _context.Add(owe);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        // insert into [OrderWindow] Table
                        OrderWindow ow = new OrderWindow();
                        ow.OrderId = order.Id;
                        ow.WindowID = details.WindowID;
                        ow.Quantity = details.Quantity;
                        _context.Add(ow);
                        await _context.SaveChangesAsync();
                        currentOderWindowID = ow.Id;
                        if (details.ElementID > 0)
                        {
                            //// insert into [OrderWindowElement] Table
                            OrderWindowElement owe = new OrderWindowElement();
                            owe.ElementID = details.ElementID;
                            owe.OrderWindowId = ow.Id;
                            _context.Add(owe);
                            await _context.SaveChangesAsync();
                        }
                    }
                   
                    currentWindowID = details.WindowID;
                }
                else
                {
                    var orderWindowElement = await _context.OrderWindowElement.FirstOrDefaultAsync(a => a.OrderWindowId == currentOderWindowID && a.ElementID == details.ElementID);
                    if (orderWindowElement != null && orderWindowElement.Id > 0)
                    {
                        // Do nothing. Nothing to change here.
                    }
                    else
                    {
                        //// insert into [OrderWindowElement] Table
                        OrderWindowElement owe = new OrderWindowElement();
                        owe.ElementID = details.ElementID;
                        owe.OrderWindowId = currentOderWindowID;
                        _context.Add(owe);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = new Orders { Id = id };
            var orderwindow = _context.OrderWindow.Where(a => a.OrderId == id).ToList();
            if (orderwindow.Count > 0)
            {
                foreach (OrderWindow ow in orderwindow)
                {
                    var orderwindowElement = _context.OrderWindowElement.Where(a => a.OrderWindowId == ow.Id).ToList();
                    if (orderwindowElement.Count > 0)
                    {
                        foreach (OrderWindowElement owe in orderwindowElement)
                        {
                            _context.Remove(owe);
                        }
                    }
                    _context.Remove(ow);
                }
            }
            _context.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
