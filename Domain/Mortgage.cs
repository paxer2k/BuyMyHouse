﻿using Domain.Entities;

namespace Domain
{
    public class Mortgage : BaseEntity
    {
        /// <summary>
        /// A list contain one or two (one-few relationship) customers for mortgage
        /// </summary>
        public List<Customer> Customers { get; set; } = new List<Customer>(); // cannot be null create on init

        /// <summary>
        /// Total mortgage amount to be granted (additive if two customers)
        /// </summary>
        public double MortgageAmount { get; set; }

        /// <summary>
        /// Date to track when the mortgage was created
        /// </summary>
        public DateTime CreatedAt { get; internal set; } = DateTime.Now; // DO NOT MAKE PRIVATE DBSET DOESNT LIKE THAT

        /// <summary>
        /// Date to track expiry date for the mortgage 
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
