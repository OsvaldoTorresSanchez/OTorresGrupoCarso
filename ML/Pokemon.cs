namespace ML
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string imagen { get; set; }
        public string Previus { get; set; }
        public string Next { get; set; }
        public List<object> Pokemons { get; set; }

        public Detalles Detalles { get; set; }

    }
}