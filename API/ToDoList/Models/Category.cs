using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(255)] 
    public string CategoryName { get; set; } = null!;

    [Required]
    [ForeignKey("User")] 
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
