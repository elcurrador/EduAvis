using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("permissions")]
[Index("Code", Name = "code", IsUnique = true)]
public partial class Permission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [ForeignKey("PermissionId")]
    [InverseProperty("Permissions")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

}
