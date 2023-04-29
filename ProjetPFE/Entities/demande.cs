namespace ProjetPFE.Entities;

public class demande
{
    public int demande_id { get; set; }
    public int offre_id { get; set; }
    public int employe_id { get; set; }
    public int nb_a_exp { get; set; }

    public string? type_demande { get; set; }
    public string? titre_fonction { get; set; }
    public string? nature_contrat { get; set; }
    public string? lien_fichier { get; set; }
    public string? nom_fichier { get; set; }
    public string? remarque { get; set; }
    public string? statut_chef { get; set; }
    public string? statut_rh { get; set; }
    public string? statut_ds { get; set; }
    public string? motif_chef { get; set; }
    public string? motif_rh { get; set; }
    public string? motif_ds { get; set; }
    public string? collaborateur_remp { get; set; }
    public DateTime date_creation { get; set; }
    public string? affectation { get; set; }
    public List<employe>? Employees { get; set; }






}
