using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.ViewModels.Account
{
    public class LoginUserVM
    {
     
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}