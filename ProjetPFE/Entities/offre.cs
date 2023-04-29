namespace ProjetPFE.Entities
{
    public class offre
    {
        public int offre_id { get; set; }
        public int nb_poste { get; set; }
        public string? fonction { get; set; }
        public string? description { get; set; }
        public string? mission { get; set; }
        public string? type_offre { get; set; }
        public ICollection<demande>? demandes { get; set; }
    }
}
