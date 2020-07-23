using Microsoft.AspNet.Identity.EntityFramework;
using SP_ASPNET_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.ViewModels.Account
{
    public class EditUserVM
    {
        public ApplicationUser User { get; set; }
        public List<IdentityRole> Roles { get; set; }

    }
}