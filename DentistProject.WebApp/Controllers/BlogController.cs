using AutoMapper;
using DentistProject.Business.Abstract;
using DentistProject.Dtos.AddOrUpdateDto;
using DentistProject.Dtos.Enum;
using DentistProject.Dtos.ListDto;
using DentistProject.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Reflection.Metadata;

namespace DentistProject.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;

        private readonly IToastNotification _toastNotification;
        private long loginUserId = 1;
        private UserListDto? loginUser;
        private List<EMethod> authMethod = new List<EMethod>();
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;
        public BlogController(IBlogService blogService, IToastNotification toastNotification, IMapper mapper, IBlogCategoryService blogCategoryService, IHttpContextAccessor contextAccessor, IAccountService accountService)
        {
            _blogService = blogService;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _blogCategoryService = blogCategoryService;
            _contextAccessor = contextAccessor;
            _accountService = accountService;

            var token = _contextAccessor.HttpContext?.Request?.Cookies?["AuthToken"] ?? "";
            var sessionResult = _accountService.GetSession(token);
            sessionResult.Wait();
            if (sessionResult.Result.Status == EResultStatus.Success)
            {
                loginUserId = sessionResult?.Result?.Result?.UserId ?? -1;
                loginUser = sessionResult?.Result?.Result?.User;
            }
            var methodResult = (loginUserId > 0)
                ? _accountService.GetUserRoleMethods(loginUserId)
                : _accountService.GetPublicRoleMethods();
            methodResult.Wait();
            if (methodResult.Result.Status == EResultStatus.Success)
            {
                authMethod = methodResult.Result.Result;
            }
        }

        // Blog Kısmı
        [HttpGet("BlogList")]
        public async Task<ActionResult> BlogList([FromQuery] int? page, [FromQuery] string? query)
        {
            if (!(authMethod.Contains(EMethod.BlogList) || authMethod.Contains(EMethod.BlogAllList)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var result = await _blogService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.BlogFilter>
            {
                ContentCount = 10,
                PageCount = page ?? 0,
                Filter = new Filters.Filter.BlogFilter
                {
                    UserId = authMethod.Contains(EMethod.BlogAllList) ? null : loginUserId,
                    Search=query
                }
            });
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.CanUpdate = ((authMethod?.Contains(EMethod.BlogUpdate)) ?? false);
                ViewBag.CanDelete = ((authMethod?.Contains(EMethod.BlogDelete)) ?? false);
                return View(result.Result);
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("BlogAdd")]
        public async Task<ActionResult> BlogAdd()
        {
            if (!(authMethod.Contains(EMethod.BlogAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            var categoryResult = await _blogCategoryService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.BlogCategoryFilter>
            {
                ContentCount = int.MaxValue,
                PageCount = 0,

            });
            if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
            {
                ViewBag.Categories = categoryResult.Result?.Values;
            }
            return View();
        }
        [HttpPost("BlogAdd")]
        public async Task<ActionResult> BlogAdd([FromForm] BlogDto blog)
        {
            if (!(authMethod.Contains(EMethod.BlogAdd)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/");
            }
            blog.UserId = loginUserId;

            var result = await _blogService.Add(blog);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return RedirectToAction("BlogList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(blog);
        }
        [HttpGet("/BlogDelete/{id:long}")]
        public async Task<ActionResult> BlogDelete(long id)
        {
            if (!(authMethod.Contains(EMethod.BlogDelete) || authMethod.Contains(EMethod.BlogAllDelete)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }
            var result = await _blogService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllDelete)))
                {   
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı silebilirsiniz");
                    return Redirect("/BlogList");
                }
            }
            var message2 = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message2);


            result = await _blogService.Delete(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {

                return Redirect("/BlogList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect("/BlogList");
        }


        [HttpGet("/BlogUpdate/{id:long}")]
        public async Task<IActionResult> BlogUpdate(long id)
        {
            if (!(authMethod.Contains(EMethod.BlogUpdate) || authMethod.Contains(EMethod.BlogAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }

            var result = await _blogService.Get(id);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                if (result.Result.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllUpdate)))
                {
                    _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı güncelleyebilirsiniz");
                    return Redirect("/BlogList");
                }
                var categoryResult = await _blogCategoryService.GetAll(new Dtos.Filter.LoadMoreFilter<Filters.Filter.BlogCategoryFilter>
                {
                    ContentCount = int.MaxValue,
                    PageCount = 0,

                });
                if (categoryResult.Status == Dtos.Enum.EResultStatus.Success)
                {
                    ViewBag.Categories = categoryResult.Result?.Values;
                }
                return View(_mapper.Map<BlogDto>(result.Result));
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpPost("/BlogUpdate/{id:long}")]
        public async Task<ActionResult> BlogUpdate([FromForm] BlogDto blog)
        {

            if (!(authMethod.Contains(EMethod.BlogUpdate) || authMethod.Contains(EMethod.BlogAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz yok");
                return Redirect("/BlogList");
            }
            if (blog.UserId != loginUserId && !(authMethod.Contains(EMethod.BlogAllUpdate)))
            {
                _toastNotification.AddAlertToastMessage("Yetkiniz Bulunmamamktadır. Sadece kendi bloglarınızı güncelleyebilirsiniz");
                return Redirect("/BlogList");
            }
            var result = await _blogService.Update(blog);
            if (result.Status == Dtos.Enum.EResultStatus.Success)
            {
                return RedirectToAction("BlogList");
            }
            var message = string.Join(",", result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View(blog);
        }
    }
}
