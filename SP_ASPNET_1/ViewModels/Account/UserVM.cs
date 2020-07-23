using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.ViewModels.Account
{
    public class UserVM
    {

        public int Id { get; set; }
        
        public string Name { get; set; }
        [Required]
        public string Surename { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}