namespace EventiAPI.Models
{
    public class Artista
    {
        public int ArtistaId { get; set; }
        public string Nome { get; set; }
        public string Genere { get; set; }
        public string Biografia { get; set; }
        public ICollection<Evento> Eventi { get; set; }
    }
}
