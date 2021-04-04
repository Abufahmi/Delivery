using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime DateJoin { get; set; }

        [StringLength(2000)]
        public string UserImage { get; set; }
    }
}
