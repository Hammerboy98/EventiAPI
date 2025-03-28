using EventiAPI.Data;
using EventiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EventiAPI.Controllers
{
    [Route("api/biglietti")]
    [ApiController]
    public class BigliettiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BigliettiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Acquisto di un biglietto (Solo Utenti Autenticati)
        [Authorize(Roles = "Utente")]
        [HttpPost]
        public async Task<IActionResult> AcquistaBiglietto([FromBody] AcquistoBigliettoDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var evento = await _context.Eventi.FindAsync(dto.EventoId);
            if (evento == null)
                return NotFound("Evento non trovato.");

            var biglietto = new Biglietto
            {
                EventoId = dto.EventoId,
                UserId = userId,
                DataAcquisto = DateTime.UtcNow
            };

            _context.Biglietti.Add(biglietto);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Biglietto acquistato con successo!" });
        }

        // 2. Ottenere i biglietti dell'utente autenticato
        [Authorize(Roles = "Utente")]
        [HttpGet("miei")]
        public async Task<IActionResult> GetBigliettiUtente()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var biglietti = await _context.Biglietti
                .Include(b => b.Evento)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(biglietti);
        }

        // 3. Ottenere tutti i biglietti venduti (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpGet("venduti")]
        public async Task<IActionResult> GetBigliettiVenduti()
        {
            var biglietti = await _context.Biglietti.Include(b => b.Evento).ToListAsync();
            return Ok(biglietti);
        }
    }

    // DTO per l'acquisto di biglietti
    public class AcquistoBigliettoDto
    {
        public int EventoId { get; set; }
    }
}
