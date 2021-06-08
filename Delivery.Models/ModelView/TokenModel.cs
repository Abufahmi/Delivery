using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Delivery.Models.ModelView
{
    public class TokenModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
