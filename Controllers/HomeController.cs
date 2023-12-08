using HW_4.Data.Entities;
using HW_4.Models;
using HW_4.Models.Home;
using HW_4.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HW_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;
        private readonly DataContext _dataContext;

        public HomeController(ILogger<HomeController> logger, IHashService hashService, DataContext dataContext)
        {
            _logger = logger;
            _hashService = hashService;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult SigupForm(SignupFormModel model)
        {

            SignupFormValidation results = new();
            bool isFormValid = true;
            if (String.IsNullOrEmpty(model.Fullname)) 
            {
                results.LoginErrorMessage = "Name must start with a capital letter and may only contain letters.";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Email))
            {
                results.EmailErrorMessage = "Email may contain uppercase and lowercase latin letters and must contain symbol @";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Phone))
            {
                results.PhoneErrorMessage = " Phone must be 0222222222 or 022-222-22-22.";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Login))
            {
                results.LoginErrorMessage = "Is required";
                isFormValid = false;
            }
            if (String.IsNullOrEmpty(model.Password))
            {
                results.PasswordErrorMessage = "Password may contain at least 5 characters.";
                isFormValid = false;
            }
            if (model.Password != model.Repeart)
            {
                results.RepeatErrorMessage = "Password do not match.";
                isFormValid = false;
            }
           
            if (isFormValid && model.Avatar != null && model.Avatar.Length > 0)
            {
               int dotPosition = model.Avatar.FileName.LastIndexOf(".");
                if(dotPosition == -1)
                {
                    results.AvatarErrorMessage = "Unable to upload a file without extension";
                    isFormValid = false;
                }
                else
                {
                    String ext = model.Avatar.FileName.Substring(dotPosition);
                    String dir = Directory.GetCurrentDirectory();
                    String savedName;
                    String fileName;
                    do
                    {
                        fileName = Guid.NewGuid() + ext;
                        savedName = Path.Combine(dir, "wwwroot", "avatars", fileName);
                    } 
                    while (System.IO.File.Exists(savedName));

                    using Stream stream = System.IO.File.OpenWrite(savedName);
                    model.Avatar.CopyTo(stream);
                }
            }

            HttpContext.Session.SetString("formModel", JsonSerializer.Serialize(model));
            HttpContext.Session.SetString("formValidation", JsonSerializer.Serialize(results));

            return RedirectToAction(nameof(SignUp));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}