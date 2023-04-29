namespace ProjetPFE.Dto
{
    public class ArchiveForUpdateDto
    {
        //public int archive_id { get; set; }
        public int demande_id { get; set; }
        public int nb_a_exp { get; set; }

        public string? type_demande { get; set; }
        public string? titre_fonction { get; set; }
        public string? nature_contrat { get; set; }
        public string? lien_fichier { get; set; }
        public string? nom_fichier { get; set; }
        public string? remarque { get; set; }

        public string? collaborateur_remp { get; set; }
        public DateTime date_creation { get; set; }
        public string? affectation { get; set; }
    }
}
