using HW_4.Data.Entities;
using HW_4.Models.User;
using HW_4.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace HW_4.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IValidationService _validationService;

        public UserController(DataContext dataContext, IValidationService validationService)
        {
            _dataContext = dataContext;
            _validationService = validationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Profile(String? id)
        {
            UserProfileViewModel model = new();
            if(id == null)
            {
                if (HttpContext.User.Identity?.IsAuthenticated ?? false)
                {
                    model.IsPersonal = true;
                    String sid = HttpContext
                         .User
                         .Claims
                         .First(claim => claim.Type == ClaimTypes.Sid)
                         .Value;
                    model.User = _dataContext
                        .Users
                        .Find(Guid.Parse(sid));

                }
                else
                {
                    model.IsPersonal = false;
                    model.User = null;
                }
            }
            else
            {
                model.IsPersonal = false;
                model.User = _dataContext
                    .Users
                    .FirstOrDefault(u => u.Login == id);
            }
            ViewData["id"] = id;
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProfile(String newName, String newEmail, String newPhone, String newLogin)
        {
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                
                String sid = HttpContext
                     .User
                     .Claims
                     .First(claim => claim.Type == ClaimTypes.Sid)
                     .Value;
                var user = _dataContext
                    .Users
                    .Find(Guid.Parse(sid));
                if(user != null)
                {
                    user.Fullname = newName;
                    user.Email = newEmail;
                    user.Phone = newPhone;
                    user.Login = newLogin;

                    await _dataContext.SaveChangesAsync();

                    return Json(new { status = 200 });
                }

            }
            return Json(new { status = 401 });
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteProfile()
        {
            var user = this.GetAuthUser();
            if (user == null) { return Json(new { status = 401 });}

            user.DeleteDt = DateTime.Now;

            user.Fullname = "";
            user.Email = "";
            user.Phone = "";
            user.Login = "";
            user.PasswordDk = "";
            user.PasswordSalt = "";
            user.Avatar = "";

            if (user.Avatar != null)
            {  
                String dir = Directory.GetCurrentDirectory();
                String avatarFileName = Path.Combine(dir, "wwwroot", "avatars", user.Avatar);
                if (System.IO.File.Exists(avatarFileName))
                {
                    System.IO.File.Delete(avatarFileName);
                }
                user.Avatar = null;
            }

            await _dataContext.SaveChangesAsync();

            return Json(new {status = 200});    
        }

        [HttpDelete]
        public async Task<JsonResult> SuspendProfile()
        {
            var user = this.GetAuthUser();
            if (user == null) { return Json(new { status = 401 }); }

            user.DeleteDt = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return Json(new { status = 200 });
        }

        public async Task<JsonResult> UpdateAvatarProfile(NewAvatarModel avatar) 
        {
            var user = this.GetAuthUser();
            if (user == null) { return Json(new { status = 401 }); }



            if (avatar.Avatar != null || avatar.Avatar?.Length > 0)
            {



                bool isFormValid = true;
                int dotPosition = 0;
                String ext = null!;
                

                dotPosition = avatar.Avatar.FileName.LastIndexOf(".");

                if (dotPosition != -1)
                {
                    ext = avatar.Avatar.FileName.Substring(dotPosition);
                }

                if (!_validationService.IsAvatarValid(ext))
                {
                   
                    isFormValid = false;
                }

                if (isFormValid)
                {
                    String dir = Directory.GetCurrentDirectory();
                    if (user.Avatar != null)
                    {
                       
                        String avatarFileName = Path.Combine(dir, "wwwroot", "avatars", user.Avatar);
                        if (System.IO.File.Exists(avatarFileName))
                        {
                            System.IO.File.Delete(avatarFileName);
                        }

                    }

                   
                    String savedName;
                    String fileName;
                    do
                    {
                        fileName = Guid.NewGuid() + ext;
                        savedName = Path.Combine(dir, "wwwroot", "avatars", fileName);
                    }
                    while (System.IO.File.Exists(savedName));

                    using Stream stream = System.IO.File.OpenWrite(savedName);
                    avatar.Avatar.CopyTo(stream);

                    user.Avatar = fileName;
                }
                await _dataContext.SaveChangesAsync();
              
            }

            return Json(new { status = 200 });

        }
        private Data.Entities.User? GetAuthUser()
        {
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {

                String sid = HttpContext
                     .User
                     .Claims
                     .First(claim => claim.Type == ClaimTypes.Sid)
                     .Value;
                return _dataContext
                    .Users
                    .Find(Guid.Parse(sid));
            }
            return null;
        }

        
    }
}
