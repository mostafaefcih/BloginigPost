using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;add 
using System.Security.Claims;
using System.Threading.Tasks;

namespace SP_ASPNET_1.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(300)]
        public string Name { get; set; }
   
        public string Surname { get; set; }
        public override string ToString()
        {
            return $"{this.Name} {this.Surname}";
        }
          [NotMapped]
        public int AutherLikes { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)

        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType

            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here

            return userIdentity;

        }

    }
}
