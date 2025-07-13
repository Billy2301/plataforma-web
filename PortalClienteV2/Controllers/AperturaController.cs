using Azure;
using BL.IServices;
using BL.Services;
using culqi.net;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using PortalClienteV2.Utilities.Response;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    [ClearHcPacienteTempData]
    public class AperturaController : BaseController
    {
        private readonly IPagosTriajeService _pagosTriajeService;
        private readonly IEspecialistaService _especialistaService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioService _usuarioService;
        private readonly ITriajeOnlineService _triajeOnlineService;
        private readonly IWebHostEnvironment _environment;
        private readonly IPagoCitaService _pagoCitaService;

        Security security = null;
        Configuracion config = null;

        public AperturaController(ITriajeOnlineService triajeOnlineService,IEspecialistaService especialistaService, IPagosTriajeService pagosService, IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService, UserManager<IdentityUser> userManager, IUsuarioService usuarioService, IWebHostEnvironment environment, IPagoCitaService pagoCitaService)
        : base(accesor, configuration, logService)
        {
            _pagosTriajeService = pagosService;
            _especialistaService = especialistaService;
            _userManager = userManager;
            _usuarioService = usuarioService;
            _triajeOnlineService = triajeOnlineService;
            security = securityKeysApertura();
            config = CurrentConfig();
            _environment = environment;
            _pagoCitaService = pagoCitaService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Evaluaciones(int id)
        {
            if (id != 0)
            {
                try
                {
                    ViewData["publicKey"] = security.public_key;
                    var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);

                    AperturaViewModel model = new AperturaViewModel();
                    model.ListaPagoCitasSearch = await _pagosTriajeService.GetPagosCitasList(id, appUsuario.Id);
                    model.TipoComprobante = 1;
                    model.PagoCitaId = id;
                    model.TriajeOnlineId = model.ListaPagoCitasSearch.Count > 0 ? model.ListaPagoCitasSearch[0].TriajeOnlineId : null;
                    model.Hc = model.ListaPagoCitasSearch.Count > 0 ? model.ListaPagoCitasSearch[0].HistoriaClinica : null;
                    model.sedeId = model.ListaPagoCitasSearch.Count > 0 ? model.ListaPagoCitasSearch[0].SedeId : 1;

                    await SaveLog(1, "AperturaViewModel", true,"");
                    return View(model);
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "AperturaViewModel", false, ex.Message);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Evaluaciones(AperturaViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            ViewData["publicKey"] = security.public_key;
            var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
            model.ListaPagoCitasSearch = await _pagosTriajeService.GetPagosCitasList(Convert.ToInt32(model.PagoCitaId), appUsuario!.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    UvPacienteTriajeSearch modelTriaje = await _triajeOnlineService.GetTriageOnline(Convert.ToInt32(model.TriajeOnlineId));

                    PagosTriaje entity = new PagosTriaje();

                    entity.IdToken = model.Cargo.TokenId;
                    entity.Sede = model.sedeId == 1 ? "Surco" : "Magdalena";
                    entity.TipoBoletaFactura = model.TipoComprobante == 1 ? "Boleta" : "Factura";
                    entity.RucClienteFacturacion = model.NroDocumento;
                    entity.NombreClienteFacturacion = model.RazonSocial;
                    entity.Estado = 0;
                    entity.ActualizadoPor = "";
                    entity.UltimaActualizacion = null;
                    entity.Atencion = null;
                    entity.NroTriaje = model.TriajeOnlineId.ToString();
                    entity.NroHc = model.Hc.ToString();
                    entity.PagoCitaIdRef = model.PagoCitaId;
                    entity.CitasDetalle = model.CitasSeleccionadas;
                    entity.NombreCliente = modelTriaje?.NombresPaciente;
                    entity.ApellidoCliente = modelTriaje?.ApellidoPaternoPaciente + " " + modelTriaje?.ApellidoMaternoPaciente;
                    entity.DireccionCliente = model.Direccion != null ? model.Direccion : string.Empty;
                    entity.CiudadCliente = "";
                    entity.CodigoPaisCliente = "";
                    entity.TelefonoCliente = modelTriaje?.TelefonoPaciente;
                    entity.DniCliente = modelTriaje?.PacienteDni;
                    entity.CorreoElectronico = appUsuario.Email;
                    entity.TipoDocCliente = model.TipoDocumento;

                    //Culqi Cargo
                    decimal montoDecimal = Convert.ToDecimal(model.Monto);
                    int pagoExternoMonto = (int)(montoDecimal * 100);
                    string email_cliente = string.IsNullOrEmpty(model.Cargo.Email) ? appUsuario.Email!.ToString() : model.Cargo.Email;
                    string description_ = "Pago apertura de citas";
                    int installments_ = Convert.ToInt32(model.Cargo.Installments);
                    string token_id = model.Cargo.TokenId;

                    string direccion = string.IsNullOrEmpty(model.Direccion) ? "Avenida Lima 123" : model.Direccion;
                    string razonSocial = string.IsNullOrEmpty(model.RazonSocial) ? "Cliente" : model.RazonSocial;
                    string telefono = string.IsNullOrEmpty(model.Telefono) ? "999777666" : model.Telefono;
                    string nroDocumento = string.IsNullOrEmpty(model.NroDocumento) ? "00000000" : model.NroDocumento;

                    bool isError = false;
                    string error_desc = "";
                    string responseValue = "";

                    string first_name = string.IsNullOrEmpty(entity.NombreCliente) ? "Culqi" : entity.NombreCliente;
                    string last_name = string.IsNullOrEmpty(entity.ApellidoCliente) ? "Core" : entity.ApellidoCliente;

                    Dictionary<string, object> antifraud_details = new Dictionary<string, object>
                    {
                      {"address", direccion},
                      {"address_city", "Lima"},
                      {"country_code", "PE"},
                      {"first_name", first_name },
                      {"last_name", last_name},
                      {"phone_number", telefono}
                    };

                    Dictionary<string, object> map = new Dictionary<string, object>
                    {
                      {"amount", pagoExternoMonto.ToString()},
                      {"capture", true},
                      {"currency_code", "PEN"},
                      {"email", email_cliente},
                      {"description", description_},
                      {"installments", installments_},
                      {"source_id", token_id},
                      {"antifraud_details", antifraud_details}
                    };

                    ResponseCulqi json_object = new Charge(security).Create(map);
                    //return json_object;

                    JObject data = JObject.Parse(json_object.body);
                    string? id_object = data["object"]?.ToString();

                    // Captura de propiedades anidadas en las propiedades de la entidad
                    entity.IdCargo = data["id"]?.ToString();
                    entity.Duplicado = data["duplicated"]?.ToObject<bool?>();
                    long? FechaCreacion = data["creation_date"]?.ToObject<long?>();
                    if (FechaCreacion.HasValue) 
                    {
                        // Convertir de Unix time a DateTime en UTC
                        DateTime utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(FechaCreacion.Value).UtcDateTime;
                        // Convertir la fecha y hora UTC a la hora local del servidor
                        DateTime localDateTime = utcDateTime.ToLocalTime();
                        // Guardar la fecha y hora ajustada
                        entity.FechaCreacion = localDateTime;
                    }
                    entity.Monto = data["amount"]?.ToObject<decimal?>();
                    entity.MontoReembolsado = data["amount_refunded"]?.ToObject<decimal?>();
                    entity.MontoActual = data["current_amount"]?.ToObject<decimal?>();
                    entity.Cuotas = data["installments"]?.ToObject<int?>();
                    entity.MontoCuotas = 0;
                    entity.CodigoMoneda = data["currency_code"]?.ToString();
                    //entity.CorreoElectronicoToken = data["email"]?.ToString();
                    entity.Descripcion = data["description"]?.ToString();
                    entity.PuntajeFraude = data["fraud_score"]?.ToObject<int?>();
                    entity.Disputa = data["dispute"]?.ToObject<bool?>();
                    entity.Captura = data["capture"]?.ToObject<bool?>();
                    entity.CodigoReferencia = data["reference_code"]?.ToString();
                    entity.CodigoAutorizacion = data["authorization_code"]?.ToString();
                    entity.TotalComision = data["fee_details"]?["variable_fee"]?["commision"]?.ToObject<decimal?>();
                    entity.TotalImpuestosComision = data["total_fee_taxes"]?.ToObject<decimal?>();
                    entity.MontoTransferido = data["transfer_amount"]?.ToObject<decimal?>();
                    entity.Pagado = data["paid"]?.ToObject<bool?>();
                    entity.DescriptorEstado = data["statement_descriptor"]?.ToString();
                    entity.IdTransferencia = data["transfer_id"]?.ToString();
                    long? FechaCaptura = data["capture_date"]?.ToObject<long?>();
                    if (FechaCaptura.HasValue)
                    { entity.FechaCaptura = DateTimeOffset.FromUnixTimeMilliseconds(FechaCaptura.Value).DateTime.ToLocalTime(); }
                    entity.IdToken = data["source"]?["id"]?.ToString();
                    entity.Tipo = "Card";
                    long? FechaCreacionToken = data["source"]?["creation_date"]?.ToObject<long?>();
                    if (FechaCreacionToken.HasValue)
                    { entity.FechaCreacionToken = DateTimeOffset.FromUnixTimeMilliseconds(FechaCreacionToken.Value).DateTime.ToLocalTime(); }
                    entity.CorreoElectronicoToken = data["source"]?["email"]?.ToString();
                    entity.NumeroTarjeta = data["source"]?["card_number"]?.ToString();
                    entity.UltimosCuatro = data["source"]?["last_four"]?.ToString();
                    entity.Activo = data["source"]?["active"]?.ToObject<bool?>();
                    entity.TipoRespuesta = data["outcome"]?["type"]?.ToString();
                    entity.CodigoRespuesta = data["outcome"]?["code"]?.ToString();
                    entity.MensajeComercio = data["outcome"]?["merchant_message"]?.ToString();
                    entity.MensajeUsuario = data["outcome"]?["user_message"]?.ToString();
                    entity.DireccionIpCliente = data["source"]?["client"]?["ip"]?.ToString();
                    entity.PaisIpCliente = data["source"]?["client"]?["ip_country"]?.ToString();
                    entity.CodigoPaisIpCliente = data["source"]?["client"]?["ip_country_code"]?.ToString();
                    entity.NavegadorCliente = data["source"]?["client"]?["browser"]?.ToString();
                    entity.HuellaDispositivoCliente = data["source"]?["client"]?["device_fingerprint"]?.ToString();
                    entity.TipoDispositivoCliente = data["source"]?["client"]?["device_type"]?.ToString();
                    entity.Bin = data["source"]?["iin"]?["bin"]?.ToString();
                    entity.MarcaTarjeta = data["source"]?["iin"]?["card_brand"]?.ToString();
                    entity.TipoTarjeta = data["source"]?["iin"]?["card_type"]?.ToString();
                    entity.CategoriaTarjeta = data["source"]?["iin"]?["card_category"]?.ToString();
                    entity.CuotasPermitidas = "";
                    entity.NombreEmisor = data["source"]?["iin"]?["issuer"]?["name"]?.ToString();
                    entity.PaisEmisor = data["source"]?["iin"]?["issuer"]?["country"]?.ToString();
                    entity.CodigoPaisEmisor = data["source"]?["iin"]?["issuer"]?["country_code"]?.ToString();
                    entity.SitioWebEmisor = data["source"]?["iin"]?["issuer"]?["website"]?.ToString();
                    entity.NumeroTelefonoEmisor = data["source"]?["iin"]?["issuer"]?["phone_number"]?.ToString();

                    entity.Sponsor = "portalusuario"; 
                    entity.ClaveOrden = "pu_order_" + DateTime.Now.ToString("yyMMdd") + Guid.NewGuid().ToString("N").Substring(0, 6);
                    //entity.IdOrden = "";
                    entity.PublicarCliente = false;
                    entity.FechaCreacionPedido = DateTime.Now;
                    entity.FechaPagoPedido = DateTime.Now;
                    entity.EstadoPedido = "processing";
                    entity.ViaCreacionPedido = "checkout";
                    //entity.CartHash = ""; 
                    entity.SimboloMonedaPedido = "S/";
                    //entity.Productos = ""; 

                    //save proceso pago  
                    int nivelError = 1;
                    if (!string.IsNullOrEmpty(id_object))
                    {
                        if (id_object.ToLower() == "error")
                        {
                            isError = true;
                            error_desc += "Error: " + data["user_message"];
                            responseValue = "0";
                        }
                    }
                    else
                    {
                        isError = true;
                        error_desc = "Error: Error en procesar el pago.";
                        responseValue = "0";
                    }

                    if (isError)
                    {
                        entity.MensajeUsuario = error_desc;
                        entity.TipoRespuesta = id_object.ToLower();
                        entity.CodigoRespuesta = "ERR0000";
                        nivelError = 2;
                    }

                    
                    if (!isError) //Si NO existe error
                    {
                        //guardar pagoCita
                        int mNew_ID = await _pagosTriajeService.GuardarPagoTriaje(entity);

                        //Actualizar pagoCita
                        await _pagosTriajeService.updatePagoCitasToPagado(Convert.ToInt32(entity.PagoCitaIdRef), mNew_ID);
                        //ActualizarPagocitaDetalle
                        await _pagosTriajeService.updatePagoCitaDetalleToPagado(Convert.ToInt32(entity.PagoCitaIdRef), entity.CitasDetalle);
                        
                        ////envia email
                        var path = Path.Combine(_environment.WebRootPath, "Templates", "TemplatePagoOK.html");
                        string templateContent = System.IO.File.ReadAllText(path);
                        await _pagosTriajeService.NotificacionPago(entity, templateContent, appUsuario.UserName);
                        await SaveLog(nivelError, "PagosTriaje", true, "Pago Apertura realizado Nro: " + mNew_ID.ToString());

                        ViewBag.Exito = "Pago realizado !";
                        return RedirectToAction(nameof(PagoConfirmado));
                    }

                    await SaveLog(nivelError, "PagosTriaje", true, error_desc);
                    ViewBag.Error = "Error al realizar pago, intentelo nuevamente ";
                    return View(model);
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "PagosTriaje", false, ex.Message);
                    ViewBag.Error = "Error al realizar pago: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Error al realizar pago";
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult PagoConfirmado()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProgramacionCitas(int id)
        {
            if (id != 0)
            {
                try
                {
                    PagoCita entity = await _pagoCitaService.getPagoCitaById(id);
                    TriageOnline entityTriaje = await _triajeOnlineService.GetTriageOnlineById(Convert.ToInt32(entity.TriajeOnlineId));
                    ProgramacionCitaViewModel model = new ProgramacionCitaViewModel();
                    model.EstadoPago = entity.EstadoPago;
                    model.HistoriaClinica = entity.HistoriaClinica;
                    model.PagoCitaId = entity.PagoCitaId;
                    model.PagoTriajeIdRef = entity.PagoTriajeIdRef;
                    model.TriajeOnlineId = entity.TriajeOnlineId;
                    model.UsuarioId = entity.UsuarioId;
                    model.TiempoDeExpiracion = entity.TiempoDeExpiracion;
                    model.FechaNacPaciente = entityTriaje.FechaNacimientoPaciente;
                    model.TiempoEsperaEnSegundos = Convert.ToInt32(config.getTiempoEsperaEnSeg);
                    model.AppName = config.getIsProduction == "1" ? config.getAppName : string.Empty;
                    model.ListUvPagoCitaDetalle = await _pagoCitaService.getPagoCitaDetalle(id);

                    await SaveLog(1, "AperturaViewModel", true, "");
                    return View(model);
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "AperturaViewModel", false, ex.Message);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarFechaCita([FromBody] CitaModel model)
        {
            try
            {
                if (model != null)
                {
                    // Guardar la citaDetalle en la base de datos
                    PagoCitaDetalle entity = await  _pagoCitaService.getPagoCitaDetalleById(model.PagoCitaDetalleId);
                    entity.EspecialistaId = model.EspecialistaId;
                    entity.HoraCitaDesde = ConvertTimeToDecimal(model.HoraCitaDesde!);
                    entity.HoraCitaHasta = ConvertTimeToDecimal(model.HoraCitaHasta!);
                    entity.FechaCita = Convert.ToDateTime(model.FechaCita);
                    entity.IsProgramado = true;
                    await _pagoCitaService.GuardarPagoCitaDetalle(entity);
                
                    if (model.NumeroCita == 1)
                    {
                        int minutos = Convert.ToInt32(config.getTiempoEsperaEnSeg) / 60;
                        // guardar hora de expiración para el contador
                        PagoCita entityPago = await _pagoCitaService.getPagoCitaById(model.PagoCitaId);
                        entityPago.TiempoDeExpiracion = DateTime.Now.AddMinutes(minutos); //  Agregar min a la hora actual
                        await _pagoCitaService.GuardarPagoCita(entityPago);
                        // Guardar especialista en todas los pagoCitaDetalle
                    }

                    return Ok(new { message = "Cita guardada correctamente" });
                }
                else
                {
                    return BadRequest(new { message = "Error al guardar la cita" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
            


            
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarioDiag(string fecha, int evaluacionID, int edad, int especialistaId,int sedeId,int tipo, double duracion)
        {
            try
            {
                DateTime compareDate = fecha != "" ? Convert.ToDateTime(fecha) : DateTime.Now;
                int minutos = (int)(duracion * 60);
                int rangoId =  (edad >= 8) ? 3 : (edad >= 6) ? 2 : 1;
                List<object> events;
                List<upCalendarioDisponibleDiagVirtualResult> calensarioListVirtual;
                List<upCalendarioDisponibleDiagPresencialResult> calensarioListPresencial;
                if (tipo == 1)
                {
                    calensarioListPresencial = await _especialistaService.GetCalendarioDisponibleDiagnosticoPresencial(evaluacionID, rangoId, especialistaId, sedeId, Convert.ToInt32(config.getDiasCalendario), minutos);
                    events = ConvertToEventsPresencial(calensarioListPresencial, compareDate);
                }
                else
                {
                    calensarioListVirtual = await _especialistaService.GetCalendarioDisponibleDiagnosticoVirtual(evaluacionID, rangoId, especialistaId, sedeId, Convert.ToInt32(config.getDiasCalendario), minutos);
                    events = ConvertToEventsVirtual(calensarioListVirtual, compareDate);
                }
                
                return Json(events);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExpirarCitasProgramadas(int id)
        {
            try
            {
                await _pagoCitaService.ExpirarCitasProgramadas(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet]
        public async Task<IActionResult> PagoCitas(int id)
        {
            if (id != 0)
            {
                try
                {
                    ViewData["publicKey"] = security.public_key;
                    var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                    PerfilUsuario? perfil = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);
                    PagoCita entity = await _pagoCitaService.getPagoCitaById(id);
                    PagoCitasViewModel model = new PagoCitasViewModel();
                    model.ListaPagoCitasSearch = await _pagosTriajeService.GetPagosCitasList(id, appUsuario.Id);
                    model.TipoComprobante = 1;
                    model.TiempoDeExpiracion = entity.TiempoDeExpiracion;
                    model.TiempoEsperaEnSegundos = Convert.ToInt32(config.getTiempoEsperaEnSeg);
                    //continuar...



                    if (perfil != null)
                    {
                        model.NroDocumento = perfil.Dni != null ? perfil.Dni : "";
                        model.RazonSocial = perfil.Nombres + " " + perfil.ApellidoPaterno + " " + perfil.ApellidoMaterno;
                        model.Telefono = perfil.Telefono;
                        model.Direccion = perfil.Dirección;
                    }
                    return View(model);
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "PagoCitasViewModel", false, ex.Message);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }

        #region Metodos Internos
        static List<object> ConvertToEventsPresencial(List<upCalendarioDisponibleDiagPresencialResult> calendarioList, DateTime compareDate)
        {
            var eventsVirtual = new List<Object>();

            foreach (var item in calendarioList)
            {
                var index = item.DiaIndex;
                var fecha = item.Fecha;

                string hora8List = item.Hora8List.ToString();
                string hora9List = item.Hora9List.ToString();
                string hora10List = item.Hora10List.ToString();
                string hora11List = item.Hora11List.ToString();
                string hora12List = item.Hora12List.ToString();
                string hora13List = item.Hora13List.ToString();
                string hora14List = item.Hora14List.ToString();
                string hora15List = item.Hora15List.ToString();
                string hora16List = item.Hora16List.ToString();
                string hora17List = item.Hora17List.ToString();
                string hora18List = item.Hora18List.ToString();
                string hora19List = item.Hora19List.ToString();
                string hora20List = item.Hora20List.ToString();

                DateTime hora8_inicio = item.Hora8Inicio;
                DateTime hora9_inicio = item.Hora9Inicio;
                DateTime hora10_inicio = item.Hora10Inicio;
                DateTime hora11_inicio = item.Hora11Inicio;
                DateTime hora12_inicio = item.Hora12Inicio;
                DateTime hora13_inicio = item.Hora13Inicio;
                DateTime hora14_inicio = item.Hora14Inicio;
                DateTime hora15_inicio = item.Hora15Inicio;
                DateTime hora16_inicio = item.Hora16Inicio;
                DateTime hora17_inicio = item.Hora17Inicio;
                DateTime hora18_inicio = item.Hora18Inicio;
                DateTime hora19_inicio = item.Hora19Inicio;
                DateTime hora20_inicio = item.Hora20Inicio;

                DateTime hora8_fin = item.Hora8Fin;
                DateTime hora9_fin = item.Hora9Fin;
                DateTime hora10_fin = item.Hora10Fin;
                DateTime hora11_fin = item.Hora11Fin;
                DateTime hora12_fin = item.Hora12Fin;
                DateTime hora13_fin = item.Hora13Fin;
                DateTime hora14_fin = item.Hora14Fin;
                DateTime hora15_fin = item.Hora15Fin;
                DateTime hora16_fin = item.Hora16Fin;
                DateTime hora17_fin = item.Hora17Fin;
                DateTime hora18_fin = item.Hora18Fin;
                DateTime hora19_fin = item.Hora19Fin;
                DateTime hora20_fin = item.Hora20Fin;

                // Agregar eventos para cada hora
                AgregarEvento(hora8List, hora8_inicio, hora8_fin, eventsVirtual, "presencial", compareDate);
                AgregarEvento(hora9List, hora9_inicio, hora9_fin, eventsVirtual, "presencial", compareDate);
                AgregarEvento(hora10List, hora10_inicio, hora10_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora11List, hora11_inicio, hora11_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora12List, hora12_inicio, hora12_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora13List, hora13_inicio, hora13_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora14List, hora14_inicio, hora14_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora15List, hora15_inicio, hora15_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora16List, hora16_inicio, hora16_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora17List, hora17_inicio, hora17_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora18List, hora18_inicio, hora18_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora19List, hora19_inicio, hora19_fin, eventsVirtual, "presencial",compareDate);
                AgregarEvento(hora20List, hora20_inicio, hora20_fin, eventsVirtual, "presencial",compareDate);

            }

            return eventsVirtual;
        }
        static List<object> ConvertToEventsVirtual(List<upCalendarioDisponibleDiagVirtualResult> calendarioList, DateTime compareDate)
        {
            var eventsVirtual = new List<Object>();

            foreach (var item in calendarioList)
            {
                var index = item.DiaIndex;
                var fecha = item.Fecha;

                string hora8List = item.Hora8List.ToString();
                string hora9List = item.Hora9List.ToString();
                string hora10List = item.Hora10List.ToString();
                string hora11List = item.Hora11List.ToString();
                string hora12List = item.Hora12List.ToString();
                string hora13List = item.Hora13List.ToString();
                string hora14List = item.Hora14List.ToString();
                string hora15List = item.Hora15List.ToString();
                string hora16List = item.Hora16List.ToString();
                string hora17List = item.Hora17List.ToString();
                string hora18List = item.Hora18List.ToString();
                string hora19List = item.Hora19List.ToString();
                string hora20List = item.Hora20List.ToString();

                DateTime hora8_inicio = item.Hora8Inicio;
                DateTime hora9_inicio = item.Hora9Inicio;
                DateTime hora10_inicio = item.Hora10Inicio;
                DateTime hora11_inicio = item.Hora11Inicio;
                DateTime hora12_inicio = item.Hora12Inicio;
                DateTime hora13_inicio = item.Hora13Inicio;
                DateTime hora14_inicio = item.Hora14Inicio;
                DateTime hora15_inicio = item.Hora15Inicio;
                DateTime hora16_inicio = item.Hora16Inicio;
                DateTime hora17_inicio = item.Hora17Inicio;
                DateTime hora18_inicio = item.Hora18Inicio;
                DateTime hora19_inicio = item.Hora19Inicio;
                DateTime hora20_inicio = item.Hora20Inicio;

                DateTime hora8_fin = item.Hora8Fin;
                DateTime hora9_fin = item.Hora9Fin;
                DateTime hora10_fin = item.Hora10Fin;
                DateTime hora11_fin = item.Hora11Fin;
                DateTime hora12_fin = item.Hora12Fin;
                DateTime hora13_fin = item.Hora13Fin;
                DateTime hora14_fin = item.Hora14Fin;
                DateTime hora15_fin = item.Hora15Fin;
                DateTime hora16_fin = item.Hora16Fin;
                DateTime hora17_fin = item.Hora17Fin;
                DateTime hora18_fin = item.Hora18Fin;
                DateTime hora19_fin = item.Hora19Fin;
                DateTime hora20_fin = item.Hora20Fin;

                // Agregar eventos para cada hora
                AgregarEvento(hora8List, hora8_inicio, hora8_fin, eventsVirtual, "virtual", compareDate);
                AgregarEvento(hora9List, hora9_inicio, hora9_fin, eventsVirtual, "virtual", compareDate);
                AgregarEvento(hora10List, hora10_inicio, hora10_fin, eventsVirtual, "virtual", compareDate);
                AgregarEvento(hora11List, hora11_inicio, hora11_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora12List, hora12_inicio, hora12_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora13List, hora13_inicio, hora13_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora14List, hora14_inicio, hora14_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora15List, hora15_inicio, hora15_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora16List, hora16_inicio, hora16_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora17List, hora17_inicio, hora17_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora18List, hora18_inicio, hora18_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora19List, hora19_inicio, hora19_fin, eventsVirtual, "virtual",compareDate);
                AgregarEvento(hora20List, hora20_inicio, hora20_fin, eventsVirtual, "virtual",compareDate);

            }

            return eventsVirtual;
        }
        static void AgregarEvento(string horaList, DateTime horaInicio, DateTime horaFin, List<object> eventsVirtual,string tipo, DateTime compareDate)
        {
            if (!string.IsNullOrEmpty(horaList))
            {
                bool disabled = horaInicio.Date <= compareDate.Date ? true: false;
                foreach (var part in horaList.Split(';'))
                {
                    string[] str_part = part.Split('|');
                    eventsVirtual.Add(new
                    {
                        id = str_part[3], // Asumiendo que tienes una propiedad 'Id'
                        title = str_part[0], //tipo, // Asumiendo que tienes una propiedad 'Titulo'
                        start = horaInicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de inicio
                        end = horaFin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de fin
                        extendedProps = new
                        {
                            calendar = disabled ? "disabled" : str_part[2],
                            especialista = str_part[0],
                            especialistaId = str_part[1]
                        }
                    });
                }
            }
        }
        public static string ConvertDecimalToTime(float? DecimalValue)
        {
            string mReturn = "";

            string mHours = "", mMinutes = "";

            if (DecimalValue.HasValue)
            {
                decimal DecimalValue2 = Convert.ToDecimal(DecimalValue.Value);
                mHours = decimal.Truncate(DecimalValue2).ToString();

                if (mHours.Length != 2) { mHours = "0" + mHours; }

                mMinutes = Convert.ToInt16((DecimalValue2 - decimal.Truncate(DecimalValue2)) * 60).ToString();
                if (mMinutes.Length != 2) { mMinutes = "0" + mMinutes; }

                mReturn = mHours + ":" + mMinutes;
            }

            return mReturn;

        }
        public static float ConvertTimeToDecimal(string time)
        {
            // Verificamos si la cadena es válida y está en formato HH:mm
            if (string.IsNullOrEmpty(time) || !time.Contains(":"))
            {
                throw new ArgumentException("El formato de tiempo debe ser HH:mm.");
            }

            // Separamos horas y minutos
            string[] parts = time.Split(':');
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);

            // Convertimos las horas a decimal y los minutos a fracción de hora
            float decimalTime = hours + (minutes / 60f);

            return decimalTime;
        }
        public async Task<IActionResult> VerRespuesta(int id)
        {
            // metodo para mostrar la respuesta de correo eniado por la especialista
            try
            {
                TriageOnline modelTriaje = await _triajeOnlineService.GetTriageOnlineById(id);

                // Crear una instancia de ViewModelRespuesta con datos de ejemplo
                var respuesta = new RespuestaTriajeViewModel
                {
                    TriageOnlineId = modelTriaje.TriageOnlineId,
                    UltimaActualizacion = modelTriaje.UltimaActualizacion.HasValue ? modelTriaje.UltimaActualizacion.Value.ToString("dddd, dd 'de' MMMM 'de' yyyy") : "",
                    ActualizadoPor = modelTriaje.ActualizadoPor != null ? modelTriaje.ActualizadoPor : "",
                    EmailProcedimiento1 = modelTriaje.EmailProcedimiento1 != null ? modelTriaje.EmailProcedimiento1 : "",
                    EmailProcedimiento2 = modelTriaje.EmailProcedimiento2 != null ? modelTriaje.EmailProcedimiento2 : "",
                    EmailProcedimiento3 = modelTriaje.EmailProcedimiento3 != null ? modelTriaje.EmailProcedimiento3 : ""
                };

                // Devolver la respuesta como JSON
                return Json(respuesta);
            }
            catch (Exception)
            {

                return null;
            }
        }

        #endregion


        [HttpPost]
        public async Task<IActionResult> GenerarOrden([FromBody] JsonElement modelo)
        {
            GenericResponse<ResponseCulqi> response = new GenericResponse<ResponseCulqi>();

            try
            {


                var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                PerfilUsuario? user = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);

                Int32 amount = modelo.GetProperty("amount").GetInt32();
                string? currency_code = modelo.GetProperty("currency").GetString();
                string? description = modelo.GetProperty("description").GetString();
                string? order_number = "port_clie_" + Helper.GenerateShortGuid() + DateTimeOffset.Now.ToUnixTimeSeconds();
                int totaPagar = modelo.GetProperty("customer.amount").GetInt32();
                string? Area = modelo.GetProperty("customer.area").GetString();
                string? Currency = modelo.GetProperty("customer.currency").GetString();
                string? Description = modelo.GetProperty("customer.description").GetString();
                string? Direccionpago = modelo.GetProperty("customer.direccionpago").GetString();
                string? Email = User.Identity.Name; // modelo.GetProperty("customer.email").GetString();
                string? Hc = modelo.GetProperty("customer.hc").GetString();
                string? Razonsocial = modelo.GetProperty("customer.razonsocial").GetString();
                string? Ruc = modelo.GetProperty("customer.ruc").GetString();
                string? Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString();
                int TotalPagarReal = modelo.GetProperty("customer.totalpagar").GetInt32();
                bool conDni = modelo.GetProperty("customer.conDni").GetBoolean();


                DateTimeOffset fechaActual = DateTimeOffset.Now;
                DateTimeOffset fechaMasUnDia = fechaActual.AddDays(1);
                long epoch = fechaMasUnDia.ToUnixTimeSeconds();

                Dictionary<string, object> client_details = new Dictionary<string, object>
                    {
                        {"first_name", user.Nombres == null? "Cliente" : user.Nombres  + ""},
                        {"last_name", user.ApellidoPaterno == null ? "Cliente" : user.ApellidoPaterno + " " + user.ApellidoMaterno},
                        {"email", appUsuario.UserName + ""},
                        {"phone_number", user.Telefono == null? "999999999": user.Telefono + ""}
                    };

                Dictionary<string, object> map = new Dictionary<string, object>
                    {
                        {"amount",amount},
                        {"currency_code", currency_code},
                        {"description", description},
                        {"order_number", order_number},
                        {"client_details", client_details},
                        {"expiration_date", epoch},
                        {"confirm", false}
                    };

                ResponseCulqi json_object = new Order(security).Create(map);

                response.Estado = true;
                response.Objeto = json_object;

                await SaveLog(1, "Order", true, "");

            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "Order", false, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, response);

        }


        [HttpPost]
        public async Task<IActionResult> GuardarOrden([FromBody] JsonElement modelo)
        {
            GenericResponse<ResponseCulqi> response = new GenericResponse<ResponseCulqi>();
            try
            {

                if (modelo.ValueKind == JsonValueKind.Null || (modelo.ValueKind == JsonValueKind.Object && !modelo.EnumerateObject().Any()))
                {
                    response.Estado = false;
                    response.Mensaje = "Error al guardar la Orden, el QR, Codigo, PagoEfectivo sera invalido para CPAL";

                    return StatusCode(StatusCodes.Status200OK, response);
                }
                //Orden mOrden = JsonConvert.DeserializeObject<Orden>(modelo.ToString());
                Orden mOrden = new Orden();

                mOrden.OrderId = modelo.GetProperty("orderId").GetString();
                mOrden.OrderNumber = modelo.GetProperty("orderNumber").GetString();
                mOrden.PaymenCode = modelo.GetProperty("paymenCode").GetString();
                mOrden.Description = modelo.GetProperty("description").GetString();
                mOrden.Object = modelo.GetProperty("object").GetString();
                mOrden.Amount = modelo.GetProperty("amount").GetDecimal();
                mOrden.Customer = new Cliente();

                mOrden.Customer.Amount = modelo.GetProperty("customer.amount").GetInt32();
                mOrden.Customer.Area = modelo.GetProperty("customer.area").GetString();
                mOrden.Customer.Currency = modelo.GetProperty("customer.currency").GetString();
                mOrden.Customer.Description = modelo.GetProperty("customer.description").GetString();
                mOrden.Customer.Direccionpago = modelo.GetProperty("customer.direccionpago").GetString();
                mOrden.Customer.Email = User.Identity.Name; //modelo.GetProperty("customer.email").GetString();
                mOrden.Customer.Hc = modelo.GetProperty("customer.hc").GetString();
                mOrden.Customer.Pagoid = modelo.GetProperty("customer.pagoid").GetInt32();
                mOrden.Customer.Razonsocial = modelo.GetProperty("customer.razonsocial").GetString();
                mOrden.Customer.Ruc = modelo.GetProperty("customer.ruc").GetString();
                mOrden.Customer.Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString();
                mOrden.Customer.TotalPagar = modelo.GetProperty("customer.totalpagar").GetInt32();
                mOrden.Customer.SedeId = modelo.GetProperty("customer.sedeId").GetInt32();
                mOrden.Customer.TriajeOnLineId = modelo.GetProperty("customer.triajeOnId").GetString();
                mOrden.Customer.Detalle = modelo.GetProperty("customer.detalle").GetString();





                //save proceso pago
                PagosTriaje entity = new PagosTriaje();
                PagoCita entityPago;
                int mSedeID = 0;

                entityPago = await _pagoCitaService.getPagoCitaById(Convert.ToInt32(mOrden.Customer.Pagoid));

                //if (entityPago != null) { mSedeID = entityPago.SedeId; }

                entity.PagoCitaIdRef = Convert.ToInt32(mOrden.Customer.Pagoid);
                entity.EstadoPedido = "PENDIENTE";
                entity.Monto = 0;
                if (!string.IsNullOrEmpty(mOrden.Customer.Amount.ToString()))
                {
                    if (Helper.IsNumeric(mOrden.Customer.TotalPagar.ToString())) { entity.Monto = Convert.ToDecimal(mOrden.Customer.TotalPagar); }
                }
                if (!string.IsNullOrEmpty(mOrden.Customer.Hc))
                {
                    entity.NroHc = mOrden.Customer.Hc;
                }
                if (!string.IsNullOrEmpty(mOrden.Customer.TriajeOnLineId))
                {
                    TriageOnline eTriageOnline = await _triajeOnlineService.GetTriageOnlineById(Convert.ToInt32(mOrden.Customer.TriajeOnLineId));
                    entity.NroTriaje = eTriageOnline.TriageOnlineId.ToString();
                    entity.DniCliente = eTriageOnline.PacienteDni;
                    entity.NombreCliente = eTriageOnline.NombresPaciente;
                    entity.ApellidoCliente = string.Concat(eTriageOnline.ApellidoPaternoPaciente," ", eTriageOnline.ApellidoPaternoPaciente).Trim() ;
                    entity.DniCliente = eTriageOnline.PacienteDni;
                }
                

                entity.CorreoElectronico = User.Identity.Name;
                entity.FechaCreacion = DateTime.Now;
                entity.SimboloMonedaPedido = "S/.";
                entity.CodigoMoneda = "PEN"; ;
                entity.IdCargo = mOrden.OrderId;
                entity.IdToken = mOrden.OrderNumber;
                //entityPagoTriaje.TransactionResponse = mOrden.Object;
                entity.TipoBoletaFactura = mOrden.Customer.Tipodocpago == "RUC" ? "Factura" : "Boleta";
                entity.Estado = 0;
                entity.CodigoReferencia = mOrden.PaymenCode;
                entity.Descripcion = mOrden.Description;
                entity.Sede = mOrden.Customer.SedeId == 1? "Surco": "Magdalena";
                entity.CitasDetalle = mOrden.Customer.Detalle;
                entity.Duplicado = false;
                entity.DescriptorEstado = "CULQI*";
                entity.MensajeUsuario = "Pendiente de pago";
                entity.MensajeComercio = "Pendiente de pago";
                entity.TipoTarjeta = "Pago Efectivo";
                entity.FechaCreacionPedido = DateTime.Now;
                entity.ViaCreacionPedido = "checkout";
                entity.CodigoRespuesta = "PEND00";
                entity.FechaCreacionToken = DateTime.Now;
                entity.CorreoElectronicoToken = User.Identity.Name;

                entity.TipoDocCliente = mOrden.Customer.Tipodocpago;
                if (!string.IsNullOrEmpty(mOrden.Customer.Razonsocial)) { entity.NombreClienteFacturacion = mOrden.Customer.Razonsocial; } else { entity.NombreClienteFacturacion = null; }
                if (!string.IsNullOrEmpty(mOrden.Customer.Ruc)) { entity.RucClienteFacturacion = mOrden.Customer.Ruc; } else { entity.RucClienteFacturacion = null; }
                if (!string.IsNullOrEmpty(mOrden.Customer.Direccionpago)) { entity.DireccionCliente = mOrden.Customer.Direccionpago; } else { entity.DireccionCliente = null; }

                //entityPagoTriaje.SedeId = mSedeID;

                await _pagosTriajeService.GuardarPagoTriaje(entity);
                int mNew_ID = entity.PagoId;

                ResponseCulqi json_object = new ResponseCulqi()
                {
                    statusCode = 200,
                    body = JsonConvert.SerializeObject(entity)
                };

                string paymentCode = "";
                string qr = "";
                using (JsonDocument doc = JsonDocument.Parse(mOrden.Object!))
                {
                    JsonElement root = doc.RootElement;

                    paymentCode = root.TryGetProperty("payment_code", out JsonElement paymentCodeElement) && paymentCodeElement.ValueKind != JsonValueKind.Null
                        ? paymentCodeElement.GetString()
                        : "N/A";

                    qr = root.TryGetProperty("qr", out JsonElement qrElement) && qrElement.ValueKind != JsonValueKind.Null
                        ? qrElement.GetString()
                        : "N/A";
                }

                await SaveLog(1, "PagoTriaje", true, "Apertura (Pago Efectivo) guardado pago nro: " + mNew_ID.ToString());
                response.Estado = true;
                response.Objeto = json_object;
                response.Mensaje = string.Format("Puede realizar su pago en cualquier agente bancario utilizando el código CIP <br> <h2><b>{0}<b></h2> o Scanear el código QR desde tu billetera móvil <br><img src='{1}' style='max-width:250px;' />", paymentCode, qr);
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "PagoTriaje", false, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }















        #region Borradores

        [HttpGet]
        public async Task<IActionResult> GetEspecialistas(int id, int edad)
        {
            int rangoId = 1;
            if (edad >= 8)
            {
                rangoId = 3;
            }
            else if (edad >= 6)
            {
                rangoId = 2;
            }
            else if (edad >= 2)
            {
                rangoId = 1;
            }

            var especialistas = await _especialistaService.GetPersonalEvaluacionByRangoEdad(id, null, 0);
            return Json(especialistas);
        }
        static Dictionary<string, List<object>> ConvertToEvents(List<upCalendarioDisponibleDiagV3PortalResult> calendarioList)
        {
            var events = new Dictionary<string, List<object>>();
            var eventsPresencial = new List<Object>();
            var eventsVirtual = new List<Object>();

            foreach (var item in calendarioList)
            {
                var index = item.DiaIndex;
                var nroDia = item.DiaSemana;
                var nombreDia = item.NombreDiaSemana;
                var fecha = item.Fecha;

                string hora8List =  item.Hora8List.ToString();
                string hora9List =  item.Hora9List.ToString();
                string hora10List = item.Hora10List.ToString();
                string hora11List = item.Hora11List.ToString();
                string hora12List = item.Hora12List.ToString();
                string hora13List = item.Hora13List.ToString();
                string hora14List = item.Hora14List.ToString();
                string hora15List = item.Hora15List.ToString();
                string hora16List = item.Hora16List.ToString();
                string hora17List = item.Hora17List.ToString();
                string hora18List = item.Hora18List.ToString();
                string hora19List = item.Hora19List.ToString();
                string hora20List = item.Hora20List.ToString();

                DateTime hora8_inicio = item.Hora8Inicio;
                DateTime hora9_inicio = item.Hora9Inicio;
                DateTime hora10_inicio = item.Hora10Inicio;
                DateTime hora11_inicio = item.Hora11Inicio;
                DateTime hora12_inicio = item.Hora12Inicio;
                DateTime hora13_inicio = item.Hora13Inicio;
                DateTime hora14_inicio = item.Hora14Inicio;
                DateTime hora15_inicio = item.Hora15Inicio;
                DateTime hora16_inicio = item.Hora16Inicio;
                DateTime hora17_inicio = item.Hora17Inicio;
                DateTime hora18_inicio = item.Hora18Inicio;
                DateTime hora19_inicio = item.Hora19Inicio;
                DateTime hora20_inicio = item.Hora20Inicio;

                DateTime hora8_fin = item.Hora8Fin;
                DateTime hora9_fin = item.Hora9Fin;
                DateTime hora10_fin = item.Hora10Fin;
                DateTime hora11_fin = item.Hora11Fin;
                DateTime hora12_fin = item.Hora12Fin;
                DateTime hora13_fin = item.Hora13Fin;
                DateTime hora14_fin = item.Hora14Fin;
                DateTime hora15_fin = item.Hora15Fin;
                DateTime hora16_fin = item.Hora16Fin;
                DateTime hora17_fin = item.Hora17Fin;
                DateTime hora18_fin = item.Hora18Fin;
                DateTime hora19_fin = item.Hora19Fin;
                DateTime hora20_fin = item.Hora20Fin;

                if (!string.IsNullOrEmpty(hora8List))
                {
                    foreach (var part in hora8List.Split(';'))
                    {
                        string[] str_part = part.Split('|');

                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3], // Asumiendo que tienes una propiedad 'Id'
                                title = tipoCita, // Asumiendo que tienes una propiedad 'Titulo'
                                start = hora8_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de inicio
                                end = hora8_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de fin
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3], // Asumiendo que tienes una propiedad 'Id'
                                title = tipoCita, // Asumiendo que tienes una propiedad 'Titulo'
                                start = hora8_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de inicio
                                end = hora8_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de fin
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        
                    }
                }

                if (!string.IsNullOrEmpty(hora9List))
                {
                    foreach (var part in hora9List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";

                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3], // Asumiendo que tienes una propiedad 'Id'
                                title = tipoCita, // Asumiendo que tienes una propiedad 'Titulo'
                                start = hora9_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de inicio
                                end = hora9_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de fin
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3], // Asumiendo que tienes una propiedad 'Id'
                                title = tipoCita, // Asumiendo que tienes una propiedad 'Titulo'
                                start = hora9_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de inicio
                                end = hora9_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"), // Fecha y hora de fin
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        
                    }
                }

                if (!string.IsNullOrEmpty(hora10List))
                {
                    foreach (var part in hora10List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora10_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora10_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora10_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora10_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora11List))
                {
                    foreach (var part in hora11List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora11_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora11_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora11_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora11_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora12List))
                {
                    foreach (var part in hora12List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora12_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora12_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora12_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora12_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora13List))
                {
                    foreach (var part in hora13List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora13_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora13_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora13_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora13_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora14List))
                {
                    foreach (var part in hora14List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora14_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora14_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora14_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora14_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora15List))
                {
                    foreach (var part in hora15List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora15_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora15_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora15_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora15_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora16List))
                {
                    foreach (var part in hora16List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora16_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora16_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora16_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora16_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora17List))
                {
                    foreach (var part in hora17List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora17_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora17_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora17_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora17_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora18List))
                {
                    foreach (var part in hora18List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora18_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora18_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora18_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora18_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora19List))
                {
                    foreach (var part in hora19List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora19_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora19_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora19_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora19_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }

                if (!string.IsNullOrEmpty(hora20List))
                {
                    foreach (var part in hora20List.Split(';'))
                    {
                        string[] str_part = part.Split('|');
                        string tipoCita = str_part[3].Substring(0, 1) == "P" ? "Presencial" : "Virtual";
                        if (tipoCita == "Presencial")
                        {
                            eventsPresencial.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora20_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora20_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }
                        else
                        {
                            eventsVirtual.Add(new
                            {
                                id = str_part[3],
                                title = tipoCita,
                                start = hora20_inicio.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                end = hora20_fin.ToString("yyyy-MM-ddTHH:mm:ss-05:00"),
                                extendedProps = new
                                {
                                    calendar = str_part[2],
                                    especialista = str_part[0],
                                    especialistaId = str_part[1]
                                }
                            });
                        }

                    }
                }
            }
            events.Add("Presencial", eventsPresencial);
            events.Add("Virtual", eventsVirtual);
            return events;
        }

        public async Task<IActionResult> GetCalendarioDiag2(int citaDetalleId)
        {
            GenericResponse<string> response = new GenericResponse<string>();
            UvPagoCitasSearch cita = await _pagosTriajeService.GetPagosCitasDetalle(citaDetalleId);
            int rangoId = 1;
            if (cita.Edad >= 8)
            {
                rangoId = 3;
            }
            else if (cita.Edad >= 6)
            {
                rangoId = 2;
            }
            else if (cita.Edad >= 2)
            {
                rangoId = 1;
            }
            try
            {
                DateTime dateInicio = DateTime.Now;
                //var calensarioList = await _especialistaService.GetCalendarioDisponibleDiagnosticoPortal(Convert.ToDateTime(GetFechaLunes(dateInicio)), Convert.ToInt32(cita.EvaluacionAreaId), rangoId, Convert.ToInt32(cita.EspecialistaId), Convert.ToInt32(cita.SedeId),45);
                //string calendario = getCalendario(calensarioList, dateInicio);
                response.Estado = true;
            }
            catch (Exception ex)
            {
                response.Mensaje = ex.Message;
                response.Estado = false;
                throw;
            }


            
            response.Objeto = null;

            return StatusCode(StatusCodes.Status200OK, response);
        }

        static string GetFechaLunes(DateTime date)
        {
            // Obtener el día de la semana (0 es Domingo, 6 es Sábado)
            int dayOfWeek = (int)date.DayOfWeek;

            // Si es domingo, cambiar el valor a 7 para tratarlo como el último día de la semana
            dayOfWeek = (dayOfWeek == 0) ? 7 : dayOfWeek;

            // Restar los días necesarios para retroceder al lunes de esa semana
            DateTime mondayDate = date.AddDays(-(dayOfWeek - 1));

            // Formatear la fecha en el formato YYYY-MM-DD
            string formattedDate = mondayDate.ToString("yyyy-MM-dd");

            // Devolver la fecha del lunes en el formato YYYY-MM-DD
            return formattedDate;
        }

        static string getCalendario2(List<upCalendarioDisponibleDiagV2Result> calendarioList, DateTime dateInicio)
        {
            try {
                string Link = GetLink(dateInicio);
                string mReturn = "";
                //'2018-06-01',0,0,0
                bool firstRow = true;
                foreach (var dataRow in calendarioList)
                {
                    string tdDay1Body = ""; 
                    string tdDay2Body = ""; 
                    string tdDay3Body = ""; 
                    string tdDay4Body = ""; 
                    string tdDay5Body = ""; 
                    string tdDay6Body = ""; 
                    string tdDay7Body = "";
                    string bgDia1 = "", bgDia2 = "", bgDia3 = "", bgDia4 = "", bgDia5 = "", bgDia6 = "", bgDia7 = "";
                    bool flagBg = false;
                    

                    string HorarioDiagnosticoID = dataRow.HorarioDiagnosticoID.ToString();
                    string HoraInicio = dataRow.HoraInicio.ToString();
                    string HoraFin = dataRow.HoraFin.ToString();
                    string horaCitaDesdeTime = ConvertDecimalToTime(Convert.ToSingle(dataRow.HoraInicio.ToString()));
                    string horaCitaHastaTime = ConvertDecimalToTime(Convert.ToSingle(dataRow.HoraFin.ToString()));
                    string HoraInicial = Convert.ToDateTime(dataRow.HoraInicial).ToString("HH:mm");
                    string HoraFinal = Convert.ToDateTime(dataRow.HoraFinal).ToString("HH:mm");

                    string day1 = Convert.ToDateTime(dataRow.day1).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day2 = Convert.ToDateTime(dataRow.day2).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day3 = Convert.ToDateTime(dataRow.day3).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day4 = Convert.ToDateTime(dataRow.day4).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day5 = Convert.ToDateTime(dataRow.day5).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day6 = Convert.ToDateTime(dataRow.day6).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
                    string day7 = Convert.ToDateTime(dataRow.day7).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));

                    DateTime day1D = Convert.ToDateTime(dataRow.day1);
                    DateTime day2D = Convert.ToDateTime(dataRow.day2);
                    DateTime day3D = Convert.ToDateTime(dataRow.day3);
                    DateTime day4D = Convert.ToDateTime(dataRow.day4);
                    DateTime day5D = Convert.ToDateTime(dataRow.day5);
                    DateTime day6D = Convert.ToDateTime(dataRow.day6);
                    DateTime day7D = Convert.ToDateTime(dataRow.day7);

                    string day1List = dataRow.day1List.ToString();
                    string day2List = dataRow.day2List.ToString();
                    string day3List = dataRow.day3List.ToString();
                    string day4List = dataRow.day4List.ToString();
                    string day5List = dataRow.day5List.ToString();
                    string day6List = dataRow.day6List.ToString();
                    string day7List = dataRow.day7List.ToString();

                    DateTime today = DateTime.Now.Date;

                    if (today == day1D.Date ||
                     today == day2D.Date ||
                     today == day3D.Date ||
                     today == day4D.Date ||
                     today == day5D.Date ||
                     today == day6D.Date ||
                     today == day7D.Date)
                    {
                        flagBg = true;
                    }

                    if (firstRow)
                    {
                        mReturn += "<tr><td class='text-center' colspan='8'>"+Link+"</td></tr>";
                        mReturn += "<tr>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>Horario</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day1 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day2 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day3 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day4 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day5 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day6 + "</b></td>";
                        mReturn += "<td style='background-color:#00438c; color:#fff; padding: 5px;'><b>" + day7 + "</b></td>";
                        mReturn += "</tr>";
                        firstRow = false;
                    }

                    if (!string.IsNullOrEmpty(day1List))
                {
                    string tdStr = "";
                    foreach (var item in day1List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day1D.ToString("dd/MM/yyyy");
                        if (day1D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";
                        }
                        tdDay1Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day2List))
                {
                    string tdStr = "";
                    foreach (var item in day2List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day2D.ToString("dd/MM/yyyy");
                        if (day2D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted' ><small>" + str_Item[0] + "</small></div>";

                        }
                        tdDay2Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day3List))
                {
                    string tdStr = "";
                    foreach (var item in day3List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day3D.ToString("dd/MM/yyyy");
                        if (day3D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";
                        }
                        tdDay3Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day4List))
                {
                    string tdStr = "";
                    foreach (var item in day4List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day4D.ToString("dd/MM/yyyy");
                        if (day4D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";
                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";

                    }
                    tdDay4Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day5List))
                {
                    string tdStr = "";
                    foreach (var item in day5List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day5D.ToString("dd/MM/yyyy");
                        if (day5D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";

                    }
                    tdDay5Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day6List))
                {
                    string tdStr = "";
                    foreach (var item in day6List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day6D.ToString("dd/MM/yyyy");
                        if (day6D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";

                    }
                    tdDay6Body = tdStr;
                }
                    if (!string.IsNullOrEmpty(day7List))
                {
                    string tdStr = "";
                    foreach (var item in day7List.Split(';'))
                    {
                        string[] str_Item = item.Split('|');
                        string mDatex = day7D.ToString("dd/MM/yyyy");
                        if (day7D >= DateTime.Now.AddDays(-1))
                        {
                            string addCita = string.Format("<a style='color: inherit;' href='javascript:oMd1(0,&#39;{0}&#39;,{1},{2},&#39;{3}&#39;)'>{3}</a>", mDatex, str_Item[1], HoraInicio, str_Item[0]);
                            //tdStr += "<table class='tblCal'><tr><td style='background-color:" + str_Item[2] + "'>" + addCita + "</td></tr></table>";
                            tdStr += "<div class='w-100 p-1 py-0 btn bg-warning-subtle btn-sm text-warning'><small>" + addCita + "</small></div>";

                        }
                        else
                            //tdStr += "<table class='tblCal'><tr><td class='td001' style='background-color:" + str_Item[2].Trim() + "'>" + str_Item[0] + "</td></tr></table>";
                            tdStr += "<div class='td001 w-100 p-1 py-0 btn-light opacity-50 text-center btn-sm text-muted'><small>" + str_Item[0] + "</small></div>";

                    }
                    tdDay7Body = tdStr;
                }

                    if (flagBg)
                    {
                        DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

                        // Usar un switch para imprimir el nombre del día de la semana
                        switch (dayOfWeek)
                        {
                            case DayOfWeek.Sunday:
                                bgDia7 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Domingo.");
                                break;
                            case DayOfWeek.Monday:
                                bgDia1 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Lunes.");
                                break;
                            case DayOfWeek.Tuesday:
                                bgDia2 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Martes.");
                                break;
                            case DayOfWeek.Wednesday:
                                bgDia3 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Miércoles.");
                                break;
                            case DayOfWeek.Thursday:
                                bgDia4 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Jueves.");
                                break;
                            case DayOfWeek.Friday:
                                bgDia5 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Viernes.");
                                break;
                            case DayOfWeek.Saturday:
                                bgDia6 = "#f2f3f5";
                                //Console.WriteLine("Hoy es Sábado.");
                                break;
                        }
                    }


                    mReturn += "<tr>";
                    mReturn += "<td class='tdBgGF2 bg-primary-subtle' style='padding:0px; font-size: 10px;'>" + HoraInicial + " -" + HoraFinal + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia1 + ";'>" + tdDay1Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia2 + ";'>" + tdDay2Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia3 + ";'>" + tdDay3Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia4 + ";'>" + tdDay4Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia5 + ";'>" + tdDay5Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia6 + ";'>" + tdDay6Body + "</td>";
                    mReturn += "<td class='tdBgGF2' style='padding:0px;background-color:" + bgDia7 + ";'>" + tdDay7Body + "</td>";
                    mReturn += "</tr>";

                }
                return mReturn;
            }
            catch {
                return "<tr><td collspan='8'>No hay datos para mostrar</td></tr>"; ;
            }
        }

        static string GetLink(DateTime dateInicio)
        {
            DateTime dateLunes = dateInicio;
            DateTime nextLunes = dateLunes.AddDays(7);
            DateTime prevLunes = dateLunes.AddDays(-7);

            string semanaDesde = dateLunes.ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
            string semanaHasta = dateLunes.AddDays(6).ToString("ddd d MMM", new System.Globalization.CultureInfo("es"));
            semanaDesde = UppercaseWords(semanaDesde); //char.ToUpper(semanaDesde[0]) + semanaDesde.Substring(1);
            semanaHasta = UppercaseWords(semanaHasta); //char.ToUpper(semanaHasta[0]) + semanaHasta.Substring(1);
            //string link = "<a style='text-decoration:underline;' href='javascript:goToCalendar(&#39;" + prevLunes.ToString("yyyy-MM-dd") + "&#39;)'>&lt;&lt;-</a> " + "Semana del " + semanaDesde + " al " + semanaHasta + " <a style='text-decoration:underline;' href='javascript:goToCalendar(&#39;" + nextLunes.ToString("yyyy-MM-dd") + "&#39;)'>-&gt;&gt;</a>";
            string link = string.Format("<div class='input-group w-50 mb-3 m-auto'> <a class='btn bg-primary-subtle text-primary  rounded-start d-flex align-items-center' type='button' href=\"javascript:RefreshCalendar('{0}')\" > <i class='ti ti-arrow-left fs-4'></i> Atras </a>", prevLunes.ToString("yyyy-MM-dd"));
            link += string.Format("<div type='text' class='form-control' aria-describedby='basic-addon1' >{0}</div>", "Semana del " + semanaDesde + " al " + semanaHasta);
            link += string.Format("<a class='btn bg-primary-subtle text-primary  rounded-end d-flex align-items-center' type='button' href=\"javascript:RefreshCalendar('{0}')\" >Siguienye <i class='ti ti-arrow-right fs-4'></i></a> </div>", nextLunes.ToString("yyyy-MM-dd"));
            return link;
        }



        static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        #endregion




    }
}
