using BL.IServices;
using BL.Services;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class ReservarTratamientoController : BaseController
    {
        private readonly IPacienteService _pacienteService;
        private readonly IReservaTratamientoService _reservaTratamientoService;
        private readonly IUsuarioService _usuarioService;
        private readonly UserManager<IdentityUser> _userManagerService;
        public ReservarTratamientoController(IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService, IPacienteService pacienteService, IReservaTratamientoService reservaTratamientoService, IUsuarioService usuarioService, UserManager<IdentityUser> userManagerService) : base(accesor, configuration, logService)
        {
            _pacienteService = pacienteService;
            _reservaTratamientoService = reservaTratamientoService;
            _userManagerService = userManagerService;
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> InicioDeReserva()
        {
            return View();
        }

        [HttpGet]
        public  IActionResult InformeTratamiento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InformeTratamiento(InformeTratamientoViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            ViewBag.Info = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                    int reservaPendienteId = await GetReservaPendienteTipoWithHc(appUsuario.Id, 1, model.Hc);
                    bool tienePendiente = reservaPendienteId > 0 ? true : false;
                    if (tienePendiente) // true && true
                    {
                        ViewBag.info = "Ya tiene una solicitud pendiente para : " + model.Nombres;
                        return View(model);
                    }


                    if (model.OpcionesSeleccionadas.Count() > 0)
                    {
                        ReservaTratamiento entity = new ReservaTratamiento
                        {
                            CreatedBy = User.Identity?.Name ?? "Sistema", // Evita nulos
                            CreatedDate = DateTime.Now,
                            SedeId = int.TryParse(model.SedeId, out int sedeId) ? sedeId : 0, // Conversión segura
                            EmailRegistro = User.Identity?.Name ?? "SinCorreo",
                            RefNroHistoriaClinica = model.Hc,
                            ReservaTratamientoEstadoId = 1,
                            RefUsuarioId = appUsuario?.Id,
                            Tipo = 1
                        };

                        entity = await _reservaTratamientoService.Guardar(entity);

                        if (entity?.ReservaTratamientoId > 0)
                        {
                            foreach (var item in model.OpcionesSeleccionadas)
                            {
                                DetalleReservaTratamiento eDetalle = new DetalleReservaTratamiento
                                {
                                    HistorialPacienteId = item,
                                    ReservaTratamientoId = entity.ReservaTratamientoId
                                };
                                await _reservaTratamientoService.GuardarDetalle(eDetalle);
                            }
                        }
                        ViewBag.Exito = "Su información ha sido guardada. Una asesora se pondrá en contacto con usted pronto.";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Error = "Debe seleccionar una terapia.";
                        return View();
                    }

                }
                else
                {
                    ViewBag.Error = "Faltan datos";
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View();
            }


        }

        [HttpGet]
        public async Task<IActionResult> NoInformeTratamiento()
        {
            try
            {
                NoInformeTratamientoViewModel viewModel = new NoInformeTratamientoViewModel();
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(appUsuario.Id.ToString());
                if (userRole != null)
                {
                    //viewModel.UsuarioId = appUsuario.Id;
                    viewModel.ContactoTelefono = string.IsNullOrEmpty(userRole.Telefono) ? "" : userRole.Telefono;
                    viewModel.ContactoCorreo = string.IsNullOrEmpty(userRole.Email) ? "" : userRole.Email;
                    viewModel.ContactoWhatsApp = string.IsNullOrEmpty(userRole.Telefono) ? "" : userRole.Telefono;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            
            
        }

        [HttpPost]
        public async Task<IActionResult> NoInformeTratamiento(NoInformeTratamientoViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            ViewBag.info = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                    int reservaPendienteId = await GetReservaPendienteTipo(appUsuario.Id,2);
                    bool tienePendiente = reservaPendienteId > 0 ? true : false;
                    if (tienePendiente && model.flag) // true && true
                    {
                        ViewBag.info = "Ya tiene una solicitud pendiente";
                        return View(model);
                    }

                    string textOrientacion = model.OrientacionSeleccionada switch
                    {
                        "1" => $"Orientación por WhatsApp al número: +51 {model.ContactoWhatsApp}",
                        "2" => $"Orientación por llamada telefónica al número: {model.ContactoTelefono}",
                         _  => $"Orientación por correo electrónico al mail: {model.ContactoCorreo}",
                    };

                    ReservaTratamiento? entity;
                    if (!tienePendiente)
                    {
                        entity = new ReservaTratamiento
                        {
                            CreatedBy = User.Identity?.Name ?? "Sistema", // Evita nulos
                            CreatedDate = DateTime.Now,
                            EmailRegistro = User.Identity?.Name ?? "SinCorreo",
                            ReservaTratamientoEstadoId = 1,
                            RefUsuarioId = appUsuario?.Id,
                            OrientacionMedio = textOrientacion,
                            Tipo = 2
                        };
                    }
                    else
                    {
                        entity = await _reservaTratamientoService.GetReserva(reservaPendienteId);
                        entity.OrientacionMedio = textOrientacion;
                    }

                    entity = await _reservaTratamientoService.Guardar(entity);
                    ViewBag.Exito = "Su información ha sido guardada. Una asesora se pondrá en contacto con usted pronto.";
                    return View(model);
                }
                else
                {
                    ViewBag.Error = "Faltan datos";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

                
        }

        [HttpGet]
        public async Task<IActionResult> GetListInformeTratamiento(string nroDoc)
        {
            try
            {
                Paciente? mPaciente = await _pacienteService.GetPacienteByDni(nroDoc);
                PacienteTx model = new PacienteTx();
                if (mPaciente != null)
                {
                    model.Nombre = mPaciente.Nombres;
                    model.ApellidoPaterno = mPaciente.ApellidoPaterno;
                    model.ApellidoMaterno = mPaciente.ApellidoMaterno;
                    model.Hc = mPaciente.NumeroHistoriaClinica.ToString();
                    model.Existe = "si";
                    model.listInforme = await _pacienteService.getHistorialDiagnosticoByHc(mPaciente.NumeroHistoriaClinica);
                }
                else
                {
                    model.Existe = "no";
                }
                return Json(model);

            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvCitasPublica", false, ex.Message);
                // Registra el error si tienes un sistema de logs
                return StatusCode(500, new { message = "Ocurrió un error al obtener los informes.", error = ex.Message });
            }
        }

        public async Task<int> GetReservaPendienteTipo(string userId, int tipo)
        {
            int output = 0;
            List<UvReservaTratamientoSearch> list = await _reservaTratamientoService.GetListReservaTratamientoPendienteTipoAsync(userId, tipo);
            if (list.Count() > 0)
            {
                output = list[0].ReservaTratamientoId;
            }

            return output;
        }

        public async Task<int> GetReservaPendienteTipoWithHc(string userId, int tipo, int hc)
        {
            int output = 0;
            List<UvReservaTratamientoSearch> list = await _reservaTratamientoService.GetListReservaTratamientoPendienteTipoWhitHistoriaAsync(userId, tipo, hc);
            if (list.Count() > 0)
            {
                output = list[0].ReservaTratamientoId;
            }

            return output;
        }
    }
}
