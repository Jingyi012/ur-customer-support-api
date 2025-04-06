using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest : RefreshTokenRequestDto
    {
        public string UserId { get; set; }
    }
}
