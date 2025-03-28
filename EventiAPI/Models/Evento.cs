namespace EventiAPI.Models
{
    public class Evento
    {
        public int EventoId { get; set; }
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public string Luogo { get; set; }
        public int ArtistaId { get; set; }
        public Artista Artista { get; set; }
        public ICollection<Biglietto> Biglietti { get; set; }
    }
}
