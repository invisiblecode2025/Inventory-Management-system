using AutoMapper;
using Inventory.Infrastructure.Interface;
using Inventory.ManagementDto;
using Inventory.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Services
{
    public class UsersServices : IUserServices
    {
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public UsersServices(IUnitOfWork unitOfWork, IMapper mapper ) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> UserLogin(UserDto _userDto)
        {
            return _mapper.Map<UserDto>(await _unitOfWork.Users.GetWhere(a => a.UserName == _userDto.UserName 
            &&  a.PassWord == _userDto.PassWord ).AsNoTracking().FirstOrDefaultAsync());
        }
    }
}
