using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using di.proyecto.clase._2024.MVVM.Base;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("incidents")]
[Index("ReasonId", Name = "reason_id")]
[Index("RegisteredBy", Name = "registered_by")]
[Index("StudentNia", Name = "student_nia")]
[Index("TeacherDni", Name = "teacher_dni")]
public class Incident
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reason_id")]
    [Required(ErrorMessage = "Reason is required.")]
    public int ReasonId { get; set; }

    [Column("event_datetime", TypeName = "datetime")]
    public DateTime EventDatetime { get; set; }

    [Column("student_nia")]
    
    public string StudentNia { get; set; }

    [Column("teacher_dni")]
    [StringLength(10)]
    public string TeacherDni { get; set; } = null!;

    [Column("registered_datetime", TypeName = "datetime")]
    public DateTime? RegisteredDatetime { get; set; }

    [Column("registered_by")]
    [StringLength(10)]
    public string RegisteredBy { get; set; } = null!;

    [Column("is_sanction")]
    public bool isSanction { get; set; }

    [Column("is_sanctioned")]
    public bool IsSanctioned { get; set; }

    [Column("reason_description", TypeName = "text")]
    public string? ReasonDescription { get; set; }

    [ForeignKey("ReasonId")]
    [InverseProperty("Incidents")]
    [StringLength(45, ErrorMessage = "No puedes superar la longitud de 45 carácteres")]
    [Required(ErrorMessage = "El nombre del modelo no puede estar vacío")]
    public virtual Reason Reason { get; set; } = null!;

    [ForeignKey("RegisteredBy")]
    [InverseProperty("IncidentRegisteredByNavigations")]
    public virtual Teacher RegisteredByNavigation { get; set; } = null!;

    [ForeignKey("StudentNia")]
    [InverseProperty("Incidents")]
    public virtual Student StudentNiaNavigation { get; set; } = null!;

    [ForeignKey("TeacherDni")]
    [InverseProperty("IncidentTeacherDniNavigations")]
    public virtual Teacher TeacherDniNavigation { get; set; } = null!;
}
