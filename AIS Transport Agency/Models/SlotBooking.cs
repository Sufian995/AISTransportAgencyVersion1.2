using AIS_Transport_Agency.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AIS_Transport_Agency.Models
{
    public class SlotBooking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? SlotName { get; set; }
        public DateTime Datetime{ get; set; }
        public List<AppUser>? Users { get; set; } = new();
        public int Slots { get; set; } = 5;
        public double Price { get; set; }

        public int OnPromotion { get; set; }

        public LicenseType LicenseType { get; set; }
        [AllowNull]
        public string ImageURL { get; set; } = String.Empty;
        [ScaffoldColumn(false)]
        [DisplayName("Slots Left")]
        public int SlotsLeft { get {
            if (Users == null) return Slots;
            return Slots - Users.Count;
            } }
    }
}
