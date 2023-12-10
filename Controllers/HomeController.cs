using HW_4.Data.Entities;
using HW_4.Models;
using HW_4.Models.Home;
using HW_4.Services.Hash;
using HW_4.Services.Validation;
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
        private readonly IValidationService _validationService;


        public HomeController(ILogger<HomeController> logger, IHashService hashService, DataContext dataContext, IValidationService validationService)
        {
            _logger = logger;
            _hashService = hashService;
            _dataContext = dataContext;
            _validationService = validationService;
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
            SignupViewModel viewModel = new();

            if (HttpContext.Session.Keys.Contains("formStatus"))
            {
                viewModel.FormStatus = Convert.ToBoolean(
                    HttpContext.Session.GetString("formStatus"));
                HttpContext.Session.Remove("formStatus");

                if (viewModel.FormStatus ?? false)
                {
                    viewModel.FormModel = null;
                    viewModel.FormValidation = null;
                }
                else
                {
                    viewModel.FormModel = JsonSerializer.Deserialize<SignupFormModel>(
                        HttpContext.Session.GetString("formModel")!);
                    HttpContext.Session.Remove("formModel");
                    viewModel.FormValidation = JsonSerializer.Deserialize<SignupFormValidation>(
                       HttpContext.Session.GetString("formValidation")!);
                    HttpContext.Session.Remove("formValidation");
                }
            }


            return View(viewModel);
        }

        [HttpPost]
        public RedirectToActionResult SigupForm(SignupFormModel model)
        {

            SignupFormValidation results = new();
            bool isFormValid = true;

            if (!_validationService.IsFullnameValid(model.Fullname))
            {
                results.FullnameErrorMessage = "Name must start with a capital letter and may only contain letters.";
                isFormValid = false;
            }
            if (!_validationService.IsEmailValid(model.Email))
            {
                results.EmailErrorMessage = "Email may contain uppercase and lowercase latin letters and must contain symbol @";
                isFormValid = false;
            }
            if (!_validationService.IsPhoneValid(model.Phone))
            {
                results.PhoneErrorMessage = " Phone must be 0222222222 or 022-222-22-22.";
                isFormValid = false;
            }
            if (!_validationService.IsLoginValid(model.Login))
            {
                results.LoginErrorMessage = "Is required";
                isFormValid = false;
            }
            if (!_validationService.IsPasswordValid(model.Password))
            {
                results.PasswordErrorMessage = "Password may contain at least 5 characters.";
                isFormValid = false;
            }
            if (model.Password != model.Repeat || model.Repeat == null)
            {
                results.RepeatErrorMessage = "Password do not match.";
                isFormValid = false;
            }

            int dotPosition = 0;
            String ext = null!;
            if (model.Avatar != null && model.Avatar.Length > 0)
            {
                dotPosition = model.Avatar.FileName.LastIndexOf(".");

                if (dotPosition != -1)
                {
                    ext = model.Avatar.FileName.Substring(dotPosition);
                }

                if (!_validationService.IsAvatarValid(ext))
                {
                    results.AvatarErrorMessage = "Unable to upload a file without extension. jpg, png extansions are acceptable";
                    isFormValid = false;
                }

                if (isFormValid)
                {

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


            if (isFormValid)
            {

                String salt = _hashService.HexString(Guid.NewGuid().ToString());
                String dk = _hashService.HexString(salt + model.Password);

                _dataContext.Users.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Fullname = model.Fullname,
                    Email = model.Email,
                    Phone = model.Phone,
                    Login = model.Login,
                    PasswordSalt = salt,
                    PasswordDk = dk,
                    RegisterDt = DateTime.Now
                });

                _dataContext.SaveChanges();

            }
            if (!isFormValid)
            {
                HttpContext.Session.SetString("formModel", JsonSerializer.Serialize(model));
                HttpContext.Session.SetString("formValidation", JsonSerializer.Serialize(results));
            }
            HttpContext.Session.SetString("formStatus", isFormValid.ToString());

            return RedirectToAction(nameof(SignUp));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}