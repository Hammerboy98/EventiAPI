using EventiAPI.Data;
using EventiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventiAPI.Controllers
{
    [Route("api/eventi")]
    [ApiController]
    public class EventiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Ottenere tutti gli eventi (Accessibile da chiunque)
        [HttpGet]
        public async Task<IActionResult> GetEventi()
        {
            var eventi = await _context.Eventi.Include(e => e.Artista).ToListAsync();
            return Ok(eventi);
        }

        // 2. Ottenere un evento per ID (Accessibile da chiunque)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            var evento = await _context.Eventi.Include(e => e.Artista).FirstOrDefaultAsync(e => e.EventoId == id);
            if (evento == null)
                return NotFound();

            return Ok(evento);
        }

        // 3. Creare un nuovo evento (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpPost]
        public async Task<IActionResult> CreateEvento([FromBody] Evento evento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Eventi.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
        }

        // 4. Modificare un evento esistente (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] Evento evento)
        {
            if (id != evento.EventoId)
                return BadRequest();

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Eventi.Any(e => e.EventoId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // 5. Eliminare un evento (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
                return NotFound();

            _context.Eventi.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
