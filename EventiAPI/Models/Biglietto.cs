namespace EventiAPI.Models
{
    public class Biglietto
    {
        public int BigliettoId { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DataAcquisto { get; set; } = DateTime.UtcNow;
    }
}
