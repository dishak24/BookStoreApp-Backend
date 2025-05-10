using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class LoginResponseModel
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        //public int UserId { get; set; }
        
        //public string Role { get; set; }
    }
}
