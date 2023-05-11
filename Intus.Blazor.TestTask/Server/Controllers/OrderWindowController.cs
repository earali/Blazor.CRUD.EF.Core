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
    public class OrderWindowController : ControllerBase
    {
        private readonly AppDBContext _context;

        public OrderWindowController(AppDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<OrderDetails> details = new List<OrderDetails>();
            // We have to do a multiple join operation, but EF doesn't support it. So, we have to do it separately. 
            //table1 read all data with a inner join between Orders and OrderWindow Table.
            var table1 = (from od in _context.Orders
                          join ow in _context.OrderWindow on od.Id equals ow.OrderId
                          select new {
                              OrderId = od.Id,
                              od.OrderName,
                              od.State,
                              od.FullAddress,
                              OrderWindowID = ow.Id,
                              ow.Quantity,
                              ow.WindowID
                          }).ToList();
            //table2 read all data with a inner join between table1 and OrderWindowElement Table.
            var table2 = (from tb1 in table1
                          join owe in _context.OrderWindowElement on tb1.OrderWindowID equals owe.OrderWindowId
                          select new {
                              tb1.OrderId,
                              tb1.OrderName,
                              tb1.State,
                              tb1.FullAddress,
                              tb1.OrderWindowID,
                              tb1.Quantity,
                              tb1.WindowID,
                              owe.ElementID,
                              owe.Id
                          }).ToList();
            //table3 read Windows Name from Windows Table. It need a inner join between table2 and Windows Table.
            var table3 = (from tb2 in table2
                          join wd in _context.Windows on tb2.WindowID equals wd.Id
                          select new
                          {
                              tb2.OrderId,
                              tb2.OrderName,
                              tb2.State,
                              tb2.FullAddress,
                              tb2.OrderWindowID,
                              tb2.Quantity,
                              tb2.WindowID,
                              tb2.ElementID,
                              tb2.Id,
                              wd.WindowName
                          }).ToList();
            //table4 read Element properties  from Element Table. It need a inner join between table3 and Elements Table.
            var table4 = (from tb3 in table3
                          join el in _context.Elements on tb3.ElementID equals el.Id
                          select new
                          {
                              tb3.OrderId,
                              tb3.OrderName,
                              tb3.State,
                              tb3.FullAddress,
                              tb3.OrderWindowID,
                              tb3.Quantity,
                              tb3.WindowID,
                              tb3.ElementID,
                              tb3.Id,
                              tb3.WindowName,
                              el.ElementName,
                              el.Type,
                              el.Width,
                              el.Height
                          }).ToList();
            //As EF doesn't support Left join (only support inner join), so we have to read all data that are in OrderWindow but not in OrderWindowElement table
            var table5 = (from tb4 in (table1.Where(a => !table4.Select(b => b.OrderWindowID).Contains(a.OrderWindowID)).ToList()) 
                          join wind in _context.Windows on tb4.WindowID equals wind.Id select new
                          {
                              tb4.OrderId,
                              tb4.OrderName,
                              tb4.State,
                              tb4.FullAddress,
                              tb4.OrderWindowID,
                              tb4.Quantity,
                              tb4.WindowID,
                              wind.WindowName
                          }).ToList();
            //we have to read all data that are in Order table but not in OrderWindow table
            var table6 = _context.Orders.Where(a => !_context.OrderWindow.Select(b => b.OrderId).Contains(a.Id)).ToList();

            // Combine element from table4,5 and 6. 
            foreach (var item in table4)
            {
                OrderDetails detail = new OrderDetails();
                detail.Id = item.Id;
                detail.OrderId = item.OrderId;
                detail.OrderName = item.OrderName;
                detail.State = item.State;
                detail.FullAddress = item.FullAddress;
                detail.ElementID = item.ElementID;
                detail.Quantity = item.Quantity;
                detail.WindowID = item.WindowID;
                detail.WindowName = item.WindowName;
                detail.ElementName = item.ElementName;
                detail.Type = item.Type;
                detail.Width = item.Width;
                detail.Height = item.Height;

                details.Add(detail);

            }
            foreach (var item in table5)
            {
                OrderDetails detail = new OrderDetails();
                detail.Id =0;
                detail.OrderId = item.OrderId;
                detail.OrderName = item.OrderName;
                detail.State = item.State;
                detail.FullAddress = item.FullAddress;
                detail.ElementID = 0;
                detail.Quantity = item.Quantity;
                detail.WindowID = item.WindowID;
                detail.WindowName = item.WindowName;
                detail.ElementName = null;
                detail.Type = null;
                detail.Width = 0;
                detail.Height = 0;

                details.Add(detail);

            }
            foreach (var item in table6)
            {
                OrderDetails detail = new OrderDetails();
                detail.Id = 0;
                detail.OrderId = item.Id;
                detail.OrderName = item.OrderName;
                detail.State = item.State;
                detail.FullAddress = item.FullAddress;
                detail.ElementID = 0;
                detail.Quantity = 0;
                detail.WindowID = 0;
                detail.WindowName = null;
                detail.ElementName = null;
                detail.Type = null;
                detail.Width = 0;
                detail.Height = 0;

                details.Add(detail);

            }
            return Ok(details.OrderBy(ob=> ob.OrderId));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ords = await _context.OrderWindow.Where(a => a.OrderId == id).ToListAsync();
            return Ok(ords);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderWindow order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(OrderWindow order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dev = new OrderWindow { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
