using System.ComponentModel.DataAnnotations;

namespace ProjetPFE.Entities;

public class employe
{
    public int employe_id { get; set; }
    
    [Required]
    public string? nom { get; set; }

    [Required]
    public string? prenom { get; set; }
    [Required]
    public string? matricule { get; set; }

    public string? matricule_resp { get; set; }

    public string? fonction { get; set; }

    [Required]
    public string? role { get; set; }

    public DateTime date_recrutement { get; set; }

    public string? email { get; set; }

    public string password { get; set; }

    public string? compte_winds { get; set; }

    public ICollection<diplome>? diplomes { get; set; }

    public ICollection<experience>? experiences { get; set; }

    public ICollection<certification>? certifications { get; set; }

    public ICollection<technologie>? technologies { get; set; }




}

