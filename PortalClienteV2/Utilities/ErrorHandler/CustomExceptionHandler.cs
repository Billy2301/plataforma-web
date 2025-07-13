using BL.IServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace PortalClienteV2.Utilities.ErrorHandler
{
    public class CustomExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<CustomExceptionHandler> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IWebHostEnvironment _environment;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger, IServiceScopeFactory serviceScopeFactory, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            _environment = environment;
        }
        /// <summary>
        /// Intenta manejar una excepción asincrónicamente y envía un correo electrónico de notificación.
        /// </summary>
        /// <param name="httpContext">El contexto HTTP asociado con la solicitud que provocó la excepción.</param>
        /// <param name="exception">La excepción que se produjo y debe ser manejada.</param>
        /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asincrónica.</param>
        /// <returns>
        /// <c>true</c> si la excepción ha sido manejada completamente y no se requiere ninguna acción adicional;
        /// <c>false</c> si la excepción no ha sido manejada completamente y debe seguir el flujo predeterminado de manejo de excepciones de ASP.NET Core.
        /// </returns>
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                // Registra el mensaje de la excepción y la hora de ocurrencia
                var exceptionMessage = exception.Message;
                var InnerException = "No inner exception was generated";
                if (exception.InnerException != null) { InnerException = exception.InnerException.Message; }
                logger.LogError(
                    "Error Message: {exceptionMessage}, Time of occurrence: {time}",
                    exceptionMessage, DateTime.UtcNow);

                // Captura los detalles de la excepción para el cuerpo del correo electrónico
                var exceptionDetails = GetExceptionDetails(exception);
                // Captura la información del HttpContext
                var httpContextInfo = GetHttpContextInfo(httpContext);

                var mailheader =GetMailHeader(httpContext);
                var exceptionDetail = GetExceptions(exception, httpContext);
                var versionNumber = GetVersionNumbers();
                var sessions = GetSession(httpContext);
                var form = GetForm(httpContext);
                var queryString = GetQueryString(httpContext);
                var cookies = GetCookies(httpContext);
                var serverVariable = GetServerVariables(httpContext);
                var mailFooter = GetMailFooter(httpContext);

                var path = Path.Combine(_environment.WebRootPath, "Templates", "ErrorHandler.html");
                var templateContent = File.ReadAllText(path);
                //templateContent = templateContent.Replace("||exceptionMessage||", exceptionMessage);
                //templateContent = templateContent.Replace("||innerException||", InnerException);
                //templateContent = templateContent.Replace("||exceptionDetails||", exceptionDetails);
                //templateContent = templateContent.Replace("||httpContextInfo||", httpContextInfo);

                templateContent = templateContent.Replace("||mailheader||", mailheader);
                templateContent = templateContent.Replace("||exceptionDetail||", exceptionDetail);
                templateContent = templateContent.Replace("||versionNumber||", versionNumber);
                templateContent = templateContent.Replace("||sessions||", sessions);
                templateContent = templateContent.Replace("||form||", form);
                templateContent = templateContent.Replace("||queryString||", queryString);
                templateContent = templateContent.Replace("||cookies||", cookies);
                templateContent = templateContent.Replace("||serverVariable||", serverVariable);
                templateContent = templateContent.Replace("||mailFooter||", mailFooter);
                // Construye el cuerpo del correo electrónico
                var emailBody = templateContent;

                
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string conexion = config.GetValue<string>("ConnectionStrings:CpalCentralConexion") ?? "";

                string bcc = config.GetValue<string>("emailBcc") ?? "";
                string email = config.GetValue<string>("error.emailList") ?? "";
                string smtpServer = config.GetValue<string>("error.smtpServer") ?? "";
                string smtpUser = config.GetValue<string>("error.smtpUser") ?? "";
                string smtpPwd = config.GetValue<string>("error.smtpPwd") ?? "";
                string smtpPort = config.GetValue<string>("error.smtpPort") ?? "";
                string smtpSsl = config.GetValue<string>("error.smtpSsl") ?? "";
                // Guarda el correo electrónico en la cola para su envío
                EnviarCorreoElectronico(conexion, "Error PortalClienteV2: " + exceptionMessage, emailBody, email, bcc, smtpServer, smtpUser, smtpPwd, smtpPort, smtpSsl);
                //emailQueueService.SaveEmailQueue("PortalClienteV2: Error", emailBody, "bsalazar@cpal.edu.pe", "System", "bsalazar@cpal.edu.pe");

                // Indica que la excepción ha sido manejada completamente
                return ValueTask.FromResult(true);
            }
            catch (Exception ex)
            {
                var InnerException = "No inner exception was generated";
                if (ex.InnerException != null) { InnerException = ex.InnerException.Message; }
                var path = Path.Combine(_environment.WebRootPath, "Templates", "ErrorHandler.html");
                var templateContent = File.ReadAllText(path);
                templateContent = templateContent.Replace("||exceptionMessage||", ex.Message);
                templateContent = templateContent.Replace("||innerException||", InnerException);
                templateContent = templateContent.Replace("||exceptionDetails||", "-");
                templateContent = templateContent.Replace("||httpContextInfo||", ex.HelpLink);

                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string conexion = config.GetValue<string>("ConnectionStrings:CpalCentralConexion") ?? "";
                string bcc = config.GetValue<string>("emailBcc") ?? "";
                string email = config.GetValue<string>("error.emailList") ?? "";
                string smtpServer = config.GetValue<string>("error.smtpServer") ?? "";
                string smtpUser = config.GetValue<string>("error.smtpUser") ?? "";
                string smtpPwd = config.GetValue<string>("error.smtpPwd") ?? "";
                string smtpPort = config.GetValue<string>("error.smtpPort") ?? "";
                string smtpSsl = config.GetValue<string>("error.smtpSsl") ?? "";

                EnviarCorreoElectronico(conexion, "Error PortalClienteV2: " + ex.Message, InnerException, email, bcc, smtpServer, smtpUser, smtpPwd, smtpPort, smtpSsl);

                // Manejo de excepciones si ocurre algún error al intentar manejar la excepción original
                logger.LogError("Error handling exception: {exception}", ex);
                // Indica que la excepción no ha sido manejada completamente
                return ValueTask.FromResult(false);
            }
            
        }

        private string GetExceptionDetails(Exception exception)
        {
            StringBuilder detailsBuilder = new StringBuilder();

            // Tipo de excepción
            detailsBuilder.AppendLine($"Tipo de excepción: {exception.GetType().FullName}");

            // Mensaje de la excepción
            detailsBuilder.AppendLine($"Mensaje de la excepción: {exception.Message}");

            // Stack trace
            detailsBuilder.AppendLine($"StackTrace: {exception.StackTrace}");

            // Detalles adicionales según el tipo de excepción
            if (exception is HttpRequestException httpException)
            {
                // Si es una excepción de solicitud HTTP, agregar detalles específicos
                detailsBuilder.AppendLine($"Método HTTP: {httpException.Message}");
                // Agregar más información específica si es necesario
            }
            else if (exception is SqlException sqlException)
            {
                // Si es una excepción de SQL, agregar detalles específicos
                detailsBuilder.AppendLine($"Número de error SQL: {sqlException.Number}");
                // Agregar más información específica si es necesario
            }
            // Puedes agregar más bloques de código para manejar otros tipos de excepción según tus necesidades

            return detailsBuilder.ToString();
        }

        private string GetHttpContextInfo(HttpContext httpContext)
        {
            StringBuilder httpContextInfoBuilder = new StringBuilder();

            // URL de la solicitud
            httpContextInfoBuilder.AppendLine($"URL de la solicitud: {httpContext.Request.Path}");

            // Método de solicitud HTTP
            httpContextInfoBuilder.AppendLine($"Método de solicitud HTTP: {httpContext.Request.Method}");

            // Encabezados de la solicitud
            httpContextInfoBuilder.AppendLine("Encabezados de la solicitud:");
            foreach (var header in httpContext.Request.Headers)
            {
                httpContextInfoBuilder.AppendLine($"{header.Key}: {header.Value}");
            }

            // Usuario autenticado
            if (httpContext.User.Identity != null)
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    httpContextInfoBuilder.AppendLine($"Usuario autenticado: {httpContext.User.Identity.Name}");
                }
            }
            

            // Dirección IP del cliente
            httpContextInfoBuilder.AppendLine($"Dirección IP del cliente: {httpContext.Connection.RemoteIpAddress}");

            // Puedes agregar más información según sea necesario

            return httpContextInfoBuilder.ToString();
        }

 
        private string GetMailHeader(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<h2>" + DateTime.Now.ToString("r") + "</h2>");
                sb.Append("<hr />");

                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td ><b>Url</b></td>");
                sb.Append("<td>" + httpContext.Request.GetDisplayUrl() + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");

                sb.Append("<hr />");

                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td ><b>Dirección IP del Usuario</b></td>");
                sb.Append("<td>" + httpContext.Connection.RemoteIpAddress?.ToString() + "</td>");
                sb.Append("</tr>");

                if (!httpContext.Connection.RemoteIpAddress.ToString().Equals(httpContext.Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append("<tr>");
                    sb.Append("<td><b>Host del Usuario</b></td>");
                    sb.Append("<td>" + httpContext.Request.Host.Host?.ToString() + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                sb.Append("<hr />");



                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td><b>Nombre de Máquina</b></td>");
                sb.Append("<td>" + Environment.MachineName + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("<hr />");
                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td><b>Navegador</b></td>");

                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (userAgent.ToLower().Contains("chrome"))
                {
                    int pos = userAgent.ToLower().IndexOf("chrome");
                    var agent = userAgent.Substring(pos + 7).Split(new char[] { ' ' });
                    sb.Append(String.Format("<td>Chrome {0}</td>", agent[0]));
                }
                else
                {
                    // Fallback for other browsers
                    sb.Append(String.Format("<td>{0}</td>", userAgent));
                }
                sb.Append("</tr>");
                sb.Append("</table>");

                sb.Append("<hr />");
            }
            catch (Exception ex)
            {
                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td><b>Exception building header :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        private string GetForm(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (httpContext.Request.HasFormContentType && httpContext.Request.Form.Count > 0)
                {
                    sb.Append("<table >");
                    sb.Append("<tr>");
                    sb.Append("<td ><b>Form</b></td>");
                    sb.Append("</tr>");

                    foreach (var key in httpContext.Request.Form.Keys)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td ><i>" + key + "</i></td>");
                        sb.Append("<td>" + httpContext.Request.Form[key].ToString() + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("<hr />");
                }
            }
            catch (Exception ex)
            {
                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building Form :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            
            return sb.ToString();
        }

        private string GetExceptions(Exception exception, HttpContext httpContext)
        {
            StringBuilder Sb = new StringBuilder();

            try
            {
                string strInnerErrorType = "";
                string strErrorTrace = "";
                string strErrorLine = "";
                string strErrorFile = "";
                string strErrorMessage = "";
                Exception _CurrentException = exception;

                Sb.Append("<table >");
                Sb.Append("<tr>");
                Sb.Append("<td ><h3>Excepciones</h3></td>");
                Sb.Append("</tr>");

                while (_CurrentException != null)
                {
                    Exception exInnerError = _CurrentException;

                    if (exInnerError != null)
                    {
                        strInnerErrorType = exInnerError.GetType().ToString();

                        // General error handling
                        System.Diagnostics.StackTrace stError = new System.Diagnostics.StackTrace(exInnerError, true);
                        for (int i = 0; i < stError.FrameCount; i++)
                        {
                            var frame = stError.GetFrame(i);
                            if (!string.IsNullOrEmpty(frame.GetFileName()))
                            {
                                strErrorLine = frame.GetFileLineNumber().ToString();
                                strErrorFile = frame.GetFileName();
                                strErrorMessage = exInnerError.Message;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(strErrorFile))
                        {
                            strErrorMessage = "Excepción no Interceptada: " + exInnerError.ToString();
                            strErrorFile = "Desconocido";
                        }

                        strErrorTrace = exInnerError.StackTrace;

                    }
                    else
                    {
                        strErrorMessage = _CurrentException.ToString();
                        strErrorTrace = _CurrentException.StackTrace;
                        strErrorFile = "Desconocido";
                    }

                    if (strErrorMessage.IndexOf("Application is restarting") > -1)
                    {
                        httpContext.Response.Clear();
                        httpContext.Response.Headers.Clear();
                        httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Error, la web se está reiniciando. Inténtelo más tarde."));
                        httpContext.Response.Body.Flush();
                        httpContext.Response.Body.Close();
                        return string.Empty; // End the response early
                    }

                    if (!string.IsNullOrEmpty(strInnerErrorType))
                    {
                        Sb.Append("<tr>");
                        Sb.Append("<td ><b>Tipo</b></td>");
                        Sb.Append("<td>" + strInnerErrorType + "</td>");
                        Sb.Append("</tr>");
                    }

                    if (!string.IsNullOrEmpty(strErrorMessage))
                    {
                        Sb.Append("<tr>");
                        Sb.Append("<td ><b>Mensaje</b></td>");
                        Sb.Append("<td><span><b>" + strErrorMessage + "</b></span></td>");
                        Sb.Append("</tr>");
                    }

                    if (!string.IsNullOrEmpty(strErrorTrace))
                    {
                        Sb.Append("<tr>");
                        Sb.Append("<td ><b>Seguimiento de la pila</b></td>");
                        Sb.Append("<td>" + strErrorTrace.Replace("\r", "<br />") + "</td>");
                        Sb.Append("</tr>");
                    }

                    if (!string.IsNullOrEmpty(strErrorFile))
                    {
                        Sb.Append("<tr>");
                        Sb.Append("<td ><b>Error de Archivo</b></td>");
                        Sb.Append("<td>" + (strErrorFile != "Desconocido" ? $"{strErrorFile}" : strErrorFile) + "</td>");
                        Sb.Append("</tr>");
                    }

                    if (!string.IsNullOrEmpty(strErrorLine))
                    {
                        Sb.Append("<tr>");
                        Sb.Append("<td ><b>Error de Línea</b></td>");
                        Sb.Append("<td>" + strErrorLine + "</td>");
                        Sb.Append("</tr>");
                    }

                    Sb.Append("<tr>");
                    Sb.Append("<td >&nbsp;</td>");
                    Sb.Append("</tr>");

                    _CurrentException = _CurrentException.InnerException;
                }
                Sb.Append("</table>");
                Sb.Append("<hr />");
            }
            catch (Exception ex)
            {

                Sb.Append("<table >");
                Sb.Append("<tr>");
                Sb.Append("<td ><b>Exception building Errors :</b></td>");
                Sb.Append("<td>" + ex.Message + "</td>");
                Sb.Append("</tr>");
                Sb.Append("</table>");
            }



            return Sb.ToString();
        }

        private string GetVersionNumbers()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td  ><h3>Versiones</h3></td>");
                sb.Append("</tr>");
                sb.Append("</table>");

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Versión .NET Core</b></td>");
                sb.Append("<td>" + System.Environment.Version.ToString() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td >&#160;</td>");
                sb.Append("<td>&#160;</td>");
                sb.Append("</tr>");

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    if (!assembly.FullName.StartsWith("System") && !assembly.FullName.StartsWith("Microsoft") && assembly.GetName().Version != null)
                    {
                        string version = assembly.GetName().Version.ToString();
                        var infoversion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                        if (infoversion != null)
                        {
                            version += " (" + infoversion.InformationalVersion + ")";
                        }

                        sb.Append("<tr>");
                        sb.Append("<td><b>" + assembly.GetName().Name + "</b></td>");
                        sb.Append("<td>Version: " + version + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td ><b>Codebase</b></td>");
                        sb.Append("<td>" + assembly.CodeBase + "</td>");
                        sb.Append("</tr>");

                        if (File.Exists(assembly.Location))
                        {
                            var fileInfo = new FileInfo(assembly.Location);
                            sb.Append("<tr>");
                            sb.Append("<td ><b>Last Write Time</b></td>");
                            sb.Append("<td>" + File.GetLastWriteTime(fileInfo.FullName).ToLongDateString() + " " + File.GetLastWriteTime(fileInfo.FullName).ToLongTimeString() + "</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("<tr>");
                        sb.Append("<td >&#160;</td>");
                        sb.Append("<td>&#160;</td>");
                        sb.Append("</tr>");
                    }
                }

                sb.Append("</table>");
                sb.Append("<hr />");
            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building Versión :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }


            return sb.ToString();
        }

        private string GetSession(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (httpContext.Session != null)
                {
                    if (httpContext.Session.Keys.Count() > 0)
                    {
                        sb.Append("<table >");
                        sb.Append("<tr>");
                        sb.Append("<td ><b>Sesión</b></td>");
                        sb.Append("</tr>");

                        foreach (string key in httpContext.Session.Keys)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td ><i>" + key + "</i></td>");

                            try
                            {
                                sb.Append("<td>" + httpContext.Session.GetString(key) + "</td>");
                            }
                            catch (Exception ex)
                            {
                                var x = ex.Message;
                                sb.Append("<td>" + x + "</td>");
                            }

                            sb.Append("</tr>");
                        }

                        sb.Append("</table>");
                        sb.Append("<hr />");
                    }
                }

            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building Session :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        private string GetQueryString(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (httpContext.Request.QueryString.HasValue)
                {
                    sb.Append("<table >");
                    sb.Append("<tr>");
                    sb.Append("<td ><b>Cadena de consulta</b></td>");
                    sb.Append("</tr>");

                    foreach (var key in httpContext.Request.Query.Keys)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td ><i>" + key + "</i></td>");
                        sb.Append("<td>" + httpContext.Request.Query[key].ToString() + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("<hr />");
                }

            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building QueryString :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        public string GetCookies(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (httpContext.Request.Cookies.Count > 0)
                {
                    sb.Append("<table >");
                    sb.Append("<tr>");
                    sb.Append("<td ><b>Cookies</b></td>");
                    sb.Append("</tr>");

                    foreach (var key in httpContext.Request.Cookies.Keys)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td ><i>" + key + "</i></td>");
                        sb.Append("<td>" + httpContext.Request.Cookies[key] + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("<hr />");
                }

            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building Cookies :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        public string GetServerVariables(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (httpContext.Request.Headers.Count > 0) // Consider using Headers instead of ServerVariables
                {
                    sb.Append("<table >");
                    sb.Append("<tr>");
                    sb.Append("<td><b>Variables de Servidor</b></td>");
                    sb.Append("</tr>");

                    foreach (var key in httpContext.Request.Headers.Keys)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td ><i>" + key + "</i></td>");
                        sb.Append("<td>" + httpContext.Request.Headers[key] + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("<hr />");
                }

            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>Exception building Server Variables :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        private string GetMailFooter(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<table >");
                sb.AppendLine($"<tr><td><b>URL</b></td><td>{httpContext.Request.Path}</td></tr>");
                sb.AppendLine($"<tr><td><b>Método HTTP</b></td><td>{httpContext.Request.Method}</td></tr>");
                sb.Append("<tr><td><b>Encabezados</b></td><td>");
                foreach (var header in httpContext.Request.Headers)
                {
                    sb.AppendLine($"{header.Key}: {header.Value} <br>");
                }
                sb.Append("</td></tr>");
                // Usuario autenticado
                if (httpContext.User.Identity != null)
                {
                    if (httpContext.User.Identity.IsAuthenticated)
                    {
                        sb.AppendLine($"<tr><td><b>Usuario autenticado:</b></td><td>{httpContext.User.Identity.Name}</td></tr>");

                    }
                }
                sb.AppendLine($"<tr><td><b>Dirección IP del cliente:</b></td><td>{httpContext.Connection.RemoteIpAddress}</td></tr>");
                sb.Append("</table>");
            }
            catch (Exception ex)
            {

                sb.Append("<table >");
                sb.Append("<tr>");
                sb.Append("<td ><b>There was a problem building this message footer :</b></td>");
                sb.Append("<td>" + ex.Message + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        public void EnviarCorreoElectronico(string _connectionString, string asunto, string emailBody, string email, string bcc, string smtpServer, string smtpUser, string smtpPwd, string smtpPort, string smtpSsl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Ejecutar un comando SQL para guardar los datos
                string sql = "INSERT INTO [dbo].[EmailQueue]([systemID],[emailFrom],[emailTo],[emailBcc],[emailSubject],[emailBody],[createdBy],[createdDate],[replyTo],[smtpServer],[smtpUser],[smtpPwd],[smtpPort],[smtpSsl],[queueStatus])";
                sql += "VALUES('20','cpal.ti@cpal.edu.pe','" + email + "','" + bcc + "','" + asunto + "','" + emailBody + "','SystemPortal',GETDATE(),'bsalazar@cpal.edu.pe','" + smtpServer + "','" + smtpUser + "','" + smtpPwd + "','" + smtpPort + "','" + smtpSsl + "','PEN')";
                using (var command = new SqlCommand(sql, connection))
                {
                    // Ejecutar el comando SQL
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
