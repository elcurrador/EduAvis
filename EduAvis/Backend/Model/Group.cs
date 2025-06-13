using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model;

[Table("groups")]
public partial class Group
{
    [Key]
    [Column("group_code")]
    [StringLength(10)]
    public string GroupCode { get; set; } = null!;

    [Column("group_name")]
    [StringLength(50)]
    public string GroupName { get; set; } = null!;

    [InverseProperty("GroupCodeNavigation")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("TutorGroupNavigation")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
