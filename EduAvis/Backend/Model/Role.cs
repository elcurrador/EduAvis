using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("roles")]
[Index("Name", Name = "name", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

}
