using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class EncryptionController : Controller
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public EncryptionController()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyPassword(string password, string hash)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hash, password);

            if (result == PasswordVerificationResult.Success)
            {
                ViewBag.SwalMessage = "Password is correct!";
                ViewBag.SwalIcon = "success";
            }
            else
            {
                ViewBag.SwalMessage = "Invalid password!";
                ViewBag.SwalIcon = "error";
            }

            return View("Index");
        }
    }
}
