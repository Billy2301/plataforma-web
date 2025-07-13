using BL.IServices;
using BL.Services;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using System.Data;
using System.Security.Claims;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class TriajeOnlineController : BaseController
    {

        private readonly IConfigSistemaService _configSistemaService;
        private readonly ITriajeOnlineService _triajeOnlineService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPacienteService _pacienteService;
        private readonly UserManager<IdentityUser>  _userManagerService;



        public TriajeOnlineController(IPacienteService pacienteService,UserManager<IdentityUser>  userManagerService,IUsuarioService usuarioService,IConfigSistemaService configSistemaService, ITriajeOnlineService triajeOnlineService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService)
        : base(accesor, configuration, logService)
        {
            _configSistemaService = configSistemaService;
            _triajeOnlineService = triajeOnlineService;
            _usuarioService = usuarioService;
            _userManagerService = userManagerService;
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<IActionResult> NuevoTriaje()
        {
          
            NuevoTriajeViewModel model = new NuevoTriajeViewModel();
            List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);

            if (Pacientes == null || Pacientes.Count == 0)
            {
                TempData["hc"] = 0;
                return RedirectToAction("Triaje", "TriajeOnline");

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
        public async Task<IActionResult> NuevoTriaje(NuevoTriajeViewModel model)
        {

            if (ModelState.IsValid)
            {
                TempData["hc"] = model.HistoriaClinica;
                return RedirectToAction("Triaje", "TriajeOnline");

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
        public async Task<IActionResult> Triaje()
        {
            ViewBag.datos = null;

            if (TempData["hc"] == null)
            {
                return RedirectToAction("NuevoTriaje", "TriajeOnline");
            }
            try
            {
                var _hc = Convert.ToInt32(TempData["hc"]);
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(appUsuario.Id.ToString());
                Paciente? paciente = await _pacienteService.GetPaciente(Convert.ToInt32(_hc));
                TriajeOnlineViewModel viewModel = new TriajeOnlineViewModel();
                viewModel.Tab1.Hc = _hc;
                if (paciente != null)
                {
                    ViewBag.datos = true;

                    viewModel.Tab1.SexoPaciente = paciente.Sexo;
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
                    viewModel.Tab2.ApoderadoTelCelular = userRole.Telefono;
                    viewModel.Tab2.ApoderadoApellidoPaterno = userRole.ApellidoPaterno;
                    viewModel.Tab2.ApoderadoApellidoMaterno = userRole.ApellidoMaterno;
                    viewModel.Tab2.ApoderadoNombre = userRole.Nombres;
                    viewModel.Tab2.EmailRegistro = userRole.Email;
                    viewModel.Tab2.ContactoTipoDoc = userRole.TipoDoc;
                    viewModel.Tab2.ContactoNroDoc = userRole.Dni;
                    viewModel.Tab2.ContactoSexo = userRole.Sexo;
                    viewModel.Tab2.ContactoFechaNac = userRole.FechaNacimiento != null ? userRole.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "";

                    if (string.IsNullOrEmpty(userRole.ApellidoPaterno) ||
                    string.IsNullOrEmpty(userRole.ApellidoMaterno) ||
                    string.IsNullOrEmpty(userRole.Nombres) ||
                    string.IsNullOrEmpty(userRole.Telefono) ||
                    !userRole.FechaNacimiento.HasValue ||
                    string.IsNullOrEmpty(userRole.Dni) ||
                    !userRole.Sexo.HasValue)
                    {
                        viewModel.Tab2.GuardarEnPerfil = true;
                    }
                }
                
                await SaveLog(1, "TriajeOnlineViewModel", true,"");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "TriajeOnlineViewModel", false, ex.Message);
                return RedirectToAction("NuevoTriaje", "TriajeOnline");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Triaje(TriajeOnlineViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;

            if (ModelState.IsValid)
            {
                try
                {

                    TriageOnline modelTriaje = new TriageOnline();
                    modelTriaje.CreatedBy = "public";
                    modelTriaje.CreatedDate = DateTime.Now;
                    modelTriaje.RefUsuarioId = model.Tab1.UsuarioId;
                    modelTriaje.RefNroHistoriaClinica = model.Tab1.Hc == 0? null : model.Tab1.Hc;
                    modelTriaje.NombresPaciente = model.Tab1.NombresPaciente.ToString().ToUpper();
                    modelTriaje.ApellidoPaternoPaciente = model.Tab1.ApellidoPaternoPaciente.ToString().ToUpper();
                    modelTriaje.ApellidoMaternoPaciente =  model.Tab1.ApellidoMaternoPaciente.ToString().ToUpper();
                    modelTriaje.PacienteGrado = model.Tab1.PacienteGrado != null ? model.Tab1.NivelEducacion + " - " + model.Tab1.PacienteGrado.ToString(): model.Tab1.NivelEducacion;
                    modelTriaje.SexoPaciente = model.Tab1.SexoPaciente;
                    modelTriaje.Procedencia = model.Tab1.Procedencia.ToString();
                    modelTriaje.FechaNacimientoPaciente = Convert.ToDateTime(model.Tab1.FechaNacimientoPaciente);
                    modelTriaje.PacienteDni = model.Tab1.PacienteDNI.ToString();
                    modelTriaje.PacienteCentroEducativo = model.Tab1.Colegio != null ? model.Tab1.Colegio.ToString().ToUpper() : "";
                    modelTriaje.TipoDocumento = model.Tab1.TipoDocumento.ToString();
                    modelTriaje.TelefonoPaciente = model.Tab2.ApoderadoTelCelular.ToString();
                    modelTriaje.Tipo = model.Tab2.Dependiente == "SI" ? 0 : 1;
                    modelTriaje.ParentescoPaciente = string.IsNullOrEmpty(model.Tab2.ApoderadoRelacion) ? "" : model.Tab2.ApoderadoRelacion.ToString();

                    if (modelTriaje.ParentescoPaciente == "MAMÁ")
                    {
                        modelTriaje.NombreMadre = model.Tab2.ApoderadoNombre + " " + model.Tab2.ApoderadoApellidoPaterno + " " + model.Tab2.ApoderadoApellidoMaterno;
                        if (string.IsNullOrEmpty(modelTriaje.NombreMadre)) { modelTriaje.NombreMadre = modelTriaje.NombreMadre.ToUpper(); }
                        modelTriaje.CelularMadre = model.Tab2.ApoderadoTelCelular;
                        modelTriaje.EmaiMadre = model.Tab2.EmailRegistro;
                    }
                    if (modelTriaje.ParentescoPaciente == "PAPÁ")
                    {
                        modelTriaje.NombrePadre = model.Tab2.ApoderadoNombre + " " + model.Tab2.ApoderadoApellidoPaterno + " " + model.Tab2.ApoderadoApellidoMaterno;
                        if (string.IsNullOrEmpty(modelTriaje.NombrePadre)) { modelTriaje.NombrePadre = modelTriaje.NombrePadre.ToUpper(); }
                        modelTriaje.CelularPadre = model.Tab2.ApoderadoTelCelular;
                        modelTriaje.EmailPadre = model.Tab2.EmailRegistro;
                    }
                    if (model.Tab2.Dependiente == "SI")
                    {

                        modelTriaje.ApoderadoRelacion = string.IsNullOrEmpty(model.Tab2.ApoderadoRelacion) ? "" : model.Tab2.ApoderadoRelacion.ToUpper();
                        modelTriaje.ApoderadoNombre = model.Tab2.ApoderadoNombre.ToUpper() + " " + model.Tab2.ApoderadoApellidoPaterno.ToUpper() + " " + model.Tab2.ApoderadoApellidoMaterno.ToUpper();
                        modelTriaje.ApoderadoTelCelular = model.Tab2.ApoderadoTelCelular;
                        modelTriaje.ApoderadoEmail = model.Tab2.EmailRegistro;
                        modelTriaje.ContactoFechaNac = GetEnglishDate(model.Tab2.ContactoFechaNac);
                        modelTriaje.ContactoNroDoc = model.Tab2.ContactoNroDoc;
                        modelTriaje.ContactoSexo = model.Tab2.ContactoSexo;
                        modelTriaje.ContactoTipoDoc = model.Tab2.ContactoTipoDoc;
                    }

                    modelTriaje.EmailRegistro = model.Tab2.EmailRegistro.ToUpper();
                    modelTriaje.MotivoConsulta = model.Tab5.MotivoConsulta.ToUpper();
                    modelTriaje.ConsultoPor = model.Tab5.ConsultoPor.ToUpper();
                    modelTriaje.Comollegocpal = model.Tab5.ComoLlegoCPAL;
                    modelTriaje.Dependiente = model.Tab2.Dependiente == "SI";
                    modelTriaje.SedeId = model.Tab5.SedeID;
                    modelTriaje.TriageOnlineEstadoId = 1;
                    modelTriaje.EliminadoFecha = null;
                    modelTriaje.GuidCheck = true;
                    modelTriaje.FechaRegistro = DateTime.Now;
                    modelTriaje.TriageTipo = 4; //Traige Onlien

                    string numRef = await _triajeOnlineService.SearchTriaje(model.Tab1.TipoDocumento, model.Tab1.PacienteDNI);
                    if (!string.IsNullOrEmpty(numRef)) { modelTriaje.TriageNoRef = Convert.ToInt32(numRef); }

                    //pre llenado del email
                    modelTriaje.EmailEvaluaciones = await _configSistemaService.getVariable("TRIAGE_EMAIL_EVALUACIONES");
                    modelTriaje.EmailProcedimiento1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE1");
                    modelTriaje.EmailProcedimiento2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE2");
                    modelTriaje.EmailProcedimiento3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_PROCE3");
                    modelTriaje.EmailBanco1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO1");
                    modelTriaje.EmailBanco2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO2");
                    modelTriaje.EmailBanco3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO3");
                    modelTriaje.EmailBanco4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO4");
                    modelTriaje.EmailBancoCuenta1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA1");
                    modelTriaje.EmailBancoCuenta2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA2");
                    modelTriaje.EmailBancoCuenta3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA3");
                    modelTriaje.EmailBancoCuenta4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA4");
                    modelTriaje.EmailBancoCuentaCci1 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI1");
                    modelTriaje.EmailBancoCuentaCci2 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI2");
                    modelTriaje.EmailBancoCuentaCci3 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI3");
                    modelTriaje.EmailBancoCuentaCci4 = await _configSistemaService.getVariable("TRIAGE_EMAIL_BANCO_CUENTA_CCI4");
                    //espacios la final
                    modelTriaje.ApellidoPaternoPaciente = modelTriaje.ApellidoPaternoPaciente.TrimEnd();
                    modelTriaje.ApellidoMaternoPaciente = modelTriaje.ApellidoMaternoPaciente.TrimEnd();
                    modelTriaje.NombresPaciente = modelTriaje.NombresPaciente.TrimEnd();
                    modelTriaje.PacienteDni = modelTriaje.PacienteDni.TrimEnd();



                    TriageCotejoExt modelCotejoExt = new TriageCotejoExt();
                    if (model.Tab3.EvaluarLenguaje)
                    {
                        modelCotejoExt.Lenguaje01 = model.Tab4.Lenguaje01 == "SI";
                        modelCotejoExt.Lenguaje02 = model.Tab4.Lenguaje02 == "SI";
                        modelCotejoExt.Lenguaje03 = model.Tab4.Lenguaje03 == "SI";
                        modelCotejoExt.Lenguaje04 = model.Tab4.Lenguaje04 == "SI";
                        modelCotejoExt.Lenguaje05 = model.Tab4.Lenguaje05 == "SI";
                        modelCotejoExt.Lenguaje06 = model.Tab4.Lenguaje06 == "SI";
                        modelCotejoExt.Lenguaje07 = model.Tab4.Lenguaje07 == "SI";
                        modelCotejoExt.Lenguaje08 = model.Tab4.Lenguaje08 == "SI";
                        modelCotejoExt.Lenguaje09 = model.Tab4.Lenguaje09 == "SI";
                        modelCotejoExt.Lenguaje10 = model.Tab4.Lenguaje10 == "SI";
                        modelCotejoExt.Lenguaje11 = model.Tab4.Lenguaje11 == "SI";
                        modelCotejoExt.Lenguaje12 = model.Tab4.Lenguaje12 == "SI";
                        modelCotejoExt.Lenguaje13 = model.Tab4.Lenguaje13 == "SI";
                    }
                    if (model.Tab3.EvaluarHabla)
                    {
                        if (model.Tab4.SelectHabla == "MO")
                        {
                            modelCotejoExt.Habla01 = model.Tab4.Habla01 == "SI";
                            modelCotejoExt.Habla03 = model.Tab4.Habla03 == "SI";
                            modelCotejoExt.Habla04 = model.Tab4.Habla04 == "SI";
                            modelCotejoExt.Habla05 = model.Tab4.Habla05 == "SI";
                            modelCotejoExt.Habla06 = model.Tab4.Habla06 == "SI";
                            modelCotejoExt.Habla17 = model.Tab4.Habla17 == "SI";
                            modelCotejoExt.Habla18 = model.Tab4.Habla18 == "SI";
                        }
                        if (model.Tab4.SelectHabla == "FL")
                        {
                            modelCotejoExt.Habla07 = model.Tab4.Habla07 == "SI";
                            modelCotejoExt.Habla08 = model.Tab4.Habla08 == "SI";
                            modelCotejoExt.Habla09 = model.Tab4.Habla09 == "SI";
                            modelCotejoExt.Habla10 = model.Tab4.Habla10 == "SI";
                            modelCotejoExt.Habla11 = model.Tab4.Habla11 == "SI";
                        }
                        if (model.Tab4.SelectHabla == "VO")
                        {
                            modelCotejoExt.Habla12 = model.Tab4.Habla12 == "SI";
                            modelCotejoExt.Habla13 = model.Tab4.Habla13 == "SI";
                            modelCotejoExt.Habla14 = model.Tab4.Habla14 == "SI";
                            modelCotejoExt.Habla15 = model.Tab4.Habla15 == "SI";
                            modelCotejoExt.Habla16 = model.Tab4.Habla16 == "SI";
                        }
                    }
                    if (model.Tab3.EvaluaAprendizaje)
                    {
                        if (model.Tab4.SelectAprendizaje == "PRE")
                        {
                            modelCotejoExt.AprendPre01 = model.Tab4.AprendPre01 == "SI";
                            modelCotejoExt.AprendPre02 = model.Tab4.AprendPre02 == "SI";
                            modelCotejoExt.AprendPre03 = model.Tab4.AprendPre03 == "SI";
                            modelCotejoExt.AprendPre04 = model.Tab4.AprendPre04 == "SI";
                            modelCotejoExt.AprendPre05 = model.Tab4.AprendPre05 == "SI";
                            modelCotejoExt.AprendPre06 = model.Tab4.AprendPre06 == "SI";
                            modelCotejoExt.AprendPre07 = model.Tab4.AprendPre07 == "SI";
                            modelCotejoExt.AprendPre08 = model.Tab4.AprendPre08 == "SI";
                            modelCotejoExt.AprendPre09 = model.Tab4.AprendPre09 == "SI";
                            modelCotejoExt.AprendPre10 = model.Tab4.AprendPre10 == "SI";
                        }
                        if (model.Tab4.SelectAprendizaje == "ESC")
                        {
                            modelCotejoExt.AprendEsc01 = model.Tab4.AprendEsc01 == "SI";
                            modelCotejoExt.AprendEsc02 = model.Tab4.AprendEsc02 == "SI";
                            modelCotejoExt.AprendEsc03 = model.Tab4.AprendEsc03 == "SI";
                            modelCotejoExt.AprendEsc04 = model.Tab4.AprendEsc04 == "SI";
                            modelCotejoExt.AprendEsc05 = model.Tab4.AprendEsc05 == "SI";
                            modelCotejoExt.AprendEsc06 = model.Tab4.AprendEsc06 == "SI";
                            modelCotejoExt.AprendEsc07 = model.Tab4.AprendEsc07 == "SI";
                            modelCotejoExt.AprendEsc08 = model.Tab4.AprendEsc08 == "SI";
                            modelCotejoExt.AprendEsc09 = model.Tab4.AprendEsc09 == "SI";
                            modelCotejoExt.AprendEsc10 = model.Tab4.AprendEsc10 == "SI";
                            modelCotejoExt.AprendEsc11 = model.Tab4.AprendEsc11 == "SI";
                            modelCotejoExt.AprendEsc12 = model.Tab4.AprendEsc12 == "SI";
                            modelCotejoExt.AprendEsc13 = model.Tab4.AprendEsc13 == "SI";
                            modelCotejoExt.AprendEsc14 = model.Tab4.AprendEsc14 == "SI";
                            modelCotejoExt.AprendEsc15 = model.Tab4.AprendEsc15 == "SI";
                        }
                        if (model.Tab4.SelectAprendizaje == "UNI")
                        {
                            modelCotejoExt.AprendUni01 = model.Tab4.AprendUni01 == "SI";
                            modelCotejoExt.AprendUni02 = model.Tab4.AprendUni02 == "SI";
                            modelCotejoExt.AprendUni03 = model.Tab4.AprendUni03 == "SI";
                            modelCotejoExt.AprendUni04 = model.Tab4.AprendUni04 == "SI";
                            modelCotejoExt.AprendUni05 = model.Tab4.AprendUni05 == "SI";
                            modelCotejoExt.AprendUni06 = model.Tab4.AprendUni06 == "SI";
                            modelCotejoExt.AprendUni07 = model.Tab4.AprendUni07 == "SI";
                            modelCotejoExt.AprendUni08 = model.Tab4.AprendUni08 == "SI";
                        }
                        if (model.Tab4.SelectAprendizaje == "ADU")
                        {
                            modelCotejoExt.AprendAdul01 = model.Tab4.AprendAdul01 == "SI";
                            modelCotejoExt.AprendAdul02 = model.Tab4.AprendAdul02 == "SI";
                            modelCotejoExt.AprendAdul03 = model.Tab4.AprendAdul03 == "SI";
                            modelCotejoExt.AprendAdul04 = model.Tab4.AprendAdul04 == "SI";
                            modelCotejoExt.AprendAdul05 = model.Tab4.AprendAdul05 == "SI";
                            modelCotejoExt.AprendAdul06 = model.Tab4.AprendAdul06 == "SI";
                        }
                    }
                    if (model.Tab3.EvaluarCuerpoMovimiento)
                    {
                        if (model.Tab4.SelectCuerpo == "PS")
                        {
                            modelCotejoExt.Cuerpo01 = model.Tab4.Cuerpo01 == "SI";
                            modelCotejoExt.Cuerpo02 = model.Tab4.Cuerpo02 == "SI";
                            modelCotejoExt.Cuerpo03 = model.Tab4.Cuerpo03 == "SI";
                            modelCotejoExt.Cuerpo04 = model.Tab4.Cuerpo04 == "SI";
                            modelCotejoExt.Cuerpo05 = model.Tab4.Cuerpo05 == "SI";
                            modelCotejoExt.Cuerpo06 = model.Tab4.Cuerpo06 == "SI";
                            modelCotejoExt.Cuerpo13 = model.Tab4.Cuerpo13 == "SI";
                        }
                        if (model.Tab4.SelectCuerpo == "OC")
                        {
                            modelCotejoExt.Cuerpo07 = model.Tab4.Cuerpo07 == "SI";
                            modelCotejoExt.Cuerpo08 = model.Tab4.Cuerpo08 == "SI";
                            modelCotejoExt.Cuerpo09 = model.Tab4.Cuerpo09 == "SI";
                            modelCotejoExt.Cuerpo10 = model.Tab4.Cuerpo10 == "SI";
                            modelCotejoExt.Cuerpo11 = model.Tab4.Cuerpo11 == "SI";
                            modelCotejoExt.Cuerpo12 = model.Tab4.Cuerpo12 == "SI";
                            modelCotejoExt.Cuerpo14 = model.Tab4.Cuerpo14 == "SI";
                        }
                    }
                    if (model.Tab3.EvaluarUdad)
                    {
                        if (model.Tab4.SelectUdad == "PL")
                        {
                            modelCotejoExt.UdadPl01 = model.Tab4.UdadPL01 == "SI";
                            modelCotejoExt.UdadPl02 = model.Tab4.UdadPL02 == "SI";
                            modelCotejoExt.UdadPl03 = model.Tab4.UdadPL03 == "SI";
                            modelCotejoExt.UdadPl04 = model.Tab4.UdadPL04 == "SI";
                            modelCotejoExt.UdadPl05 = model.Tab4.UdadPL05 == "SI";
                            modelCotejoExt.UdadPl06 = model.Tab4.UdadPL06 == "SI";
                            modelCotejoExt.UdadPl07 = model.Tab4.UdadPL07 == "SI";
                            modelCotejoExt.UdadJuego01 = model.Tab4.UdadJuego01 == "SI";
                            modelCotejoExt.UdadJuego02 = model.Tab4.UdadJuego02 == "SI";
                            modelCotejoExt.UdadEstereo01 = model.Tab4.UdadEstereo01 == "SI";
                            modelCotejoExt.UdadEstereo02 = model.Tab4.UdadEstereo02 == "SI";
                            modelCotejoExt.UdadSensorial01 = model.Tab4.UdadSensorial01 == "SI";
                            modelCotejoExt.UdadSensorial02 = model.Tab4.UdadSensorial02 == "SI";
                            modelCotejoExt.UdadSensorial03 = model.Tab4.UdadSensorial03 == "SI";
                            modelCotejoExt.UdadSensorial04 = model.Tab4.UdadSensorial04 == "SI";
                        }
                        if (model.Tab4.SelectUdad == "LF")
                        {
                            modelCotejoExt.UdadConduc01 = model.Tab4.UdadConduc01 == "SI";
                            modelCotejoExt.UdadConduc02 = model.Tab4.UdadConduc02 == "SI";
                            modelCotejoExt.UdadConduc03 = model.Tab4.UdadConduc03 == "SI";
                            modelCotejoExt.UdadConduc04 = model.Tab4.UdadConduc04 == "SI";
                            modelCotejoExt.UdadFsenso01 = model.Tab4.UdadFSenso01 == "SI";
                            modelCotejoExt.UdadFsenso02 = model.Tab4.UdadFSenso02 == "SI";
                            modelCotejoExt.UdadLf01 = model.Tab4.UdadLF01 == "SI";
                            modelCotejoExt.UdadLf02 = model.Tab4.UdadLF02 == "SI";
                            modelCotejoExt.UdadLf03 = model.Tab4.UdadLF03 == "SI";
                            modelCotejoExt.UdadLf04 = model.Tab4.UdadLF04 == "SI";
                            modelCotejoExt.UdadLf05 = model.Tab4.UdadLF05 == "SI";
                            modelCotejoExt.UdadLf06 = model.Tab4.UdadLF06 == "SI";
                        }
                    }
                    if (model.Tab3.EvaluarAudiologia)
                    {
                        if (model.Tab4.SelectAudiologia == "BB")
                        {
                            modelCotejoExt.AudioPerdBebe01 = model.Tab4.AudioPerdBebe01 == "SI";
                            modelCotejoExt.AudioPerdBebe02 = model.Tab4.AudioPerdBebe02 == "SI";
                            modelCotejoExt.AudioPerdBebe03 = model.Tab4.AudioPerdBebe03 == "SI";
                            modelCotejoExt.AudioPerdBebe04 = model.Tab4.AudioPerdBebe04 == "SI";
                            modelCotejoExt.AudioPerdBebe05 = model.Tab4.AudioPerdBebe05 == "SI";
                            modelCotejoExt.AudioPerdBebe06 = model.Tab4.AudioPerdBebe06 == "SI";
                            modelCotejoExt.AudioPerdBebe07 = model.Tab4.AudioPerdBebe07 == "SI";
                            modelCotejoExt.AudioPerdBebe08 = model.Tab4.AudioPerdBebe08 == "SI";
                            modelCotejoExt.AudioPerdBebe09 = model.Tab4.AudioPerdBebe09 == "SI";
                            modelCotejoExt.AudioPerdBebe10 = model.Tab4.AudioPerdBebe10 == "SI";
                        }
                        if (model.Tab4.SelectAudiologia == "PA")
                        {
                            modelCotejoExt.AudioProcAud01 = model.Tab4.AudioProcAud01 == "SI";
                            modelCotejoExt.AudioProcAud02 = model.Tab4.AudioProcAud02 == "SI";
                            modelCotejoExt.AudioProcAud03 = model.Tab4.AudioProcAud03 == "SI";
                            modelCotejoExt.AudioProcAud04 = model.Tab4.AudioProcAud04 == "SI";
                            modelCotejoExt.AudioProcAud05 = model.Tab4.AudioProcAud05 == "SI";
                            modelCotejoExt.AudioProcAud06 = model.Tab4.AudioProcAud06 == "SI";
                            modelCotejoExt.AudioProcAud07 = model.Tab4.AudioProcAud07 == "SI";
                            modelCotejoExt.AudioProcAud08 = model.Tab4.AudioProcAud08 == "SI";
                            modelCotejoExt.AudioProcAud09 = model.Tab4.AudioProcAud09 == "SI";
                            modelCotejoExt.AudioProcAud10 = model.Tab4.AudioProcAud10 == "SI";
                        }
                        if (model.Tab4.SelectAudiologia == "AD")
                        {
                            modelCotejoExt.AudioPerdAdul01 = model.Tab4.AudioPerdAdul01 == "SI";
                            modelCotejoExt.AudioPerdAdul02 = model.Tab4.AudioPerdAdul02 == "SI";
                            modelCotejoExt.AudioPerdAdul03 = model.Tab4.AudioPerdAdul03 == "SI";
                            modelCotejoExt.AudioPerdAdul04 = model.Tab4.AudioPerdAdul04 == "SI";
                            modelCotejoExt.AudioPerdAdul05 = model.Tab4.AudioPerdAdul05 == "SI";
                            modelCotejoExt.AudioPerdAdul06 = model.Tab4.AudioPerdAdul06 == "SI";
                            modelCotejoExt.AudioPerdAdul07 = model.Tab4.AudioPerdAdul07 == "SI";
                            modelCotejoExt.AudioPerdAdul08 = model.Tab4.AudioPerdAdul08 == "SI";
                            modelCotejoExt.AudioPerdAdul09 = model.Tab4.AudioPerdAdul09 == "SI";
                            modelCotejoExt.AudioPerdAdul10 = model.Tab4.AudioPerdAdul10 == "SI";
                        }
                    }
                    if (model.Tab3.EvaluarPsicologia)
                    {
                        modelCotejoExt.Psicologia01 = model.Tab4.SelectPsicologia;
                    }

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

                    // Guardar triajeCotejo
                    TriageCotejo modelCotejo = new TriageCotejo();
                    modelCotejo.CreatedBy = "Public";
                    modelCotejo.CreatedDate = DateTime.Now;
                    modelCotejo.CotejoEstado = "PEN"; // PEN, ENV=enviado, RES=respuesta

                    var id = await _triajeOnlineService.Crear(modelTriaje, modelCotejoExt, modelCotejo);

                    // Guardar datos para perfil
                    if (model.Tab2.GuardarEnPerfil)
                    {
                        if (model.Tab2.Dependiente == "SI")
                        {
                            await GuardarEnPerfil(model.Tab2);
                        }
                        else
                        {
                            await GuardarEnPerfil2(model.Tab1);
                        }
                    }

                    await SaveLog(1, "TriageOnline", true, "");
                    ViewBag.Exito = "Solicitud Nro :" + id.ToString();
                    return RedirectToAction(nameof(ConfirmacionTriaje), new { mensaje = "Solicitud Nro: " + id.ToString() });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    await SaveLog(3, "TriageOnline", false, ex.Message);
                    return View(model);
                }
            }
            ViewBag.Error = "Error al enviar información";

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmacionTriaje(string mensaje) {
            ViewBag.Exito = mensaje;
            return  View(); 
        }

        public DateTime GetEnglishDate(string SpanishDate)
        {
            DateTime output;
            System.DateTime.TryParse(SpanishDate, System.Globalization.CultureInfo.CreateSpecificCulture("es"), System.Globalization.DateTimeStyles.None, out output);
            return output;
        }

        public async Task GuardarEnPerfil(Tab2 model)
        {
            try
            {
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                PerfilUsuario perfil = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);
                if (perfil != null)
                {
                    perfil.Nombres = model.ApoderadoNombre?.ToUpper();
                    perfil.ApellidoPaterno = model.ApoderadoApellidoPaterno?.ToUpper();
                    perfil.ApellidoMaterno = model.ApoderadoApellidoMaterno?.ToUpper();
                    perfil.Telefono = model.ApoderadoTelCelular;
                    perfil.TipoDoc = model.ContactoTipoDoc;
                    perfil.Dni = model.ContactoNroDoc;
                    perfil.FechaNacimiento = Convert.ToDateTime(model.ContactoFechaNac);
                    perfil.ActualizadoPor = User.Identity.Name;
                    perfil.Sexo = model.ContactoSexo;

                    await _usuarioService.Editar(perfil);
                    await SaveLog(1, "PerfilUsuario", true, "");
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "PerfilUsuario", false, ex.Message);
            }

        }

        public async Task GuardarEnPerfil2(Tab1 model)
        {
            try
            {
                var appUsuario = await _userManagerService.FindByNameAsync(User.Identity.Name);
                PerfilUsuario perfil = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);
                if (perfil != null)
                {
                    perfil.Nombres = model.NombresPaciente?.ToUpper();
                    perfil.ApellidoPaterno = model.ApellidoPaternoPaciente?.ToUpper();
                    perfil.ApellidoMaterno = model.ApellidoMaternoPaciente?.ToUpper();
                    perfil.TipoDoc = model.TipoDocumento;
                    perfil.Dni = model.PacienteDNI;
                    perfil.FechaNacimiento = Convert.ToDateTime(model.FechaNacimientoPaciente);
                    perfil.ActualizadoPor = User.Identity.Name;
                    perfil.Sexo = model.SexoPaciente;

                    await _usuarioService.Editar(perfil);
                    await SaveLog(1, "PerfilUsuario", true, "");
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "PerfilUsuario", false, ex.Message);
            }

        }
    }
}
