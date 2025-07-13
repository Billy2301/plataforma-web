using BL.IServices;
using BL.Services;
using culqi.net;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using PortalClienteV2.Utilities.Response;
using RestSharp;
using System;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace PortalClienteV2.Controllers
{
    public class PagoExternoController : BaseController
    {
        Security security = null;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioService _usuarioService;
        private readonly IPagoService _pagoService;
        private readonly IPagoExternoService _pagoExternoService;
        private readonly IWebHostEnvironment _environment;
        private readonly string? imagesDirectory;
        private readonly IPacienteService _pacienteService;
        private readonly IPagoExternoVoucherService _pagoVoucherService;

        public PagoExternoController(IHttpContextAccessor accesor, IConfiguration configuration, UserManager<IdentityUser> userManager, IUsuarioService usuarioService, IPagoService pagoService, IPagoExternoService pagoExternoService, IWebHostEnvironment environment, ILogService logService, IPacienteService pacienteService, IPagoExternoVoucherService pagoVoucherService) : base(accesor, configuration, logService)
        {
            security = securityKeys();
            _userManager = userManager;
            _usuarioService = usuarioService;
            _pagoService = pagoService;
            _pagoExternoService = pagoExternoService;
            _environment = environment;
            imagesDirectory = CurrentConfig().pathFile;
            _pacienteService = pacienteService;
            _pagoVoucherService = pagoVoucherService;
        }

        public IActionResult Index()
        {
            return View();
        }

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
                string? order_number = "port_clie_" +  Helper.GenerateShortGuid() + DateTimeOffset.Now.ToUnixTimeSeconds();
                int totaPagar = modelo.GetProperty("customer.amount").GetInt32();
                string? Area = modelo.GetProperty("customer.area").GetString();
                string? Currency = modelo.GetProperty("customer.currency").GetString();
                string? Description = modelo.GetProperty("customer.description").GetString();
                string? Direccionpago = modelo.GetProperty("customer.direccionpago").GetString();
                string? Email = modelo.GetProperty("customer.email").GetString();
                string? Hc = modelo.GetProperty("customer.hc").GetString();
                string? Razonsocial = modelo.GetProperty("customer.razonsocial").GetString();
                string? Ruc = modelo.GetProperty("customer.ruc").GetString();
                string? Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString();
                int TotalPagarReal = modelo.GetProperty("customer.totalpagar").GetInt32();
                bool conDni = modelo.GetProperty("customer.conDni").GetBoolean();

                string first_name = user.Nombres == null ? "Cliente" : user.Nombres;
                string last_name = user.ApellidoPaterno == null ? "Cliente" : user.ApellidoPaterno + " " + user.ApellidoMaterno;
                string email = appUsuario.UserName + "";
                string phone_number = user.Telefono == null ? "999999999" : user.Telefono + "";
                if (conDni)
                {
                    first_name = Razonsocial == null ? "Cliente" : Razonsocial;
                }

                DateTimeOffset fechaActual = DateTimeOffset.Now;
                DateTimeOffset fechaMasUnDia = fechaActual.AddDays(1);
                long epoch = fechaMasUnDia.ToUnixTimeSeconds();

                Dictionary<string, object> client_details = new Dictionary<string, object>
                {
                    {"first_name", first_name},
                    {"last_name", last_name},
                    {"email", email},
                    {"phone_number", phone_number}
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
        public async Task<IActionResult> CrearCargo([FromBody] JsonElement modelo)
        {
            GenericResponse<ResponseCulqi> response = new GenericResponse<ResponseCulqi>();
            try
            {

                if (modelo.ValueKind == JsonValueKind.Null || (modelo.ValueKind == JsonValueKind.Object && !modelo.EnumerateObject().Any()))
                {
                    response.Estado = false;
                    response.Mensaje = "Error al guardar el pago";

                    return StatusCode(StatusCodes.Status200OK, response);
                }
                Cargo mCargo = new Cargo()
                {
                    TokenId = modelo.GetProperty("tokenId").GetString(),
                    Browser = modelo.GetProperty("browser").GetString(),
                    Card = modelo.GetProperty("card").GetString(),
                    Cardbrand = modelo.GetProperty("cardbrand").GetString(),
                    Cardtype = modelo.GetProperty("cardtype").GetString(),
                    Devicetype = modelo.GetProperty("devicetype").GetString(),
                    Email = modelo.GetProperty("email").GetString(),
                    Installments = modelo.GetProperty("installments").GetInt32(),
                    Ip = modelo.GetProperty("ip").GetString(),
                    Customer = new Cliente()
                    {
                        Amount = modelo.GetProperty("customer.amount").GetInt32(),
                        Area = modelo.GetProperty("customer.area").GetString(),
                        Currency = modelo.GetProperty("customer.currency").GetString(),
                        Description = modelo.GetProperty("customer.description").GetString(),
                        Direccionpago = modelo.GetProperty("customer.direccionpago").GetString(),
                        Email = modelo.GetProperty("customer.email").GetString(),
                        Hc = modelo.GetProperty("customer.hc").GetString(),
                        Pagoid = modelo.GetProperty("customer.pagoid").GetInt32(),
                        Razonsocial = modelo.GetProperty("customer.razonsocial").GetString(),
                        Ruc = modelo.GetProperty("customer.ruc").GetString(),
                        Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString(),
                        TotalPagar = modelo.GetProperty("customer.totalpagar").GetInt32(),
                    }
                };

                var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                PerfilUsuario? user = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);

                PagosExterno entityPagoExt = new PagosExterno();
                Pago entityPago;

                int sedeID = 0;
                entityPago = await _pagoService.getPago(Convert.ToInt32(mCargo.Customer.Pagoid));
                if (entityPago != null) { sedeID = entityPago.SedeId; }
                entityPagoExt.RefPagoId = Convert.ToInt32(mCargo.Customer.Pagoid);
                entityPagoExt.AreaNombre = mCargo.Customer.Area;
                entityPagoExt.PagoExternoEstado = "PENDIENTE";
                entityPagoExt.PagoExternoMonto = 0;
                if (!string.IsNullOrEmpty(mCargo.Customer.Amount.ToString()))
                {
                    if (Helper.IsNumeric(mCargo.Customer.TotalPagar.ToString())) { entityPagoExt.PagoExternoMonto = Convert.ToDecimal(mCargo.Customer.TotalPagar); }
                }
                if (!string.IsNullOrEmpty(mCargo.Customer.Hc))
                {
                    entityPagoExt.PagoExternoBeneficiario = Convert.ToInt32(mCargo.Customer.Hc);
                }
                entityPagoExt.CreadoPor = appUsuario.UserName;
                entityPagoExt.CreadoFecha = DateTime.Now;
                entityPagoExt.MonedaAbrev = "S/.";
                entityPagoExt.MonedaCodigo = "PEN"; ;
                entityPagoExt.TransactionCode = "";
                entityPagoExt.TransactionDesc = mCargo.Customer.Description;
                entityPagoExt.TokenId = mCargo.TokenId;
                entityPagoExt.TokenEmail = mCargo.Email;
                entityPagoExt.TokenCardNumber = mCargo.Card;
                entityPagoExt.TokenClientIp = mCargo.Ip;
                entityPagoExt.TokenClientBrowser = mCargo.Browser;
                entityPagoExt.TokenClientDeviceType = mCargo.Devicetype;
                entityPagoExt.TokenInstallments = mCargo.Installments.ToString();
                entityPagoExt.TokenCardBrand = mCargo.Cardbrand;
                entityPagoExt.TokenCardType = mCargo.Cardtype;
                entityPagoExt.SedeId = sedeID;
                entityPagoExt.DocuTipo = mCargo.Customer.Tipodocpago;
                if (!string.IsNullOrEmpty(mCargo.Customer.Razonsocial)) { entityPagoExt.DocuRazonSocial = mCargo.Customer.Razonsocial; } else { entityPagoExt.DocuRazonSocial = null; }
                if (!string.IsNullOrEmpty(mCargo.Customer.Ruc)) { entityPagoExt.DocuRuc = mCargo.Customer.Ruc; } else { entityPagoExt.DocuRuc = null; }
                if (!string.IsNullOrEmpty(mCargo.Customer.Direccionpago)) { entityPagoExt.DocuDireccion = mCargo.Customer.Direccionpago; } else { entityPagoExt.DocuDireccion = null; }
                entityPagoExt.ActualizadoPor = appUsuario.UserName;
                entityPagoExt.ActualizadoFecha = DateTime.Now;
                await _pagoExternoService.Guardar(entityPagoExt);
                int mNew_ID = entityPagoExt.PagoExternoId;

                //Culqi Cargo
                decimal pagoExternoMonto = 0;
                if (mCargo.Customer.Amount > 0) { pagoExternoMonto = Convert.ToDecimal(mCargo.Customer.Amount); }
                string email_cliente = "";
                string description_ = "";
                Int32 installments_ = 0;

                if (!string.IsNullOrEmpty(entityPagoExt.TokenEmail)) { email_cliente = entityPagoExt.TokenEmail; }
                if (!string.IsNullOrEmpty(entityPagoExt.TransactionDesc)) { if (entityPagoExt.TransactionDesc.Length > 79) { description_ = entityPagoExt.TransactionDesc.Substring(0, 79); } else { description_ = entityPagoExt.TransactionDesc; } } else { description_ = "CPAL Pago"; }
                if (!string.IsNullOrEmpty(entityPagoExt.TokenInstallments)) { installments_ = Convert.ToInt32(entityPagoExt.TokenInstallments); }

                Dictionary<string, object> map = new Dictionary<string, object>
                {
                  {"amount", pagoExternoMonto.ToString()},
                  {"capture", true},
                  {"currency_code", "PEN"},
                  {"email", email_cliente},
                  {"description", description_},
                  {"installments", installments_},
                  {"source_id", entityPagoExt.TokenId.ToString()}
                };
                ResponseCulqi json_object = new Charge(security).Create(map);

                //return json_object;
                dynamic data = JObject.Parse(json_object.body);
                string id_object = data["object"];
                string id_charge = data["id"];
                string reference_code = data["reference_code"];
                bool isError = false;
                string error_desc = "";
                string responseValue = mNew_ID.ToString();
                int nivelError = 1;

                //save proceso pago  
                if (!string.IsNullOrEmpty(id_object))
                {
                    if (id_object.ToLower() == "error")
                    {
                        isError = true;
                        error_desc = "Error: " + data["user_message"];
                        responseValue = "0";
                        nivelError = 2;
                    }
                }
                else
                {
                    isError = true;
                    error_desc = "Error: Error en procesar el pago.";
                    responseValue = "0";
                    nivelError = 2;
                }

                if (!string.IsNullOrEmpty(data.ToString())) entityPagoExt.TransactionResponse = data.ToString(); else entityPagoExt.TransactionResponse = "VACIO";
                entityPagoExt.ActualizadoPor = "SYSTEM";
                entityPagoExt.TransactionCode = id_charge;
                entityPagoExt.ReferenceCode = reference_code;
                entityPagoExt.PagoExternoEstado = (isError) ? "ERROR" : "PAGADO";
                entityPagoExt.ActualizadoFecha = DateTime.Now;
                await _pagoExternoService.Guardar(entityPagoExt);

                // Enviar Correo confirmacion     
                //******************************************
                if (entityPagoExt.PagoExternoEstado == "PAGADO")
                {
                    //actualiza referencia
                    await _pagoExternoService.updatePagosDetalleReferencia(entityPagoExt.RefPagoId, "CULQI Nro ref: " + entityPagoExt.ReferenceCode);
                    
                    ////envia email
                    var path = Path.Combine(_environment.WebRootPath, "Templates", "TemplatePagoOK.html");
                    string templateContent = System.IO.File.ReadAllText(path);
                    await _pagoExternoService.NotificacionPago(entityPagoExt, templateContent, appUsuario.UserName);
                }
                //end

                await SaveLog(nivelError, "PagoExterno", true, error_desc);

                response.Estado = true;
                response.Objeto = json_object;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "PagoExterno", false, ex.Message);
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
                Orden mOrden = new Orden()
                {
                    OrderId = modelo.GetProperty("orderId").GetString(),
                    OrderNumber = modelo.GetProperty("orderNumber").GetString(),
                    PaymenCode = modelo.GetProperty("paymenCode").GetString(),
                    Description = modelo.GetProperty("description").GetString(),
                    Object = modelo.GetProperty("object").GetString(),
                    Amount = modelo.GetProperty("amount").GetDecimal(),
                    Customer = new Cliente()
                    {
                        Amount = modelo.GetProperty("customer.amount").GetInt32(),
                        Area = modelo.GetProperty("customer.area").GetString(),
                        Currency = modelo.GetProperty("customer.currency").GetString(),
                        Description = modelo.GetProperty("customer.description").GetString(),
                        Direccionpago = modelo.GetProperty("customer.direccionpago").GetString(),
                        Email = modelo.GetProperty("customer.email").GetString(),
                        Hc = modelo.GetProperty("customer.hc").GetString(),
                        Pagoid = modelo.GetProperty("customer.pagoid").GetInt32(),
                        Razonsocial = modelo.GetProperty("customer.razonsocial").GetString(),
                        Ruc = modelo.GetProperty("customer.ruc").GetString(),
                        Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString(),
                        TotalPagar = modelo.GetProperty("customer.totalpagar").GetInt32(),
                    }
                };

                security = securityKeys();

                //save proceso pago
                PagosExterno entityPagoExt = new PagosExterno();
                Pago entityPago;
                int mSedeID = 0;

                entityPago = await _pagoService.getPago(Convert.ToInt32(mOrden.Customer.Pagoid));

                if (entityPago != null) { mSedeID = entityPago.SedeId; }

                entityPagoExt.RefPagoId = Convert.ToInt32(mOrden.Customer.Pagoid);
                entityPagoExt.AreaNombre = mOrden.Customer.Area;
                entityPagoExt.PagoExternoEstado = "PENDIENTE";
                entityPagoExt.PagoExternoMonto = 0;
                if (!string.IsNullOrEmpty(mOrden.Customer.Amount.ToString()))
                {
                    if (Helper.IsNumeric(mOrden.Customer.TotalPagar.ToString())) { entityPagoExt.PagoExternoMonto = Convert.ToDecimal(mOrden.Customer.TotalPagar); }
                }
                if (!string.IsNullOrEmpty(mOrden.Customer.Hc))
                {
                    entityPagoExt.PagoExternoBeneficiario = Convert.ToInt32(mOrden.Customer.Hc);
                }
                entityPagoExt.CreadoPor = User.Identity.Name;
                entityPagoExt.TokenEmail = User.Identity.Name;
                entityPagoExt.CreadoFecha = DateTime.Now;
                entityPagoExt.MonedaAbrev = "S/.";
                entityPagoExt.MonedaCodigo = "PEN"; ;
                entityPagoExt.TransactionCode = mOrden.OrderId;
                entityPagoExt.TokenId = mOrden.OrderNumber;
                entityPagoExt.TransactionResponse = mOrden.Object;
                entityPagoExt.ReferenceCode = mOrden.PaymenCode;
                entityPagoExt.TransactionDesc = mOrden.Description;

                entityPagoExt.DocuTipo = mOrden.Customer.Tipodocpago;
                if (!string.IsNullOrEmpty(mOrden.Customer.Razonsocial)) { entityPagoExt.DocuRazonSocial = mOrden.Customer.Razonsocial; } else { entityPagoExt.DocuRazonSocial = null; }
                if (!string.IsNullOrEmpty(mOrden.Customer.Ruc)) { entityPagoExt.DocuRuc = mOrden.Customer.Ruc; } else { entityPagoExt.DocuRuc = null; }
                if (!string.IsNullOrEmpty(mOrden.Customer.Direccionpago)) { entityPagoExt.DocuDireccion = mOrden.Customer.Direccionpago; } else { entityPagoExt.DocuDireccion = null; }

                entityPagoExt.SedeId = mSedeID;

                await _pagoExternoService.Guardar(entityPagoExt);
                int mNew_ID = entityPagoExt.PagoExternoId;

                ResponseCulqi json_object = new ResponseCulqi()
                {
                    statusCode = 200,
                    body = JsonConvert.SerializeObject(entityPagoExt)
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

                await SaveLog(1, "PagoExterno", true, "");

                response.Estado = true;
                response.Objeto = json_object;
                response.Mensaje =string.Format("Puede realizar su pago en cualquier agente bancario utilizando el código CIP <br> <h2><b>{0}<b></h2> o Scanear el código QR desde tu billetera móvil <br><img src='{1}' style='max-width:250px;' />", paymentCode, qr);
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "PagoExterno", false, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] JsonElement modelo)
        {
            GenericResponse<ResponseCulqi> response = new GenericResponse<ResponseCulqi>();

            try
            {


                var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                PerfilUsuario? user = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);

                Int32 amount = modelo.GetProperty("amount").GetInt32();
                string? currency_code = modelo.GetProperty("currency").GetString();
                string? description = modelo.GetProperty("description").GetString();
                string? order_number = Helper.GenerateShortGuid();
                int totaPagar = modelo.GetProperty("customer.amount").GetInt32();
                string? Area = modelo.GetProperty("customer.area").GetString();
                string? Currency = modelo.GetProperty("customer.currency").GetString();
                string? Description = modelo.GetProperty("customer.description").GetString();
                string? Direccionpago = modelo.GetProperty("customer.direccionpago").GetString();
                string? Email = modelo.GetProperty("customer.email").GetString();
                string? Hc = modelo.GetProperty("customer.hc").GetString();
                string? Razonsocial = modelo.GetProperty("customer.razonsocial").GetString();
                string? Ruc = modelo.GetProperty("customer.ruc").GetString();
                string? Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString();
                int TotalPagarReal = modelo.GetProperty("customer.totalpagar").GetInt32();
                bool conDni = modelo.GetProperty("customer.conDni").GetBoolean();


                DateTimeOffset fechaActual = DateTimeOffset.Now;
                DateTimeOffset fechaMasUnDia = fechaActual.AddDays(1);
                long expiration_date = fechaMasUnDia.ToUnixTimeSeconds();

                string json = string.Format("{{\"amount\":{0},\"currency_code\":\"{1}\",\"description\":\"{2}\",\"order_number\":\"{3}\",\"expiration_date\":\"{4}\",\"client_details\":{{\"first_name\":\"{5}\",\"last_name\":\"{6}\",\"email\":\"{7}\",\"phone_number\":\"{8}\"}},\"confirm\":false,\"metadata\":{{\"dni\":\"{9}\"}}}}", amount, currency_code, description, order_number, expiration_date, user.Nombres , user.ApellidoPaterno, User.Identity.Name, user.Telefono, user.Dni);


                var client = new RestClient("https://api.culqi.com/v2/orders");
                var request = new RestRequest(Method.Post.ToString());
                request.AddHeader("Authorization", "Bearer " + security.secret_key); 
                request.AddHeader("content-type", "application/json");

                request.AddParameter("application/json", json, ParameterType.RequestBody);
                RestResponse restResponse = client.Execute(request);

                string content = restResponse.Content;


                JsonTextReader reader = new JsonTextReader(new StringReader(content));

                ResponseCulqi responseCulqi = new ResponseCulqi();
                responseCulqi.statusCode = 200;
                responseCulqi.body = reader.ToString(); ;

                response.Estado = true;
                response.Objeto = responseCulqi;

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
        public async Task<IActionResult> GuardarVoucher([FromBody] JsonElement modelo)
        {
            GenericResponse<ResponseVoucher> response = new GenericResponse<ResponseVoucher>();

            try
            {
                if (modelo.ValueKind == JsonValueKind.Null || (modelo.ValueKind == JsonValueKind.Object && !modelo.EnumerateObject().Any()))
                {
                    response.Estado = false;
                    response.Mensaje = "Error al guardar el Voucher";
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                Voucher mVoucher = new Voucher();

                mVoucher.Banco = modelo.GetProperty("banco").GetString();
                mVoucher.FechaPago = Convert.ToDateTime(modelo.GetProperty("fechaPago").GetString());
                mVoucher.NroOperacion = modelo.GetProperty("nroOperacion").GetString();
                mVoucher.Agencia = modelo.GetProperty("agencia").GetString();
                mVoucher.Moneda = modelo.GetProperty("moneda").GetString();
                mVoucher.NombreImagen = modelo.GetProperty("nombreImagen").GetString();
                var FileExternal = modelo.GetProperty("fileExternal").GetString();
                var FileInternal = modelo.GetProperty("fileInternal").GetString();
                var FileExtension = modelo.GetProperty("fileExtension").GetString();
                mVoucher.FileExternal = FileExternal;
                mVoucher.FileInternal = string.Concat(FileInternal, FileExtension);
                mVoucher.FileExtension = FileExtension;
                mVoucher.Customer = new Cliente();

                mVoucher.Customer.Amount = modelo.GetProperty("customer.amount").GetInt32();
                mVoucher.Customer.Area = modelo.GetProperty("customer.area").GetString();
                mVoucher.Customer.Currency = modelo.GetProperty("customer.currency").GetString();
                mVoucher.Customer.Description = modelo.GetProperty("customer.description").GetString();
                mVoucher.Customer.Direccionpago = modelo.GetProperty("customer.direccionpago").GetString();
                mVoucher.Customer.Email = modelo.GetProperty("customer.email").GetString();
                mVoucher.Customer.Hc = modelo.GetProperty("customer.hc").GetString();
                mVoucher.Customer.Pagoid = modelo.GetProperty("customer.pagoid").GetInt32();
                mVoucher.Customer.Razonsocial = modelo.GetProperty("customer.razonsocial").GetString();
                mVoucher.Customer.Ruc = modelo.GetProperty("customer.ruc").GetString();
                mVoucher.Customer.Tipodocpago = modelo.GetProperty("customer.tipodocpago").GetString();
                mVoucher.Customer.TotalPagar = modelo.GetProperty("customer.totalpagar").GetInt32();
                mVoucher.Customer.ConDni = modelo.GetProperty("customer.conDni").GetBoolean();


                //save proceso pago      

                PagoExternoVoucher entity = new PagoExternoVoucher();
                Pago entityPago;
                Paciente? entityPaiente;
                int mSedeID = 0;
                decimal monto = 0;
                int HC = 0;
                bool? conDNI = mVoucher.Customer.ConDni == null ? false : mVoucher.Customer.ConDni;

                entityPaiente = await _pacienteService.GetPaciente(Convert.ToInt32(mVoucher.Customer.Hc));
                entityPago = await _pagoService.getPago(Convert.ToInt32(mVoucher.Customer.Pagoid));
                if (entityPago != null) { mSedeID = entityPago.SedeId; }
                if (mVoucher.Customer.TotalPagar.HasValue) { monto = Convert.ToDecimal(mVoucher.Customer.TotalPagar); }
                if (!string.IsNullOrEmpty(mVoucher.Customer.Hc)) { HC = Convert.ToInt32(mVoucher.Customer.Hc); }

                entity.RefPagoId = Convert.ToInt32(mVoucher.Customer.Pagoid);
                entity.AreaNombre = mVoucher.Customer.Area;
                entity.PagoHc = HC;
                entity.CreadoPor = User?.Identity?.Name;
                entity.CreadoFecha = DateTime.Now;
                entity.SedeId = mSedeID;
                entity.NombreCompleto = entityPaiente?.Nombres + " " + entityPaiente?.ApellidoPaterno + " " + entityPaiente?.ApellidoMaterno;
                entity.Email = entityPaiente?.EmailContacto;
                entity.PagoDescripcion = mVoucher.Customer.Description;
                //entity.refFacturaID = null;
                entity.FileInternal = mVoucher.FileInternal;
                entity.FileExternal = mVoucher.FileExternal;
                entity.FileExtension = mVoucher.FileExtension;
                entity.PagoBanco = mVoucher.Banco;
                entity.PagoTipoDoc = mVoucher.Customer.Tipodocpago;
                entity.PagoDocNumero = mVoucher.Customer.Ruc;
                entity.PagoRazonSocial = mVoucher.Customer.Razonsocial;
                entity.PagoDireccion = mVoucher.Customer.Direccionpago;
                if (entity.PagoTipoDoc == "BOLETA" && conDNI == false)
                {
                    entity.PagoDocNumero = "Sin DNI";
                    entity.PagoRazonSocial = "Varios";
                }
                entity.FechaPago = Convert.ToDateTime(mVoucher.FechaPago); // CAMBIAR
                entity.NroOperacion = mVoucher.NroOperacion; ;
                entity.Agencia = mVoucher.Agencia;
                entity.PagoTipo = "VOUCHER";
                entity.PagoMonto = monto;
                entity.PagoMoneda = mVoucher.Moneda;
                entity.PagoMonedaAbbrev = mVoucher.Moneda == "PEN" ? "S/." : "$";
                entity.PagoVoucherEstado = "PENDIENTE";

                entity = await _pagoVoucherService.Guardar(entity);
                int pagoVoucherID = entity.PagoExternoVoucherId;
                //actualiza referencia 
                await _pagoVoucherService.updatePagosDetalleReferencia(entity.RefPagoId, "Voucher Op. " + entity.NroOperacion + " Fecha " + Convert.ToDateTime(entity.FechaPago).ToString("dd/MM/yyyy"));
                await _pagoVoucherService.updateFechaPagobyId(entity.RefPagoId, Convert.ToDateTime(entity.FechaPago).ToString("yyyy-MM-d HH:mm:ss"));
                await SaveLog(1, "PagoExternoVoucher", true, "");

                ResponseVoucher json_object = new ResponseVoucher()
                {
                    statusCode = 200,
                    body = JsonConvert.SerializeObject(entity)
                };

                response.Estado = true;
                response.Objeto = json_object;
                response.Mensaje = "Voucher registrado";
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "PagoExternoVoucher", false, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> CargarImagen(List<IFormFile> file)
        {
            GenericResponse<ImagenViewModel> response = new GenericResponse<ImagenViewModel>();

            try
            {
                if (file == null)
                {
                    response.Estado = false;
                    response.Mensaje = "Se ha producido un error al cargar el archivo";
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                string External = Path.GetFileName(file[0].FileName);
                string Internal = Guid.NewGuid().ToString("N");
                string Extension = Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine(imagesDirectory, string.Concat(Internal, Extension));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file[0].CopyToAsync(stream);
                }

                ImagenViewModel model = new ImagenViewModel()
                {
                    Internal = Internal,
                    External = External,
                    Extension = Extension,
                };

                response.Objeto = model;
                response.Estado = true;
                response.Mensaje = "El archivo se ha cargado correctamente";

                await SaveLog(1, "ImagenViewModel", true, "");


                return StatusCode(StatusCodes.Status200OK, response);


            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                await SaveLog(3, "ImagenViewModel", false, ex.Message);
                return StatusCode(StatusCodes.Status200OK, response.Mensaje + "path: " + imagesDirectory);
            }
        }
    }
}
