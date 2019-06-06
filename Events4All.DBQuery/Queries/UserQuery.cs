using Events4All.DB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Events4All.DBQuery
{
   
    public class UserQuery
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public UserDTO FindCurrentUser()
        {            
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            UserDTO userDTO = MapUserToDTO(user);
            return userDTO;
        }

        public UserDTO MapUserToDTO(ApplicationUser user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Id = user.Id;
            userDTO.LockoutEnabled = user.LockoutEnabled;
            userDTO.LockoutEndDateUtc = user.LockoutEndDateUtc;
            userDTO.Logins = user.Logins;
            userDTO.PasswordHash = user.PasswordHash;
            userDTO.PhoneNumber = user.PhoneNumber;
            userDTO.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            userDTO.Roles = user.Roles;
            userDTO.SecurityStamp = user.SecurityStamp;
            userDTO.TwoFactorEnabled = user.TwoFactorEnabled;
            userDTO.Username = user.UserName;
            return userDTO;
        }

    }
}
