using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("reasons")]
public partial class Reason
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("short_description")]
    [StringLength(255)]
    public string ShortDescription { get; set; } = null!;

    [InverseProperty("Reason")]
    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
