using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models;
using PortalClienteV2.Models.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using BL.IServices;
using BL.Services;
using Microsoft.AspNetCore.Identity;
using PortalClienteV2.Utilities.Helpers;
using PortalClienteV2.Utilities.Response;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Entity.CentralModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class HomeController : BaseController
    {
        private readonly IPagosTriajeService _pagosTriajeService;
        private readonly UserManager<IdentityUser> _userManagerService;
        private readonly IPacienteService _pacienteService;
        private readonly ITriajeOnlineService _triajeOnlineService;
        private readonly ICitasService _citasService;
        private readonly IReservaTratamientoService _reservaTratamientoService;

        public HomeController(ITriajeOnlineService triajeOnlineService,IPagosTriajeService pagosService, UserManager<IdentityUser> userManagerService, IPacienteService pacienteService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService, ICitasService citasService, IReservaTratamientoService reservaTratamientoService)
        : base(accesor, configuration, logService)
        {
            _pagosTriajeService = pagosService;
            _userManagerService = userManagerService;
            _pacienteService = pacienteService;
            _triajeOnlineService = triajeOnlineService;
            _citasService = citasService;
            _reservaTratamientoService = reservaTratamientoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                if (appUsuario == null) {
                    return RedirectToAction(actionName: "CerrarSession", controllerName: "Acceso");
                }
                HomeViewModel model = new HomeViewModel();
                model.ListPagoTriaje = await _pagosTriajeService.GetPagosTriajeList(appUsuario.Id);
                model.ListPacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);
                //model.ListTriajeOnline = await _triajeOnlineService.GetListTriajeOnlinePendienteAsync(appUsuario.Id);
                model.ListReservaTx = await _reservaTratamientoService.GetListReservaTratamientoPendienteAsync(appUsuario.Id);
                model.ListReservaDx = await _triajeOnlineService.GetReservaDiagnostico(appUsuario.Id);
                return View(model);
            }
            catch (Exception ex)
            {

                return RedirectToAction(actionName: "CerrarSession", controllerName: "Acceso");
            }

        }

        [HttpPost]
        public IActionResult Index(int? id)
        {
            if(id != null && id > 0)
            {
                //return RedirectToAction("Evaluaciones", "Apertura", new { id = id });
                return RedirectToAction("Evaluaciones", "Apertura", new { id = id });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetCitasProximas()
        {
            try
            {
                IQueryable<upCitasProximasPorUsuarioResult> query = await _citasService.getCitasProximas(User.Identity.Name);

                // Convertir el IQueryable a una lista para trabajar con LINQ
                var citasList = query.ToList();

                // Agrupar citas por nombre_paciente
                var citasAgrupadas = citasList
                    .GroupBy(c => new { c.nombre_paciente, c.numeroHistoriaClinica })
                    .Select(g => new
                    {
                        NombrePaciente = g.Key.nombre_paciente,
                        NumeroHistoriaClinica = g.Key.numeroHistoriaClinica,
                        Citas = g.Select(cita => new
                        {
                            // Formatear la fecha
                            FechaCita = cita.fechaCita?.ToString("dd/MM/yyyy"),
                            cita.numeroCita,
                            cita.Tipo,
                            horaCitaHasta = Helper.ConvertirHora(cita.horaCitaHasta),
                            horaCitaDesde = Helper.ConvertirHora(cita.horaCitaDesde),
                            cita.evaluacion,
                            cita.Area,
                            cita.Sede
                        }).ToList()
                    })
                    .ToList();

                return Json(new { data = citasAgrupadas });
            }
            catch (Exception ex)
            {
                await SaveLog(3, "upCitasProximasPorUsuarioResult", false, ex.Message);
                return Json(new { data = "" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarEvents()
        {
            var events = new List<object>();

            var classes = new[] {"Warning"}; /*new[] { "Success", "Danger", "Warning", "Primary", "Info" };*/
            var random = new Random();

            try
            {
                IQueryable<upCitasProximasPorUsuarioResult> query = await _citasService.getCitasProximas(User.Identity.Name);
                var citasList = query.ToList();
                foreach (var cita in citasList)
                {
                    var className = classes[random.Next(classes.Length)];
                    events.Add(new
                    {
                        groupId = cita.numeroHistoriaClinica,
                        id = string.Concat(cita.numeroHistoriaClinica, cita.numeroCita),
                        title = cita.nombre_paciente + " - Cita N° " + cita.numeroCita + " :  " + cita.evaluacion,  // Asumiendo que upCitas tiene una propiedad Title
                        start = cita.fechaCita?.Add(TimeSpan.Parse(Helper.ConvertirHora(cita.horaCitaDesde))).ToString("yyyy-MM-ddTHH:mm:ss"),  // Asumiendo que upCitas tiene una propiedad StartDate
                        end = cita.fechaCita?.Add(TimeSpan.Parse(Helper.ConvertirHora(cita.horaCitaHasta))).ToString("yyyy-MM-ddTHH:mm:ss"),  // Asumiendo que upCitas tiene una propiedad EndDate
                        allDay = false,
                        url = "",
                        extendedProps = new
                        {
                            calendar = cita.Area, //className,
                            descripcion = "Cita N° " + cita.numeroCita,
                            hc = cita.numeroHistoriaClinica,
                            inicio = Helper.ConvertirHora(cita.horaCitaDesde),
                            fin = Helper.ConvertirHora(cita.horaCitaHasta),
                            fecha = cita.fechaCita ?.ToString("dd 'de' MMMM 'de' yyyy"),
                            tipo = cita.Tipo,
                            nombre = cita.nombre_paciente,
                            evaluacion = cita.evaluacion == null ? string.Empty : cita.evaluacion,
                            area = cita.Area,
                            sede = cita.Sede,
                        },
                        //className = "",
                        editable = false,
                        description = "Descripción detallada del evento"
                    }); 
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "upCitasProximasPorUsuarioResult", false, ex.Message);
            }

            return Json(events);
        }

        public async Task<IActionResult> ShowDetallePagoTriaje(string id)
        {
            var entity = await _pagosTriajeService.GetPagosTriaje(Convert.ToInt32(id));
            ShowDetallePagoTriajeViewModel response = new ShowDetallePagoTriajeViewModel();
            response.descripcion = entity.Descripcion;
            response.sede = entity.Sede;
            response.monto = entity.SimboloMonedaPedido + " " + (entity.Monto / 100)!.Value.ToString("F0");
            response.fechaPago = entity.FechaCreacion!.Value.ToString("yyyy-MM-dd HH:mm");
            response.detalle = await _pagosTriajeService.GetDetallePagosTriaje(entity.CitasDetalle);

            return Json(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
}
