using BL.IServices;
using Entity.ClinicaModels;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    public class HistoriaClinicaController : BaseController
    {
        private readonly IPacienteService _pacienteService;
        public HistoriaClinicaController(IHttpContextAccessor accesor, IConfiguration configuration, IPacienteService paciente, ILogService logService) : base(accesor, configuration, logService)
        {
            _pacienteService = paciente;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ClearHcPacienteTempData]
        public async Task<IActionResult> Historias()
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            HistoriasViewModel model = new HistoriasViewModel();
            try { 
                model.ListPacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);
                if (model.ListPacientes != null && model.ListPacientes.Count == 1)
                {
                    if(model.ListPacientes[0].NumeroHistoriaClinica != 0)
                    {
                        await SaveLog(1, "HistoriasViewModel", true, "");
                        TempData["HcPaciente"] = model.ListPacientes[0].NumeroHistoriaClinica.ToString();
                        return RedirectToAction("Evaluaciones", "Diagnostico");
                    }
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "HistoriasViewModel", false, ex.Message);
                ViewBag.Error = "Ocurrió un error al obtener la lista de pacientes: " + ex.Message;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Historias(HistoriasViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;

            if (ModelState.IsValid)
            {
                TempData["HcPaciente"] = model.NroHistoriaClinica;
                return RedirectToAction("Evaluaciones","Diagnostico");
            }
            ViewBag.Error = "Error al seleccionar HC";
            return RedirectToAction(nameof(Historias));
        }

    }
}
