using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using di.proyecto.clase._2024.MVVM.Base;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("students")]
[Index("Email", Name = "email", IsUnique = true)]
[Index("GroupCode", Name = "group_code")]
public partial class Student : PropertyChangedDataError
{
    [Key]
    [Column("nia")]
    [Required(ErrorMessage = "NIA is required.")]
    [StringLength(10, MinimumLength = 5, ErrorMessage = "NIA must be 5 to 10 digits.")]
    [RegularExpression(@"^\d{5,10}$", ErrorMessage = "NIA must contain only numeric digits.")]
    public string Nia { get; set; } = null!;



    [Column("first_name")]
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]

    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(30, ErrorMessage = "Last name cannot exceed 30 characters.")]
    public string LastName { get; set; } = null!;

    [Column("phone")]
    [StringLength(15, ErrorMessage = "Phone number must be at most 15 digits.")]
    [RegularExpression(@"^\d{9,15}$", ErrorMessage = "Phone must contain only digits (9 to 15 numbers).")]
    public string? Phone { get; set; }


    [Column("email")]
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [Column("group_code")]
    [Required(ErrorMessage = "Group code is required.")]
    public string GroupCode { get; set; } = null!;


    public string FullName => $"{LastName}, {FirstName}";

    [ForeignKey("GroupCode")]
    [InverseProperty("Students")]
    public virtual Group GroupCodeNavigation { get; set; } = null!;

    [InverseProperty("StudentNiaNavigation")]
    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    public IEnumerable<string> GetValidationErrors()
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);
        Validator.TryValidateObject(this, context, validationResults, true);
        return validationResults.Select(vr => vr.ErrorMessage);
    }
}
