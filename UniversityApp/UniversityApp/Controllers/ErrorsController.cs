using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Presentation.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult EntityNotFound()
        {
            return View();
        }
    }
}
