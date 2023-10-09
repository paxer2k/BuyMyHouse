﻿using Domain.Entities;

namespace Domain
{
    public class House : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal Price { get; set; }

    }
}
