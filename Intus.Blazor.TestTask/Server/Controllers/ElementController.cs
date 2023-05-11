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
    public class ElementController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ElementController(AppDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var elements = await _context.Elements.ToListAsync();
            return Ok(elements);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var elements = await _context.Elements.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(elements);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Elements elements)
        {
            _context.Add(elements);
            await _context.SaveChangesAsync();
            return Ok(elements.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Elements elements)
        {
            _context.Entry(elements).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var elements = new Elements { Id = id };
            _context.Remove(elements);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
