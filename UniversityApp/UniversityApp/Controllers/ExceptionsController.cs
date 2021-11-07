using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Presentation.Controllers
{
    public class ExceptionsController : Controller
    {
        public IActionResult EntityNotFound()
        {
            return View();
        }
    }
}
