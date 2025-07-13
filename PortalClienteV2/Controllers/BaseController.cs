using BL.IServices;
using BL.Services;
using culqi.net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models.Model;

namespace PortalClienteV2.Controllers
{
    public abstract class BaseController : Controller
    {
        Security security = null;
        Agente? userAgent = null;
        Configuracion? config = null;

        private readonly IHttpContextAccessor _accesor;
        public readonly IConfiguration _configuration;
        public readonly ILogService _logService;

        protected BaseController(IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService)
        {
            _accesor = accesor;
            _configuration = configuration;
            _logService = logService;

        }

        public Agente userAjent()
        {
            userAgent = new Agente();
            userAgent.rootApp = _accesor.HttpContext.Request.Host.Value;
            userAgent.Url = _accesor.HttpContext.Request.Path + _accesor.HttpContext.Request.QueryString;
            userAgent.isMobil = _accesor.HttpContext.Request.Headers["User-Agent"].ToString().Contains("Mobi", StringComparison.OrdinalIgnoreCase);
            userAgent.ipAddress = _accesor.HttpContext.Connection.RemoteIpAddress.ToString() != null ? _accesor.HttpContext.Connection.RemoteIpAddress.ToString() : "0.0.0.0";
            userAgent.userAgentString = _accesor.HttpContext.Request.Headers["User-Agent"].ToString();
            userAgent.scheme = _accesor.HttpContext.Request.Scheme;
            userAgent.host = _accesor.HttpContext.Request.Host;
            userAgent.Controller = _accesor.HttpContext.Request.Path;
            userAgent.metod = _accesor.HttpContext.Request.Method;
            userAgent.UserName = _accesor.HttpContext.User.Identity.Name;
            userAgent.UserId = "noting";
            return userAgent;
        }

        public Configuracion CurrentConfig()
        {
            config = new Configuracion();
            config.pathFile = _configuration.GetValue<string>("path_files");
            config.getDiasCalendario = _configuration.GetValue<string>("dias_calendario");
            config.getTiempoEsperaEnSeg = _configuration.GetValue<string>("tiempo_espera_seg");
            config.getAppName = _configuration.GetValue<string>("appName");
            config.getIsProduction = _configuration.GetValue<string>("is_production");
            return config;
        }

        public Security securityKeys()
        {

            security = new Security();
            security.public_key = _configuration.GetValue<string>("public_key");
            security.secret_key = _configuration.GetValue<string>("secret_key");
            security.rsa_id = _configuration.GetValue<string>("rsa_id");
            security.rsa_key = _configuration.GetValue<string>("rsa_key");
            return security;
        }

        public Security securityKeysApertura()
        {

            security = new Security();
            security.public_key = _configuration.GetValue<string>("publicApertura_key");
            security.secret_key = _configuration.GetValue<string>("secretApertura_key");
            security.rsa_id = _configuration.GetValue<string>("rsa_id");
            security.rsa_key = _configuration.GetValue<string>("rsa_key");
            return security;
        }

        public void GetSlider()
        {
            var context = _accesor.HttpContext;
            var ImgSlider1 = _configuration.GetValue<string>("slider1");
            var ImgSlider2 = _configuration.GetValue<string>("slider2");
            var ImgSlider3 = _configuration.GetValue<string>("slider3");
            if (context != null)
            {
                context.Session.SetString("ImgSlider1", ImgSlider1);
                context.Session.SetString("ImgSlider2", ImgSlider2);
                context.Session.SetString("ImgSlider3", ImgSlider3);
            }
        }

        /// <summary>
        /// Guarda un registro en el log del sistema.
        /// </summary>
        /// <param name="nivel">El nivel del log: 1 para "Info", 2 para "Warning", 3 para "Error", 4 para "Crítico".</param>
        /// <param name="clase">El nombre de la clase donde se está generando el log.</param>
        /// <param name="informacion">Indica si la operación fue completada con éxito (true) o no (false).</param>
        /// <param name="mensaje">El mensaje del log que se desea guardar.</param>
        /// <returns>Una tarea que representa la operación asíncrona de guardar el log.</returns>
        /// <remarks>
        /// Este método construye el mensaje de log utilizando información del contexto HTTP actual, como la URL, 
        /// el método de solicitud, el agente de usuario, el nombre de usuario y la dirección IP.
        /// </remarks>
        public async Task SaveLog(int nivel, string clase, bool informacion, string mensaje)
        {
            string _nivel = "";
            string _userAgent = "";
            switch (nivel)
            {
                case 1:
                    _nivel = "Info";
                    break;
                case 2:
                    _nivel = "Warning";
                    break;
                case 3:
                    _nivel = "Error";
                    break;
                case 4:
                    _nivel = "Critico";
                    break;
                default:
                    break;
            }

            var url = _accesor.HttpContext?.Request.Path + _accesor.HttpContext?.Request.QueryString != null ? _accesor.HttpContext.Request.Path + _accesor.HttpContext.Request.QueryString : "";
            var method = _accesor.HttpContext?.Request.Method != null ? _accesor.HttpContext.Request.Method : "";
            var userName = _accesor.HttpContext?.User?.Identity?.Name != null ? _accesor.HttpContext.User.Identity.Name?.ToString() : "";
            var ip = _accesor.HttpContext?.Connection.RemoteIpAddress != null ? _accesor.HttpContext.Connection.RemoteIpAddress.ToString() : "0.0.0.0";
            var isMobil = _accesor?.HttpContext?.Request.Headers["User-Agent"].ToString().Contains("Mobi", StringComparison.OrdinalIgnoreCase);
            var operacion = informacion == true ? "Operación completada" : "Operación incompleta";
            var userAgent = !string.IsNullOrEmpty(_accesor.HttpContext.Request.Headers["User-Agent"].ToString()) ? _accesor.HttpContext.Request.Headers["User-Agent"].ToString() : "";
            _userAgent = "PC - " + userAgent;
            if (Convert.ToBoolean(isMobil)) { _userAgent = "Mobil - " + userAgent; }

            await _logService.SaveLog(_nivel, url, clase, method, _userAgent, "", userName, ip, operacion, mensaje);

        }
    }
}
