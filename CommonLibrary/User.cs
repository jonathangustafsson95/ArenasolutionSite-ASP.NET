using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User name:")]
        [StringLength(10, ErrorMessage = "Your username can't be longer than 10 characters.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password:")]
        [StringLength(10, ErrorMessage = "Your password can't be longer than 10 characters.")]
        public string Password { get; set; }
    }
}
