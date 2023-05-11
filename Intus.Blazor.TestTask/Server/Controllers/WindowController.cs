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
    public class WindowController : ControllerBase
    {
        private readonly AppDBContext _context;

        public WindowController(AppDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var windows = await _context.Windows.ToListAsync();
            return Ok(windows);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var windows = await _context.Windows.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(windows);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Windows windows)
        {
            _context.Add(windows);
            await _context.SaveChangesAsync();
            return Ok(windows.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Windows windows)
        {
            _context.Entry(windows).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var windows = new Windows { Id = id };
            _context.Remove(windows);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
