using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistProject.Business.Abstract
{
    public interface IAccountService
    {
        public Task<BussinessLayerResult<SessionListDto>> Login(IdentityCheckDto ıdentity);
        public Task<BussinessLayerResult<SessionListDto>> Logout(string key);
        public Task<BussinessLayerResult<SessionListDto>> SignUp(UserDto user);

        public Task<BussinessLayerResult<bool?>> ForgatPassword(string email);

        public  Task<BussinessLayerResult<SessionListDto>> GetSession(string key);
        public  Task<BussinessLayerResult<List<EMethod>>> GetUserRoleMethods(long userId);
        public  Task<BussinessLayerResult<List<EMethod>>> GetPublicRoleMethods();
    }
}
