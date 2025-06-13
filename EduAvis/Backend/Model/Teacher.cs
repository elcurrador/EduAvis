using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using di.proyecto.clase._2024.MVVM.Base;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("teachers")]
[Index("Email", Name = "email", IsUnique = true)]
[Index("RoleId", Name = "role_id")]
[Index("TutorGroup", Name = "tutor_group")]
public partial class Teacher: PropertyChangedDataError
{
    [Key]
    [Column("dni")]
    [Required(ErrorMessage = "DNI is required.")]
    [StringLength(10, MinimumLength = 8, ErrorMessage = "DNI must be between 8 and 10 characters.")]
    [RegularExpression(@"^[0-9A-Z]+$", ErrorMessage = "DNI must be alphanumeric (no symbols).")]
   
    public string Dni { get; set; } = null!;

    [Column("first_name")]
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(30, ErrorMessage = "Last name cannot exceed 30 characters.")]
    public string LastName { get; set; } = null!;

    public string FullName => $"{LastName}, {FirstName}";

    [Column("email")]
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Column("password")]
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = null!;

    [Column("tutor_group")]
    [Required(ErrorMessage = "Tutor group is required.")]
    [StringLength(10)]
    public string? TutorGroup { get; set; }

    [Column("role_id")]
    [Required(ErrorMessage = "Role is required.")]
    public int RoleId { get; set; }

    [Column("is_tutor")]
    public bool IsTutor { get; set; }

    [InverseProperty("RegisteredByNavigation")]
    public virtual ICollection<Incident> IncidentRegisteredByNavigations { get; set; } = new List<Incident>();

    [InverseProperty("TeacherDniNavigation")]
    public virtual ICollection<Incident> IncidentTeacherDniNavigations { get; set; } = new List<Incident>();

    [ForeignKey("RoleId")]
    [InverseProperty("Teachers")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("TutorGroup")]
    [InverseProperty("Teachers")]
    public virtual Group? TutorGroupNavigation { get; set; }

    public IEnumerable<string> GetValidationErrors()
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        Validator.TryValidateObject(this, context, validationResults, true);
        return validationResults.Select(vr => vr.ErrorMessage);
    }

}
