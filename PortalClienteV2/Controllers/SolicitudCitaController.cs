using BL.IServices;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class SolicitudCitaController : BaseController
    {
        private readonly IConfigSistemaService _configSistemaService;
        private readonly ITriajeOnlineService _triajeOnlineService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPacienteService _pacienteService;
        private readonly UserManager<IdentityUser> _userManagerService;



        public SolicitudCitaController(IPacienteService pacienteService, UserManager<IdentityUser> userManagerService, IUsuarioService usuarioService, IConfigSistemaService configSistemaService, ITriajeOnlineService triajeOnlineService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService)
        : base(accesor, configuration, logService)
        {
            _configSistemaService = configSistemaService;
            _triajeOnlineService = triajeOnlineService;
            _usuarioService = usuarioService;
            _userManagerService = userManagerService;
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<IActionResult> NuevaCita()
        {

            NuevoTriajeViewModel model = new NuevoTriajeViewModel();
            List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);

            if (Pacientes == null || Pacientes.Count == 0)
            {
                TempData["hc"] = 0;
                return RedirectToAction("Solicitud", "SolicitudCita");

            }

            var listaHistoriaClinica = Pacientes.Select(p => new SelectListItem
            {
                Text = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                Value = p.NumeroHistoriaClinica.ToString(),
            }).ToList();
            listaHistoriaClinica.Insert(0, new SelectListItem { Text = "Nueva persona en atenderse", Value = "0" });
            model.ListHistoriaClinica = listaHistoriaClinica;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaCita(NuevoTriajeViewModel model)
        {

            if (ModelState.IsValid)
            {
                TempData["hc"] = model.HistoriaClinica;
                return RedirectToAction("Solicitud", "SolicitudCita");

            }
            List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);

            var listaHistoriaClinica = Pacientes.Select(p => new SelectListItem
            {
                Text = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                Value = p.NumeroHistoriaClinica.ToString(),
            }).ToList();
            listaHistoriaClinica.Insert(0, new SelectListItem { Text = "Nueva persona en atenderse", Value = "0" });
            model.ListHistoriaClinica = listaHistoriaClinica;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Solicitud()
        {
            ViewBag.datos = null;

            if (TempData["hc"] == null)
            {
                return RedirectToAction("NuevaCita", "SolicitudCita");
            }
            try
            {
                var _hc = Convert.ToInt32(TempData["hc"]);
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(appUsuario.Id.ToString());
                Paciente? paciente = await _pacienteService.GetPaciente(Convert.ToInt32(_hc));
                SolicitudViewModel viewModel = new SolicitudViewModel();
                viewModel.Tab1.Hc = _hc;
                if (paciente != null)
                {
                    ViewBag.datos = true;

                    viewModel.Tab1.NombresPaciente = paciente.Nombres;
                    viewModel.Tab1.ApellidoPaternoPaciente = paciente.ApellidoPaterno;
                    viewModel.Tab1.ApellidoMaternoPaciente = paciente.ApellidoMaterno;
                    viewModel.Tab1.TipoDocumento = "DNI";
                    viewModel.Tab1.PacienteDNI = paciente.PacienteDni;
                    viewModel.Tab1.FechaNacimientoPaciente = paciente.FechaNacimiento.ToString("dd/MM/yyyy");
                    viewModel.Tab1.Colegio = paciente.CentroEducativo;
                }
                if (userRole != null)
                {
                    viewModel.Tab1.UsuarioId = appUsuario.Id;
                    viewModel.Tab1.ApoderadoTelCelular = userRole.Telefono;
                    viewModel.Tab1.ApoderadoNombre = (userRole.Nombres + " " + userRole.ApellidoPaterno + " " + userRole.ApellidoMaterno).Trim();
                    viewModel.Tab1.EmailRegistro = userRole.Email;
                    viewModel.Tab1.ContactoTipoDoc = userRole.TipoDoc;
                    viewModel.Tab1.ContactoNroDoc = userRole.Dni;

                    if (string.IsNullOrEmpty(userRole.ApellidoPaterno) ||
                    string.IsNullOrEmpty(userRole.ApellidoMaterno) ||
                    string.IsNullOrEmpty(userRole.Nombres) ||
                    string.IsNullOrEmpty(userRole.Telefono) ||
                    !userRole.FechaNacimiento.HasValue ||
                    string.IsNullOrEmpty(userRole.Dni) ||
                    !userRole.Sexo.HasValue)
                    {
                        viewModel.Tab1.GuardarEnPerfil = false;  // modificar a "TRUE" si lo requieren
                        //viewModel.Tab1.GuardarEnPerfil = true;
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "SolicitudViewModel", false, ex.Message);
                return RedirectToAction("NuevaSolicitud", "SolicitudCita");
            }

        }

    }
}
