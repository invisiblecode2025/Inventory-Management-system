using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Interface
{
    public interface IUserServices
    {
        public Task<UserDto> UserLogin(UserDto _userDto);
    }
}
