﻿using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Category : AuditableEntity
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] 
    public string Name { get; set; }
}