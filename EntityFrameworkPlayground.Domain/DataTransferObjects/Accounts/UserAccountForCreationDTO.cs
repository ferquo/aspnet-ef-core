using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkPlayground.Domain.DataTransferObjects.Accounts
{
    public class UserAccountForCreationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
