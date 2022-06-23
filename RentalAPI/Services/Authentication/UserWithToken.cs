using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Authentication
{
   public class UserWithToken : RentalUser
    {
        public UserWithToken(RentalUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
       //     this.Password = user.Password;
        }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
