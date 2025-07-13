using BL.IServices;
using culqi.net;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models.ViewModel;

namespace PortalClienteV2.Controllers
{
    public class TratamientoController : BaseController
    {
        private readonly IPacienteService _pacienteService;
        Security security = null;
        public TratamientoController(IPacienteService pacienteService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService) : base(accesor, configuration, logService)
        {
            _pacienteService = pacienteService;
            security = securityKeys();
        }
        [HttpGet]
        public async Task<IActionResult> Terapias()
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
                TratamientoViewModel model = new TratamientoViewModel();
                Paciente mPaciente = await _pacienteService.GetPaciente(Convert.ToInt32(HcPaciente));
                model.PacienteDni = mPaciente!.PacienteDni;
                model.ApePaterno = mPaciente.ApellidoPaterno;
                model.ApeMaterno = mPaciente.ApellidoMaterno;
                model.Nombres = mPaciente.Nombres;
                model.EmailContacto = mPaciente.EmailContacto;
                model.NroHc = mPaciente.NumeroHistoriaClinica;
                model.ListaTratamiento = await _pacienteService.getHCTratamiento(HcPaciente);
                await SaveLog(1, "TratamientoViewModel", true, "");

                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "TratamientoViewModel", false, ex.Message);

                return RedirectToAction("Historias", "HistoriaClinica");
            }
        }

    }
}
