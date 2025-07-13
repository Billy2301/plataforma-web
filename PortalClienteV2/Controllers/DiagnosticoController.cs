using BL.IServices;
using BL.Services;
using culqi.net;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Response;

namespace PortalClienteV2.Controllers
{
    public class DiagnosticoController : BaseController
    {
        private readonly IPacienteService _pacienteService;
        Security security = null;

        public DiagnosticoController(IPacienteService pacienteService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService) : base(accesor, configuration, logService)
        {
            _pacienteService = pacienteService;
            security = securityKeys();
        }

        [HttpGet]
        public async Task<IActionResult> Pagos()
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            if (TempData["HcPaciente"] == null || string.IsNullOrEmpty(TempData["HcPaciente"].ToString()))
            {
                return RedirectToAction("Historias","HistoriaClinica");
            }
            try
            {
                ViewData["publicKey"] = security.public_key;
                string? HcPaciente = TempData["HcPaciente"].ToString();
                TempData.Keep("HcPaciente"); // Mantener Hc en TempData
                DiagnosticoViewModel model = new DiagnosticoViewModel();
                Paciente mPaciente = await _pacienteService.GetPaciente(Convert.ToInt32(HcPaciente));
                model.PacienteDni = mPaciente!.PacienteDni;
                model.ApePaterno = mPaciente.ApellidoPaterno;
                model.ApeMaterno = mPaciente.ApellidoMaterno;
                model.Nombres = mPaciente.Nombres;
                model.EmailContacto = mPaciente.EmailContacto;
                model.NroHc = mPaciente.NumeroHistoriaClinica;
                model.ListaDiagnostico = await _pacienteService.getHCDiagnostico(HcPaciente);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Historias", "HistoriaClinica");
            }          
        }

        [HttpPost]
        public async Task<IActionResult> Pagos(DiagnosticoViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            
            if (!ModelState.IsValid)
            {
                try
                {
                    ViewBag.Error = "Error al realizar el pago";
                    model.ListaDiagnostico = await _pacienteService.getHCDiagnostico(model.NroHc.ToString());
                    return View(model);
                }
                catch (Exception)
                {
                    return RedirectToAction("Historias", "HistoriaClinica");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Evaluaciones()
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            if (TempData["HcPaciente"] == null || string.IsNullOrEmpty(TempData["HcPaciente"].ToString()))
            {
                return RedirectToAction("Historias", "HistoriaClinica");
            }
            try
            {
                ViewData["publicKey"] = security.public_key;
                string? HcPaciente = TempData["HcPaciente"].ToString();
                TempData.Keep("HcPaciente"); // Mantener Hc en TempData
                DiagnosticoViewModel model = new DiagnosticoViewModel();
                Paciente mPaciente = await _pacienteService.GetPaciente(Convert.ToInt32(HcPaciente));
                model.PacienteDni = mPaciente!.PacienteDni;
                model.ApePaterno = mPaciente.ApellidoPaterno;
                model.ApeMaterno = mPaciente.ApellidoMaterno;
                model.Nombres = mPaciente.Nombres;
                model.EmailContacto = mPaciente.EmailContacto;
                model.NroHc = mPaciente.NumeroHistoriaClinica;
                model.ListaDiagnostico = await _pacienteService.getHCDiagnostico(HcPaciente);
                await SaveLog(1, "DiagnosticoViewModel", true, "");

                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "DiagnosticoViewModel", false, ex.Message);
                return RedirectToAction("Historias", "HistoriaClinica");
            }
        }

    }
}
