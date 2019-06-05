using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events4All.DBQuery
{
    public class UserDTO
    {
        public string Id { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public ICollection<Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin> Logins { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public ICollection<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole> Roles { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Username { get; set; }
    }
}
