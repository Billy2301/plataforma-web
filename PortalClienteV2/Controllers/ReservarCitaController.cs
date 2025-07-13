using BL.IServices;
using BL.Services;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class ReservarCitaController : BaseController
    {
        private readonly IConfigSistemaService _configSistemaService;
        private readonly ITriajeOnlineService _triajeOnlineService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPacienteService _pacienteService;
        private readonly IEvaluacionCitasService _evaluacionCitaService;
        private readonly IPagoCitaService _pagoCitaService;
        private readonly IPagosTriajeService _pagosTriajeService;
        private readonly UserManager<IdentityUser> _userManagerService;
        public ReservarCitaController(IPacienteService pacienteService, UserManager<IdentityUser> userManagerService, IUsuarioService usuarioService, IConfigSistemaService configSistemaService, ITriajeOnlineService triajeOnlineService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService, IEvaluacionCitasService evaluacionCita, IPagoCitaService pagoCitaService, IPagosTriajeService pagosTriajeService)
        : base(accesor, configuration, logService)
        {
            _configSistemaService = configSistemaService;
            _triajeOnlineService = triajeOnlineService;
            _usuarioService = usuarioService;
            _userManagerService = userManagerService;
            _pacienteService = pacienteService;
            _evaluacionCitaService = evaluacionCita;
            _pagoCitaService = pagoCitaService;
            _pagosTriajeService = pagosTriajeService;
        }


        [HttpGet]
        public async Task<IActionResult> InicioDeReserva()
        {
            InicioDeRecervaViewModel model = new InicioDeRecervaViewModel();
            List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);

            if (Pacientes == null || Pacientes.Count == 0)
            {
                TempData["hc"] = 0;
                return RedirectToAction("FormularioDeReserva", "ReservarCita", new { doAction = "Nuevo" });

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
        public async Task<IActionResult> InicioDeReserva(InicioDeRecervaViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.pagoId = null;
            ViewBag.reservaId = null;

            if (ModelState.IsValid)
            {
                if (model.HistoriaClinica == 0)
                {
                    TempData["hc"] = model.HistoriaClinica;
                    return RedirectToAction("FormularioDeReserva", "ReservarCita", new { doAction = "Nuevo" });
                }
                else
                {
                    Paciente? mPaciente = await _pacienteService.GetPaciente(Convert.ToInt32(model.HistoriaClinica));
                    if (mPaciente == null)
                    {
                        ViewBag.Error = "No se encontró el paciente en el sistema.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(mPaciente.PacienteDni))
                        {
                            //ViewBag.Error = "El paciente no tiene un DNI registrado.";
                            TempData["hc"] = model.HistoriaClinica;
                            return RedirectToAction("FormularioDeReserva", "ReservarCita", new { doAction = "Existe" });
                        }
                        else
                        {
                            var(reservaPendienteId, pagoId) = await GetReservaPendiente2(mPaciente.PacienteDni, "PENDIENTE");
                            bool tienePendiente = reservaPendienteId > 0 ? true : false;
                            if (!tienePendiente)
                            {
                                TempData["hc"] = model.HistoriaClinica;
                                return RedirectToAction("FormularioDeReserva", "ReservarCita", new { doAction = "Existe" });
                            }
                            else
                            {
                                ViewBag.Error = "Ya existe una reserva pendiente para esta persona.";
                                ViewBag.reservaId = reservaPendienteId;
                                ViewBag.pagoId = pagoId;
                            }
                        }
                    }
                }
            }

            // Solo cargar la lista si hay un error o si ModelState no es válido
            if (!ModelState.IsValid || ViewBag.Error != null)
            {
                List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);
                var listaHistoriaClinica = Pacientes.Select(p => new SelectListItem
                {
                    Text = $"{p.Nombres} {p.ApellidoPaterno} {p.ApellidoMaterno}",
                    Value = p.NumeroHistoriaClinica.ToString(),
                }).ToList();
                listaHistoriaClinica.Insert(0, new SelectListItem { Text = "Nueva persona en atenderse", Value = "0" });
                model.ListHistoriaClinica = listaHistoriaClinica;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FormularioDeReserva(int? triajeId = null, int? pagoId = null, string? doAction = null)
        {
            ViewBag.datos = null;
            ViewBag.Pago = pagoId;
            ViewBag.doAction = doAction;

            if (triajeId != null && int.TryParse(triajeId.ToString(), out int parsedTriajeId) && parsedTriajeId > 0)
            {
                TriageOnline entity = await _triajeOnlineService.GetTriageOnlineById(Convert.ToInt32(triajeId));
                if (entity != null)
                {
                    TempData["hc"] = entity.RefNroHistoriaClinica != null ? entity.RefNroHistoriaClinica : 0;
                    var _hc = Convert.ToInt32(TempData["hc"]);
                    FormularioDeReservaViewModel viewModel = new FormularioDeReservaViewModel();
                    viewModel.Tab1.TriageOnlineID = Convert.ToInt32(triajeId);
                    viewModel.Tab1.Hc = _hc;
                    viewModel.Tab1.PagoCitaID = Convert.ToInt32(pagoId);
                    viewModel.Tab1.NombresPaciente = string.IsNullOrEmpty(entity.NombresPaciente) ? "" : entity.NombresPaciente;
                    viewModel.Tab1.ApellidoPaternoPaciente = string.IsNullOrEmpty(entity.ApellidoPaternoPaciente) ? "" : entity.ApellidoPaternoPaciente;
                    viewModel.Tab1.ApellidoMaternoPaciente = string.IsNullOrEmpty(entity.ApellidoMaternoPaciente) ? "" : entity.ApellidoMaternoPaciente;
                    viewModel.Tab1.TipoDocumento = string.IsNullOrEmpty(entity.TipoDocumento) ? "" : entity.TipoDocumento;
                    viewModel.Tab1.PacienteDNI = string.IsNullOrEmpty(entity.PacienteDni) ? "" : entity.PacienteDni;
                    viewModel.Tab1.FechaNacimientoPaciente = Convert.ToDateTime(entity.FechaNacimientoPaciente).ToString("dd/MM/yyyy");
                    viewModel.Tab1.Colegio = string.IsNullOrEmpty(entity.Colegio) ? "" : entity.Colegio;
                    viewModel.Tab1.Procedencia = string.IsNullOrEmpty(entity.Procedencia) ? "" : entity.Procedencia;
                    viewModel.Tab1.Colegio = string.IsNullOrEmpty(entity.PacienteCentroEducativo) ? "" : entity.PacienteCentroEducativo;
                    viewModel.Tab1.Parentesco = string.IsNullOrEmpty(entity.ParentescoPaciente) ? "" : entity.ParentescoPaciente;

                    if (!string.IsNullOrEmpty(entity.PacienteGrado))
                    {
                        var nivelGrado = entity.PacienteGrado.Split(" - ");
                        viewModel.Tab1.NivelEducacion = nivelGrado.Length > 0 ? nivelGrado[0].ToString() : null;
                        viewModel.Tab1.PacienteGrado = nivelGrado.Length > 1 ? nivelGrado[1].ToString() : null;
                    }

                    

                    viewModel.Tab1.UsuarioId = string.IsNullOrEmpty(entity.RefUsuarioId)? "" : entity.RefUsuarioId;
                    viewModel.Tab1.ApoderadoNombre = string.IsNullOrEmpty(entity.ApoderadoNombre) ? "" : entity.ApoderadoNombre;
                    viewModel.Tab1.ContactoTelCelular = string.IsNullOrEmpty(entity.ApoderadoTelCelular) ? "" : entity.ApoderadoTelCelular;
                    viewModel.Tab1.ContactoCorreo = string.IsNullOrEmpty(entity.EmailRegistro) ? "" : entity.EmailRegistro;
                    viewModel.Tab1.ContactoTipoDoc = string.IsNullOrEmpty(entity.ContactoTipoDoc) ? "" : entity.ContactoTipoDoc;
                    viewModel.Tab1.ContactoNroDoc = string.IsNullOrEmpty(entity.ContactoNroDoc) ? "" : entity.ContactoNroDoc;

                    viewModel.Tab2.MotivoConsulta = string.IsNullOrEmpty(entity.MotivoConsulta) ? "" : entity.MotivoConsulta;
                    viewModel.Tab2.SedeID = entity.SedeId.ToString();

                    viewModel.Tab3.Especialidades = await getEspecialidades();
                    viewModel.Tab3.EvaluacionesPago = await getListEvaluacionesByPagoAsync(Convert.ToInt32(pagoId), string.IsNullOrEmpty(entity.RefUsuarioId) ? "" : entity.RefUsuarioId);
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("InicioDeReserva", "ReservarCita");
                }

            }

            if (TempData["hc"] == null)
            {
                return RedirectToAction("InicioDeReserva", "ReservarCita");
            }
            try
            {
                var _hc = Convert.ToInt32(TempData["hc"]);
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(appUsuario.Id.ToString());
                Paciente? paciente = await _pacienteService.GetPaciente(Convert.ToInt32(_hc));
                FormularioDeReservaViewModel viewModel = new FormularioDeReservaViewModel();
                viewModel.Tab1.Hc = _hc;
                if (paciente != null)
                {
                    ViewBag.datos = true;

                    viewModel.Tab1.NombresPaciente = paciente.Nombres;
                    viewModel.Tab1.ApellidoPaternoPaciente = paciente.ApellidoPaterno;
                    viewModel.Tab1.ApellidoMaternoPaciente = paciente.ApellidoMaterno;
                    viewModel.Tab1.TipoDocumento = paciente.PacienteDni.Length == 8 ? "DNI" : "";
                    viewModel.Tab1.PacienteDNI = paciente.PacienteDni;
                    viewModel.Tab1.FechaNacimientoPaciente = paciente.FechaNacimiento.ToString("dd/MM/yyyy");
                    viewModel.Tab1.Colegio = paciente.CentroEducativo;
                }
                if (userRole != null)
                {
                    viewModel.Tab1.UsuarioId = appUsuario.Id;
                    viewModel.Tab1.ApoderadoNombre = string.IsNullOrEmpty(userRole.Nombres)? "" : userRole.Nombres.Trim();
                    viewModel.Tab1.ContactoTelCelular = string.IsNullOrEmpty(userRole.Telefono) ? "" : userRole.Telefono;
                    viewModel.Tab1.ContactoCorreo = string.IsNullOrEmpty(userRole.Email) ? "" : userRole.Email;
                    viewModel.Tab1.ContactoTipoDoc = string.IsNullOrEmpty(userRole.TipoDoc) ? "" : userRole.TipoDoc;
                    viewModel.Tab1.ContactoNroDoc = string.IsNullOrEmpty(userRole.Dni) ? "" : userRole.Dni;
                }
                viewModel.Tab3.Especialidades = await getEspecialidades();


                return View(viewModel);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "ObtenTuCitaViewModel", false, ex.Message);
                return RedirectToAction("InicioDeReserva", "ReservarCita");
            }

        }

        [HttpPost]
        public async Task<IActionResult> FormularioDeReserva(FormularioDeReservaViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;

            if (ModelState.IsValid)
            {
                try
                {

                    var listTotal = new List<EvaluacionCitum>();
                    // Comprueba si las Especialidades tienen valores
                    if (model.Tab3.EvaluacionSeleccionada != null)
                    {
                        listTotal = await _evaluacionCitaService.getListEvaluacionCitaByEvaluacionPublicaId(Convert.ToInt32(model.Tab3.EvaluacionSeleccionada));
                    }
                    TriageOnline mTriajeOnLine;
                    if (model.Tab1.TriageOnlineID != 0)
                    {
                        mTriajeOnLine = await _triajeOnlineService.GetTriageOnlineById(model.Tab1.TriageOnlineID);
                        mTriajeOnLine.ActualizadoPor = User.Identity.Name;
                        mTriajeOnLine.UltimaActualizacion = DateTime.Now;
                    }
                    else
                    {
                        mTriajeOnLine = new TriageOnline();
                        mTriajeOnLine.CreatedBy = User.Identity.Name;
                        mTriajeOnLine.CreatedDate = DateTime.Now;
                        mTriajeOnLine.FechaRegistro = DateTime.Now;
                    }
                    
                    mTriajeOnLine.RefUsuarioId = model.Tab1.UsuarioId;
                    mTriajeOnLine.RefNroHistoriaClinica = model.Tab1.Hc == 0 ? null : model.Tab1.Hc;
                    mTriajeOnLine.NombresPaciente = model.Tab1.NombresPaciente.ToString().ToUpper();
                    mTriajeOnLine.ApellidoPaternoPaciente = model.Tab1.ApellidoPaternoPaciente.ToString().ToUpper();
                    mTriajeOnLine.ApellidoMaternoPaciente = model.Tab1.ApellidoMaternoPaciente.ToString().ToUpper();
                    mTriajeOnLine.PacienteGrado = model.Tab1.PacienteGrado != null ? model.Tab1.NivelEducacion + " - " + model.Tab1.PacienteGrado.ToString() : model.Tab1.NivelEducacion;
                    mTriajeOnLine.Procedencia = model.Tab1.Procedencia.ToString();
                    mTriajeOnLine.FechaNacimientoPaciente = Convert.ToDateTime(model.Tab1.FechaNacimientoPaciente);
                    mTriajeOnLine.PacienteDni = model.Tab1.PacienteDNI.ToString();
                    mTriajeOnLine.PacienteCentroEducativo = model.Tab1.Colegio != null ? model.Tab1.Colegio.ToString().ToUpper() : "";
                    mTriajeOnLine.TipoDocumento = model.Tab1.TipoDocumento.ToString();
                    mTriajeOnLine.Tipo = model.Tab1.Dependiente == "SI" ? 0 : 1;
                    mTriajeOnLine.ParentescoPaciente = string.IsNullOrEmpty(model.Tab1.Parentesco) ? "" : model.Tab1.Parentesco.ToString();
                    mTriajeOnLine.PersonalId = 1269;

                    if (model.Tab1.Dependiente == "SI")
                    {

                        mTriajeOnLine.ApoderadoRelacion = string.IsNullOrEmpty(model.Tab1.Parentesco) ? "" : model.Tab1.Parentesco.ToUpper();
                        mTriajeOnLine.ApoderadoNombre = model.Tab1.ApoderadoNombre?.ToUpper();
                        mTriajeOnLine.ApoderadoTelCelular = model.Tab1.ContactoTelCelular;
                        mTriajeOnLine.ApoderadoEmail = model.Tab1.ContactoCorreo;
                        mTriajeOnLine.ContactoNroDoc = model.Tab1.ContactoNroDoc;
                        mTriajeOnLine.ContactoTipoDoc = model.Tab1.ContactoTipoDoc;

                        if (mTriajeOnLine.ParentescoPaciente == "MAMÁ")
                        {
                            mTriajeOnLine.NombreMadre = model.Tab1.ApoderadoNombre;
                            if (string.IsNullOrEmpty(mTriajeOnLine.NombreMadre)) { mTriajeOnLine.NombreMadre = mTriajeOnLine.NombreMadre.ToUpper(); }
                            mTriajeOnLine.CelularMadre = model.Tab1.ContactoTelCelular;
                            mTriajeOnLine.EmaiMadre = model.Tab1.ContactoCorreo;
                        }
                        if (mTriajeOnLine.ParentescoPaciente == "PAPÁ")
                        {
                            mTriajeOnLine.NombrePadre = model.Tab1.ApoderadoNombre;
                            mTriajeOnLine.CelularPadre = model.Tab1.ContactoTelCelular;
                            mTriajeOnLine.EmailPadre = model.Tab1.ContactoCorreo;
                        }
                    }
                    else
                    {
                        mTriajeOnLine.TelefonoPaciente = model.Tab1.ContactoTelCelular;
                        mTriajeOnLine.ApoderadoTelCelular = model.Tab1.ContactoTelCelular;
                    }

                    mTriajeOnLine.EmailRegistro = model.Tab1.ContactoCorreo.ToUpper();
                    mTriajeOnLine.MotivoConsulta = model.Tab2.MotivoConsulta;
                    mTriajeOnLine.Dependiente = model.Tab1.Dependiente == "SI";
                    mTriajeOnLine.SedeId = Convert.ToInt32(model.Tab2.SedeID);
                    if (listTotal.Any())
                    {
                        // Tiene evaluaciones → siempre Completado
                        mTriajeOnLine.TriageOnlineEstadoId = 2;
                    }
                    else
                    {
                        // No tiene evaluaciones
                        if (mTriajeOnLine.TriageOnlineEstadoId != 2)
                        {
                            // Si no estaba completado → pasar a Pendiente
                            mTriajeOnLine.TriageOnlineEstadoId = 1;
                        }
                        // Si ya estaba en Completado → mantener
                    }
                    mTriajeOnLine.EliminadoFecha = null;
                    mTriajeOnLine.GuidCheck = true;

                    mTriajeOnLine.TriageTipo = 4; //Traige Online
                    mTriajeOnLine.Area = 1;

                    if (!string.IsNullOrEmpty(model.Tab3.OrientacionSeleccionada))
                    {
                        string textOrientacion = "";
                        if (model.Tab3.OrientacionSeleccionada == "1")
                        {
                            textOrientacion = "Orientación por WhatsApp al número: +51 " + model.Tab1.ContactoTelCelular; 
                        }
                        else if (model.Tab3.OrientacionSeleccionada == "2")
                        {
                            textOrientacion = "Orientación por llamada telefónica al número: " + model.Tab1.ContactoTelCelular; 
                        }
                        else
                        {
                            textOrientacion = "Orientación por correo electrónico al mail: " + model.Tab1.ContactoCorreo;
                        }

                        mTriajeOnLine.DetalleOrientacion = textOrientacion;
                    }
                   

                    string numRef = await _triajeOnlineService.SearchTriaje(model.Tab1.TipoDocumento, model.Tab1.PacienteDNI);
                    if (!string.IsNullOrEmpty(numRef)) { mTriajeOnLine.TriageNoRef = Convert.ToInt32(numRef); }

                    //pre llenado del email
                    mTriajeOnLine.EmailEvaluaciones = await _configSistemaService.getVariable("TRIAGE_EMAIL_EVALUACIONES");
                    mTriajeOnLine.EmailProcedimiento1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE1");
                    mTriajeOnLine.EmailProcedimiento2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE2");
                    mTriajeOnLine.EmailProcedimiento3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE3");
                    mTriajeOnLine.EmailBanco1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO1");
                    mTriajeOnLine.EmailBanco2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO2");
                    mTriajeOnLine.EmailBanco3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO3");
                    mTriajeOnLine.EmailBanco4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO4");
                    mTriajeOnLine.EmailBancoCuenta1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA1");
                    mTriajeOnLine.EmailBancoCuenta2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA2");
                    mTriajeOnLine.EmailBancoCuenta3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA3");
                    mTriajeOnLine.EmailBancoCuenta4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA4");
                    mTriajeOnLine.EmailBancoCuentaCci1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI1");
                    mTriajeOnLine.EmailBancoCuentaCci2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI2");
                    mTriajeOnLine.EmailBancoCuentaCci3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI3");
                    mTriajeOnLine.EmailBancoCuentaCci4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI4");
                    //espacios al final
                    mTriajeOnLine.ApellidoPaternoPaciente = mTriajeOnLine.ApellidoPaternoPaciente.TrimEnd();
                    mTriajeOnLine.ApellidoMaternoPaciente = mTriajeOnLine.ApellidoMaternoPaciente.TrimEnd();
                    mTriajeOnLine.NombresPaciente = mTriajeOnLine.NombresPaciente.TrimEnd();
                    mTriajeOnLine.PacienteDni = mTriajeOnLine.PacienteDni.TrimEnd();

                    // Crear triajeCotejoExterno
                    TriageCotejoExt modelCotejoExt = new TriageCotejoExt();
                    modelCotejoExt.LenguajeObs = "";
                    modelCotejoExt.HablaObs = "";
                    modelCotejoExt.AprendiObs = "";
                    modelCotejoExt.CuerpoObs = "";
                    modelCotejoExt.UdadObs = "";
                    modelCotejoExt.AudioObs = "";
                    modelCotejoExt.PsicoObs = "";
                    modelCotejoExt.CreatedBy = "web";
                    modelCotejoExt.CreatedDate = DateTime.Now;
                    modelCotejoExt.ActualizadoPor = "public";
                    modelCotejoExt.ActualizadoFecha = DateTime.Now;

                    // Crear triajeCotejo
                    TriageCotejo modelCotejo = new TriageCotejo();
                    modelCotejo.CreatedBy = "Public";
                    modelCotejo.CreatedDate = DateTime.Now;
                    modelCotejo.CotejoEstado = "PEN"; // PEN, ENV=enviado, RES=respuesta

                    var idTriajeOnline =  await _triajeOnlineService.Crear(mTriajeOnLine, modelCotejoExt, modelCotejo);
                    var idPagoCita = 0;
                    var textMessage = "En breve una especialista se comunicará con usted.";

                    var triajeNo = 0;

                    // si existe evaluaciones seleccionadas => crear pago
                    if (listTotal.Count() > 0)
                    {
                        PagoCita ePagoCita;
                        if (model.Tab1.PagoCitaID == 0)
                        {
                            ePagoCita = new PagoCita();
                        }
                        else
                        {
                            ePagoCita = await _pagoCitaService.getPagoCitaById(model.Tab1.PagoCitaID);
                        }
                        
                        ePagoCita.CreadoFecha = DateTime.Now;
                        ePagoCita.EstadoPago = "PENDIENTE";
                        ePagoCita.UsuarioId = model.Tab1.UsuarioId;
                        ePagoCita.HistoriaClinica = model.Tab1.Hc;
                        ePagoCita.TriajeOnlineId = idTriajeOnline;
                        ePagoCita.CreadoPor = User.Identity?.Name;

                        await _pagoCitaService.GuardarPagoCita(ePagoCita);

                        UvEvaluacionesPublica ePublica = await _evaluacionCitaService.GetEvaluacionPublicaById(Convert.ToInt32(model.Tab3.EvaluacionSeleccionada));
                        var evaluaciones = new Dictionary<int, Action>
                        {
                            { 1, () => mTriajeOnLine.EvaluarAudiologia = true },
                            { 2, () => mTriajeOnLine.EvaluarPsicologia = true },
                            { 3, () => mTriajeOnLine.EvaluarHabla = true },
                            { 4, () => mTriajeOnLine.EvaluarLenguajeAprendizaje = true },
                            { 5, () => mTriajeOnLine.EvaluarNeurologia = true },
                            { 6, () => mTriajeOnLine.EvaluarPsicomotriz = true },
                            { 7, () => mTriajeOnLine.EvaluarPsicologia = true },
                            { 8, () => mTriajeOnLine.EvaluaAprendizaje = true },
                            { 9, () => mTriajeOnLine.Evaluaudad = true },
                        };

                        foreach (var item in listTotal)
                        {
                            int nroCita = await _pagoCitaService.getNextNumeroCita(ePagoCita.PagoCitaId, Convert.ToInt32(item.EvaluacionId));
                            // Crear los detalles y asignar el número consecutivo
                            var eDetalle = new PagoCitaDetalle
                            {
                                PagoCitaId = ePagoCita.PagoCitaId, // Relacionar con el PagoCita creado
                                EvaluacionAreaId = item.EvaluacionAreaId,
                                EvaluacionId = item.EvaluacionId,
                                EvaluacionCitaId = item.EvaluacionCitaId,
                                TipoCitaId = item.TipoCitaId,
                                SedeId = mTriajeOnLine.SedeId,
                                NumeroCita = nroCita,
                                EvaluacionNombre = ePublica.EvaluacionPublica,
                                Precio = item.Precio
                            };

                            if (eDetalle.EvaluacionAreaId.HasValue && evaluaciones.TryGetValue(eDetalle.EvaluacionAreaId.Value, out var accion))
                            {
                                accion();
                            }

                            await _pagoCitaService.GuardarPagoCitaDetalle(eDetalle);
                        }

                        //Crear Triaje
                        triajeNo = await CopyTriageOnlineToTriage(idTriajeOnline);
                        mTriajeOnLine.TriageGenerado = triajeNo;
                        await _triajeOnlineService.GuardarTriageOnline(mTriajeOnLine);
                        await SaveLog(1, "Reserva", true, "Dx Completada Nro: " + mTriajeOnLine.TriageOnlineId.ToString());


                        int pagoCitaId = ePagoCita.PagoCitaId;
                        model.Tab1.PagoCitaID = ePagoCita.PagoCitaId;
                        textMessage = "Los datos se guardaron correctamente. ¿Desea agregar otra evaluación?.";
                        return RedirectToAction(nameof(FormularioDeReserva), (object)new
                        {
                            triajeId = mTriajeOnLine.TriageOnlineId,
                            pagoId = pagoCitaId,
                            doAction = "Agregar"
                        });
                    }
                    mTriajeOnLine.EsOrientado = true;

                    await _triajeOnlineService.GuardarTriageOnline(mTriajeOnLine);
                    await SaveLog(1, "Reserva", true, "Dx Solicita Asesoria Nro: " + mTriajeOnLine.TriageOnlineId.ToString());


                    ViewBag.Exito = textMessage;
                    ViewBag.Pago = model.Tab1.PagoCitaID.ToString();
                    ViewBag.doAction = "Guardar";
                    ViewBag.triajeOL = mTriajeOnLine.TriageOnlineId.ToString();
                    return View(model);

                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    await SaveLog(3, "TriageOnline", false, ex.Message);
                    model.Tab3.Especialidades = await getEspecialidades();
                    return View(model);
                }
            }
            ViewBag.Error = "Error al enviar información";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvaluaciones(int id)
        {
            try
            {
                List<UvEvaluacionesPublica> model = await _evaluacionCitaService.getListEvaluacionesPublicasByEsp(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvEvaluacionesPublica", false, ex.Message);
                // Registra el error si tienes un sistema de logs
                return StatusCode(500, new { message = "Ocurrió un error al obtener las evaluaciones.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSesiones(int id,int idPublic)
        {
            try
            {
                List<UvCitasPublica> model = await _evaluacionCitaService.getListSesionesByEval(id, idPublic);
                return Json(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvCitasPublica", false, ex.Message);
                // Registra el error si tienes un sistema de logs
                return StatusCode(500, new { message = "Ocurrió un error al obtener las sesiones.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarEvaluacionPagoCita(int pagoCitaId, int evaluacionId)
        {
            try
            {
                await _pagoCitaService.DeletePagoCitaDetalleByEvaluacion(pagoCitaId, evaluacionId);
                await SaveLog(1, "Reserva", true, "Dx Evaluación eliminada id: " + evaluacionId.ToString());
                return Ok(new { message = "Evaluación eliminada correctamente." });
            }
            catch (Exception ex)
            {
                await SaveLog(3, "", false, ex.Message);
                return BadRequest(new { error = "Ocurrió un error al eliminar la evaluación.", message = ex.Message });
            }
        }
        public async Task<List<EvaluacionPagoViewModel>> getListEvaluacionesByPagoAsync(int pagoCitaId, string usuarioId)
        {
            //var evaluacionesByPagosList =  await _pagosTriajeService.GetPagosCitasList(pagoCitaId, usuarioId);
            //var listaDepurada = evaluacionesByPagosList
            //.GroupBy(x => new
            //{
            //    x.PagoCitaId,
            //    x.CreadoFecha,
            //    x.EstadoPago,
            //    x.EvaluacionAreaId,
            //    x.EvaluacionId,
            //    x.Nombre,
            //    x.EspecialistaId,
            //    x.SedeId,
            //    x.Sede,
            //    x.ImagenUrl
            //})
            //.Select(g => g.First())
            //.Select(g => new EvaluacionPagoViewModel
            //{
            //    PagoCitaId = g.PagoCitaId,
            //    CreadoFecha = g.CreadoFecha,
            //    EstadoPago = g.EstadoPago,
            //    EvaluacionAreaId = g.EvaluacionAreaId,
            //    EvaluacionId = g.EvaluacionId,
            //    Nombre = g.Nombre,
            //    EspecialistaId = g.EspecialistaId,
            //    SedeId = g.SedeId,
            //    Sede = g.Sede,
            //    ImagenUrl = g.ImagenUrl
            //})
            //.ToList();
            var evaluacionesByPagosList = await _pagosTriajeService.GetPagosCitasList(pagoCitaId, usuarioId);
            var listaDepurada = evaluacionesByPagosList
                .GroupBy(x => new
                {
                    x.PagoCitaId,
                    x.CreadoFecha,
                    x.EstadoPago,
                    x.EvaluacionAreaId,
                    x.EvaluacionId,
                    x.Nombre,
                    x.EspecialistaId,
                    x.SedeId,
                    x.Sede,
                    x.ImagenUrl
                })
                .Select(g => new EvaluacionPagoViewModel
                {
                    PagoCitaId = g.Key.PagoCitaId,
                    CreadoFecha = g.Key.CreadoFecha,
                    EstadoPago = g.Key.EstadoPago,
                    EvaluacionAreaId = g.Key.EvaluacionAreaId,
                    EvaluacionId = g.Key.EvaluacionId,
                    Nombre = g.Key.Nombre,
                    EspecialistaId = g.Key.EspecialistaId,
                    SedeId = g.Key.SedeId,
                    Sede = g.Key.Sede,
                    ImagenUrl = g.Key.ImagenUrl,
                    TotalSesiones = g.Count() // Contar el total de elementos en el grupo
                })
                .ToList();

            return listaDepurada;
        }
        public async Task<List<UvEspecilidadesPublica>> getEspecialidades()
        {
            var listaEspEvaluacionesCitas = await _evaluacionCitaService.getListEspecialidadesPublicas();
            return listaEspEvaluacionesCitas
                .OrderBy(e => e.EspecialidadNroOrden).ToList();
        }
        public async Task<int> CopyTriageOnlineToTriage(int id)
        {
            int mReturn = 0;
            try
            {
                //BL.BLTriage mBLTriage = new BLTriage();
                TriageOnline entityTriageOnline = await _triajeOnlineService.GetTriageOnlineById(id);
                Triage entityTriage;
                if (entityTriageOnline != null)
                {
                    //copiar info de TriageOnline  a Triage
                    if (entityTriageOnline.TriageNoRef.HasValue)
                    {
                        entityTriage = await _triajeOnlineService.GetTriageById(entityTriageOnline.TriageNoRef.Value);
                        if (entityTriage == null) entityTriage = new Triage();
                    }
                    else
                    {
                        entityTriage = new Triage();
                    }
                    entityTriage.Fecha = DateTime.Today;
                    //entityTriage.PersonalId = entityTriageOnline.PersonalId;
                    entityTriage.ApellidoPaternoPaciente = entityTriageOnline.ApellidoPaternoPaciente;
                    entityTriage.ApellidoMaternoPaciente = entityTriageOnline.ApellidoMaternoPaciente;
                    entityTriage.NombresPaciente = entityTriageOnline.NombresPaciente;
                    entityTriage.FechaNacimientoPaciente = entityTriageOnline.FechaNacimientoPaciente!.Value;
                    entityTriage.SexoPaciente = entityTriage.SexoPaciente == 0 ? Convert.ToByte(0) : entityTriage.SexoPaciente;
                    //entityTriage.TelefonoPaciente = entityTriageOnline.telefonoPaciente;
                    //entityTriage.FechaEvaluacion = entityTriageOnline.fechaEvaluacion;
                    entityTriage.EvaluarNeurologia = entityTriageOnline.EvaluarNeurologia;
                    entityTriage.EvaluarPsicologia = entityTriageOnline.EvaluarPsicologia;
                    entityTriage.EvaluarLenguajeAprendizaje = entityTriageOnline.EvaluarLenguajeAprendizaje;
                    entityTriage.EvaluarPsicomotriz = entityTriageOnline.EvaluarPsicomotriz;
                    entityTriage.EvaluarAudiologia = entityTriageOnline.EvaluarAudiologia;
                    entityTriage.EvaluarOrientacionVocacional = entityTriageOnline.EvaluarOrientacionVocacional;
                    entityTriage.EvaluarHabla = entityTriageOnline.EvaluarHabla;
                    entityTriage.EvaluaAprendizaje = entityTriageOnline.EvaluaAprendizaje;
                    entityTriage.EvaluaMotividad = entityTriageOnline.EvaluaMotividad;
                    entityTriage.EvaluaDisfluencia = entityTriageOnline.EvaluaDisfluencia;
                    entityTriage.EvaluaVoz = entityTriageOnline.EvaluaVoz;
                    entityTriage.Evaluaudad = entityTriageOnline.Evaluaudad;
                    entityTriage.EvaluaTrastComunicacion = entityTriageOnline.EvaluaTrastComunicacion;


                    entityTriage.Dependiente = entityTriageOnline.Dependiente;
                    entityTriage.Observaciones = entityTriageOnline.Observaciones;
                    entityTriage.Colegio = entityTriageOnline.Colegio;
                    entityTriage.MotivoConsulta = entityTriageOnline.MotivoConsulta;
                    entityTriage.TriageTipo = entityTriageOnline.TriageTipo;
                    //entityTriage.Comollegocpal = entityTriageOnline.comollegocpal;
                    entityTriage.Descomollego = entityTriageOnline.Descomollego;
                    //entityTriage.IdColegio = entityTriageOnline.id_colegio;
                    //entityTriage.CodigoColegio = entityTriageOnline.codigoColegio;
                    entityTriage.PacienteGrado = entityTriageOnline.PacienteGrado;
                    //entityTriage.PacienteTel = entityTriageOnline.pacienteTel;
                    //entityTriage.PacienteProfNombre = entityTriageOnline.pacienteProfNombre;
                    //entityTriage.PacienteProfTel = entityTriageOnline.pacienteProfTel;
                    //entityTriage.PacienteProfEmail = entityTriageOnline.pacienteProfEmail;
                    //entityTriage.PacienteDerivadoPor = entityTriageOnline.pacienteDerivadoPor;
                    //entityTriage.PacienteTratAnteriores = entityTriageOnline.pacienteTratAnteriores;
                    //entityTriage.PacienteObservaciones = entityTriageOnline.pacienteObservaciones;
                    //entityTriage.PacienteDireccion = entityTriageOnline.pacienteDireccion;
                    //entityTriage.PacienteCodigoPais = entityTriageOnline.pacienteCodigoPais;
                    //entityTriage.PacienteCodigoDepartamento = entityTriageOnline.pacienteCodigoDepartamento;
                    //entityTriage.PacienteCodigoProvincia = entityTriageOnline.pacienteCodigoProvincia;
                    //entityTriage.PacienteCodigoDistrito = entityTriageOnline.pacienteCodigoDistrito;
                    //entityTriage.PacienteDomicilioTel = entityTriageOnline.pacienteDomicilioTel;
                    //entityTriage.PacienteDomicilioTelEmer = entityTriageOnline.pacienteDomicilioTelEmer;
                    //entityTriage.PacienteDireccion2 = entityTriageOnline.pacienteDireccion2;
                    entityTriage.PersonalId = entityTriageOnline.PersonalId;
                    entityTriage.NombrePadre = entityTriageOnline.NombrePadre;
                    //entityTriage.OcupacionPadre = entityTriageOnline.ocupacionPadre;
                    //entityTriage.CentroTrabajoPadre = entityTriageOnline.centroTrabajoPadre;
                    entityTriage.TelefonoPadre = entityTriageOnline.TelefonoPadre;
                    entityTriage.CelularPadre = entityTriageOnline.CelularPadre;
                    entityTriage.EmailPadre = entityTriageOnline.EmailPadre;

                    entityTriage.NombreMadre = entityTriageOnline.NombreMadre;
                    //entityTriage.OcupacionMadre = entityTriageOnline.ocupacionMadre;
                    //entityTriage.CentroTrabajoMadre = entityTriageOnline.centroTrabajoMadre;
                    entityTriage.TelefonoMadre = entityTriageOnline.TelefonoMadre;
                    entityTriage.CelularMadre = entityTriageOnline.CelularMadre;
                    entityTriage.EmaiMadre = entityTriageOnline.EmaiMadre;

                    //entityTriage.NumeroHijosVarones = entityTriageOnline.numeroHijosVarones;
                    //entityTriage.NumeroHijosMujeres = entityTriageOnline.numeroHijosMujeres;
                    //entityTriage.LugarEntreHermanos = entityTriageOnline.lugarEntreHermanos;
                    //entityTriage.PacienteViveCon = entityTriageOnline.pacienteViveCon;
                    entityTriage.PacienteEmail = entityTriageOnline.PacienteEmail;
                    entityTriage.PacienteCentroEducativo = entityTriageOnline.PacienteCentroEducativo;
                    entityTriage.PacienteDni = entityTriageOnline.PacienteDni;
                    entityTriage.TipoDocumento = entityTriageOnline.TipoDocumento;
                    entityTriage.ContactoTipoDoc = entityTriageOnline.ContactoTipoDoc;
                    entityTriage.ContactoNroDoc = entityTriageOnline.ContactoNroDoc;
                    entityTriage.ApoderadoNombre = entityTriageOnline.ApoderadoNombre;
                    entityTriage.ApoderadoRelacion = entityTriageOnline.ApoderadoRelacion;
                    entityTriage.ApoderadoTelCelular = entityTriageOnline.ApoderadoTelCelular;

                    //entityTriage.Presuncion = entityTriageOnline.presuncion;
                    entityTriage.Responsable = entityTriageOnline.Responsable;
                    entityTriage.Tipo = entityTriageOnline.Tipo;
                    //entityTriage.Coordinarcoleg = entityTriageOnline.coordinarcoleg;
                    entityTriage.EmailDeContacto = entityTriageOnline.EmailRegistro;

                    entityTriage.CreadoPor = User.Identity.Name;
                    entityTriage.CreadoFecha = DateTime.Now;

                    entityTriage.ActualizadoPor = User.Identity.Name;
                    entityTriage.UltimaActualizacion = DateTime.Now;
                    entityTriage.SedeId = entityTriageOnline.SedeId;

                    await _triajeOnlineService.GuardarTriage(entityTriage);

                    mReturn = entityTriage.TriageNo;
                    
                   
                }

                return mReturn;
            }
            catch (Exception ex)
            {
                await SaveLog(3, "Triage", false, ex.Message);
                return mReturn;
            }

        }

        public async Task<(int reservaPendienteId, int? pagoId)> GetReservaPendiente2(string dni, string estado)
        {
            int reservaPendienteId = 0;
            int? pagoId = null;
            try
            {  
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                List<UvReservaDiagnosticoSearch> list = await _triajeOnlineService.GetReservaDiagnosticoPendienteByDniAsync(appUsuario.Id, dni, estado);
                if (list.Count() > 0)
                {
                    reservaPendienteId = Convert.ToInt32(list[0].TriajeOnlineId);
                    pagoId = Convert.ToInt32(list[0].PagoCitaId);
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvPacienteTriajeSearch", false, ex.Message);
                // Registra el error si tienes un sistema de logs
            }
            return (reservaPendienteId, pagoId);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservaPendiente(string dni)
        {
            try
            {
                int reservaPendienteId = 0;
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                List<UvReservaDiagnosticoSearch> list = await _triajeOnlineService.GetReservaDiagnosticoPendienteByDniAsync(appUsuario.Id, dni,"PENDIENTE");
                if (list.Count() > 0)
                {
                    reservaPendienteId = Convert.ToInt32(list[0].TriajeOnlineId);
                }
                bool tienePendiente = reservaPendienteId > 0 ? true : false;
                return Json(tienePendiente);

            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvPacienteTriajeSearch", false, ex.Message);
                // Registra el error si tienes un sistema de logs
                return StatusCode(500, new { message = "Ocurrió un error al obtener los informes.", error = ex.Message });
            }
        }


    }
}
