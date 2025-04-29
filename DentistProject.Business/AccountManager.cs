using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.ListDto;
using DentistProject.Dtos.Result;
using DentistProject.Entities.Enum;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentistProject.Dtos.Filter;
using DentistProject.Filters.Filter;
using DentistProject.Dtos.Enum;

namespace DentistProject.Business
{
    public class AccountManager : IAccountService
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IRoleMethodService _roleMethodService;
        private readonly ISessionService _sessionService;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AccountManager(IIdentityService identityService, ISessionService sessionService, IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserRoleService userRoleService, IRoleMethodService roleMethodService)
        {
            _identityService = identityService;
            _sessionService = sessionService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _roleMethodService = roleMethodService;
        }

        public async Task<BussinessLayerResult<bool?>> ForgatPassword(string email)
        {
            var response = new BussinessLayerResult<bool?>();

            var userResult = await _userService.GetAll(new LoadMoreFilter<UserFilter>
            {
                ContentCount = 1,
                PageCount = 0,
                Filter = new UserFilter
                {
                    Email = email,
                }
            });
            if (userResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(userResult.ErrorMessages);
                return response;
            }
            if (userResult.Result == null)
            {
                response.AddError(EErrorCode.AccountForgatPasswordEmailWrongError, "Lütfen eposta adresinizi doğru giriniz.");
                return response;
            }

            var identityResult = await _identityService.ForgatPassword(userResult.Result.Values[0].Id);
            if (identityResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(identityResult.ErrorMessages);
                return response;
            }
            response.Result = true;
            return response;

        }

        public async Task<BussinessLayerResult<SessionListDto>> Login(IdentityCheckDto ıdentity)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            var result = await _identityService.CheckPassword(ıdentity);
            if (result.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(result.ErrorMessages);
                return response;
            }
            if (result.Result == null)
            {
                response.AddError(EErrorCode.AccountLoginPasswordWrongError, "Kullanıcı adı veya şifre yanlış");
                return response;
            }
            DateTime? expiryDate = (!ıdentity.RememberMe) ? DateTime.Now.AddDays(1) : null;
            var sessionResult = await _sessionService.Add(new SessionDto
            {
                DeviceType = ıdentity.DeviceType,
                ExpiryDate = expiryDate,
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "",
                Key = Guid.NewGuid().ToString(),
                UserId = result.Result.UserId,

            });
            if (sessionResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                return response;
            }

            response.Result = sessionResult.Result;
            return response;



        }

        public async Task<BussinessLayerResult<SessionListDto>> Logout(string key)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            var sessionResult = await _sessionService.Get(key);
            if (sessionResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                return response;
            }

            if (sessionResult.Result == null)
            {
                response.AddError(EErrorCode.AccountLogoutPasswordWrongError, "oturum bilginiz alınamamıştır");
                return response;
            }
            var session = _mapper.Map<SessionDto>(sessionResult.Result);

            session.ExpiryDate = DateTime.Now;
            sessionResult = await _sessionService.Update(session);
            if (sessionResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                return response;
            }

            response.Result = sessionResult.Result;
            return response;

        }

        public async Task<BussinessLayerResult<SessionListDto>> SignUp(UserDto user)
        {
            user.Role = ERoleType.Unknown;
            var response = new BussinessLayerResult<SessionListDto>();
            var userResult = await _userService.Add(user);
            if (userResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(userResult.ErrorMessages);
                return response;
            }

            var sessionResult = await _sessionService.Add(new SessionDto
            {
                DeviceType = EDeviceType.None,
                ExpiryDate = DateTime.Now.AddDays(1),
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Key = Guid.NewGuid().ToString(),
                UserId = userResult.Result.Id
            });
            if (sessionResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                return response;
            }

            response.Result = sessionResult.Result;
            return response;

        }

        public async Task<BussinessLayerResult<SessionListDto>> GetSession(string key)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            var sessionResult = await _sessionService.Get(key);
            if (sessionResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(sessionResult.ErrorMessages);
                return response;
            }

            response.Result = sessionResult.Result;
            return response;
        }

        public async Task<BussinessLayerResult<List<EMethod>>> GetUserRoleMethods(long userId)
        {
            var response = new BussinessLayerResult<List<EMethod>>();
            var userRoleResult = await _userRoleService.GetAll(new LoadMoreFilter<UserRoleFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new UserRoleFilter
                {
                    UserId = userId,
                }
            });
            if (userRoleResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(userRoleResult.ErrorMessages);
                return response;
            }
            var methodList = new List<EMethod>();
            var roles = userRoleResult.Result.Values.Select(x => x.Role).ToList();
            roles.Add(ERoleType.Unknown);
            foreach (var role in roles)
            {
                var methodResult = await _roleMethodService.GetAll(new LoadMoreFilter<RoleMethodFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,
                    Filter = new RoleMethodFilter
                    {
                        Role = role,
                    }
                });
                if (methodResult.Status == EResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(methodResult.ErrorMessages);
                    return response;
                }
                methodList.AddRange(methodResult.Result.Values.Select(x => x.Method).ToList());

            }


            response.Result = methodList;
            return response;

        }
        public async Task<BussinessLayerResult<List<EMethod>>> GetPublicRoleMethods()
        {
            var response = new BussinessLayerResult<List<EMethod>>();

            var methodList = new List<EMethod>();


            var methodResult = await _roleMethodService.GetAll(new LoadMoreFilter<RoleMethodFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,
                Filter = new RoleMethodFilter
                {
                    Role = ERoleType.Unknown,
                }
            });
            if (methodResult.Status == EResultStatus.Error)
            {
                response.ErrorMessages.AddRange(methodResult.ErrorMessages);
                return response;
            }
            methodList.AddRange(methodResult.Result.Values.Select(x => x.Method).ToList());




            response.Result = methodList;
            return response;

        }

    }
}
