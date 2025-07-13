using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Utilities.Helpers;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class GuiaRapidaController : Controller
    {
        public IActionResult Asistencia()
        {
            return View();
        }
    }
}
