using EventiAPI.Data;
using EventiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventiAPI.Controllers
{
    [Route("api/artisti")]
    [ApiController]
    public class ArtistiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Ottenere tutti gli artisti (Accessibile da chiunque)
        [HttpGet]
        public async Task<IActionResult> GetArtisti()
        {
            var artisti = await _context.Artisti.ToListAsync();
            return Ok(artisti);
        }

        // 2. Ottenere un artista per ID (Accessibile da chiunque)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);
            if (artista == null)
                return NotFound();

            return Ok(artista);
        }

        // 3. Creare un nuovo artista (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpPost]
        public async Task<IActionResult> CreateArtista([FromBody] Artista artista)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Artisti.Add(artista);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtista), new { id = artista.ArtistaId }, artista);
        }

        // 4. Modificare un artista esistente (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtista(int id, [FromBody] Artista artista)
        {
            if (id != artista.ArtistaId)
                return BadRequest();

            _context.Entry(artista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Artisti.Any(a => a.ArtistaId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // 5. Eliminare un artista (Solo Admin)
        [Authorize(Roles = "Amministratore")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);
            if (artista == null)
                return NotFound();

            _context.Artisti.Remove(artista);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
