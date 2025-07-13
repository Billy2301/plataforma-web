using BL.IServices;
using DA.CentralContext;
using Entity.CentralModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using System.Security.Claims;

namespace PortalClienteV2.Controllers
{
    [ClearHcPacienteTempData]
    public class AccesoController : BaseController
    {
        private readonly IUserManagerService<IdentityUser> _userManagerService;
        private readonly IRoleManagerService<IdentityRole> _roleManagerService;
        private readonly ISingInManagerService<IdentityUser> _sinInManagerService;
        private readonly IEmailQueueService _EmailQueueService;
        private readonly ITranslationService _translationService;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<IdentityUser> _userManager;


        public AccesoController(IUserManagerService<IdentityUser> userManagerService, IRoleManagerService<IdentityRole> roleManagerService, IEmailQueueService emailQueueService, ISingInManagerService<IdentityUser> sinInManagerService, ITranslationService translationService, IUsuarioService usuarioService, IConfiguration configuration, UserManager<IdentityUser> userManager, IHttpContextAccessor accesor, ILogService logService, IWebHostEnvironment environment) : base(accesor, configuration, logService)
        {
            _userManagerService = userManagerService;
            _roleManagerService = roleManagerService;
            _EmailQueueService = emailQueueService;
            _sinInManagerService = sinInManagerService;
            _translationService = translationService;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _userManager = userManager;
            _environment = environment;

        }

        public IActionResult Index()
        {
            return View();
        }

        //Metodos de Registro
        #region Registro
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(string returnurl = null)
        {
            try
            {
                InitSliderSessions();
                if (!await _roleManagerService.RoleExistsAsync("ADMINISTRADOR"))
                {
                    await _roleManagerService.CreateAsync(new IdentityRole("ADMINISTRADOR"));
                }

                if (!await _roleManagerService.RoleExistsAsync("CLIENTE"))
                {
                    await _roleManagerService.CreateAsync(new IdentityRole("CLIENTE"));
                }

                ViewData["ReturnUrl"] = returnurl;
                RegistroViewModel model = new RegistroViewModel();
                return View(model);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/Acceso/ConfirmacionRegistro");
            try
            {
                InitSliderSessions();
                if (ModelState.IsValid)
                {
                    var usuario = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                    };
                    var resultado = await _userManagerService.CreateAsync(usuario, model.Password);

                    if (resultado.Succeeded)
                    {
                        PerfilUsuario perfil = new PerfilUsuario()
                        { 
                            RefAspNetUser = usuario.Id,
                            CreadoFecha = DateTime.Now
                        };
                        await _usuarioService.Crear(perfil);

                        //Asignacion de Usuario que se registra al rol
                        await _userManagerService.AddToRoleAsync(usuario, "CLIENTE");

                        //implementacion de confirmacion e cuenta
                        var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(usuario);
                        var urlRetorno = Url.Action("ConfirmarEmailRegistro", "Acceso", new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

                        var path = Path.Combine(_environment.WebRootPath, "Templates", "TemplateConfirCuenta.html");
                        string templateContent = System.IO.File.ReadAllText(path);

                        templateContent = templateContent.Replace("||link||", urlRetorno);
                        await _EmailQueueService.SaveEmailQueue( "CPAL Portal del Usuario: Confirmar cuenta", templateContent, model.Email,"System","bsalazar@cpal.edu.pe");


                        ////login automatico
                        //await _sinInManagerService.SignInAsync(usuario, isPersistent: false);
                        return LocalRedirect(returnurl);

                    }
                    ValidarErrores(resultado);

                }

                if(ModelState.ErrorCount > 0)
                {
                    // Acceder al primer objeto ModelState en la colección
                    var firstModelState = ModelState.Values.FirstOrDefault();
                    if (firstModelState != null)
                    {
                        // Acceder al primer error de este objeto ModelState
                        var firstError = firstModelState.Errors.FirstOrDefault();
                        if (firstError != null)
                        {
                            // Acceder al mensaje de error
                            ViewBag.Error = firstError.ErrorMessage;
                        }
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionRegistro()
        {
            InitSliderSessions();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmarEmailRegistro(string userId, string code)
        {
            InitSliderSessions();
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var usuario = await _userManagerService.FindByIdAsync(userId);
            if (usuario == null)
            {
                return View("Error");
            }

            var resultado = await _userManagerService.ConfirmEmailAsync(usuario, code);

            return View(resultado.Succeeded ? "ConfirmarEmailRegistro" : "Error");
        }

        #endregion

        //Metodos de Iniciar Sesion
        #region IniciarSesion
        [HttpGet]
        [AllowAnonymous]
        public IActionResult IniciarSesion(string? returnurl = null)
        {
            InitSliderSessions();
            ViewBag.Error = null;
            returnurl = returnurl ?? Url.Content("~/Home/Index");
            if (!Request.IsHttps && _configuration.GetValue<string>("is_production") == "1")
            {
                string rootApp = HttpContext.Request.Host.Value;
                string strRawUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return Redirect($"https://{rootApp}{strRawUrl}");
            }

            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnurl))
                {
                    return LocalRedirect(returnurl);
                }
                return LocalRedirect(returnurl);
            }

            ViewData["ReturnUrl"] = Url.IsLocalUrl(returnurl) ? returnurl : Url.Content("~/Home/Index");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(IniciarSesionViewModel model, string returnurl = null)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            returnurl ??= Url.Content("~/Home/Index");
            ViewData["ReturnUrl"] = Url.IsLocalUrl(returnurl) ? returnurl : Url.Content("~/Home/Index");


            if (ModelState.IsValid)
            {
                try
                {
                    InitSliderSessions();
                    var resultado = await _sinInManagerService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                    if (resultado.Succeeded)
                    {
                        var usuario = await _userManagerService.FindByEmailAsync(model.Email);
                        if (usuario != null && usuario.EmailConfirmed == false)
                        {
                            await _sinInManagerService.SignOutAsync();
                            ViewBag.Error = "Debe confirmar su cuenta, revise su bandeja de correo";
                            return View(model);
                        }

                        ViewData["ReturnUrl"] = returnurl;
                        await SaveLog(1, "IniciarSesionViewModel", true, "");
                        return LocalRedirect(returnurl);
                    }
                    else if (resultado.IsLockedOut)
                    {
                        await _sinInManagerService.SignOutAsync();
                        ViewBag.Error = "Usuario bloqueado";
                        await SaveLog(1, "IniciarSesionViewModel", true, "Usuario bloqueado");
                        return View("Bloqueado");
                    }
                    else if (resultado.RequiresTwoFactor) //para autenticar dos factores
                    {
                        //return RedirectToAction(nameof(VerificarCodigoAutenticador), new { returnurl, model.RememberMe });
                    }
                    else
                    {
                        await SaveLog(1, "IniciarSesionViewModel", true, "Acceso Invalido, verifique su usuario y contraseña");
                        ViewBag.Error = "Acceso Invalido, verifique su usuario y contraseña";
                        ModelState.AddModelError(string.Empty, "Acceso invalido");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "IniciarSesionViewModel", false, ex.Message);
                    ViewBag.Error = "Error al iniciar session intentelo mas tarde";
                    return View(model);
                }

            }
            ViewBag.Error = "Error al iniciar session verifique usuario y clave";
            return View(model);         
        }

        #endregion

        //Metodos de Recuperación de contraseña
        #region RecoveryPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecuperarContraseña()
        {
            InitSliderSessions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarContraseña(RecuperarContraseñaViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;

            if (ModelState.IsValid)
            {
                try
                {
                    InitSliderSessions();
                    var usuario = await _userManagerService.FindByEmailAsync(model.Email);
                    if (usuario == null)
                    {
                        ViewBag.Error = "No se encontró ningún usuario con el correo: " + model.Email.ToString();
                        return View(model);
                    }
                    var codigo = await _userManagerService.GeneratePasswordResetTokenAsync(usuario);
                    var urlRetorno = Url.Action("RestablecerContraseña", "Acceso", new { userId = usuario.Id, code = codigo }, protocol: HttpContext.Request.Scheme);

                    var path = Path.Combine(_environment.WebRootPath, "Templates", "TemplateRecoveryPass.html");
                    string templateContent = System.IO.File.ReadAllText(path);

                    templateContent = templateContent.Replace("||link||", urlRetorno);

                    await _EmailQueueService.SaveEmailQueue("CPAL Portal del Usuario: Restablecer contraseña", templateContent, model.Email, "SystemPortal", "");
                    await SaveLog(1, "RecuperarContraseñaViewModel", true, "");
                    return RedirectToAction("ConfirmacionRecuperarContraseña");
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "RecuperarContraseñaViewModel", false, ex.Message);
                    return View(model);
                }

            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionRecuperarContraseña()
        {
            InitSliderSessions();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RestablecerContraseña(string code = null)
        {
            InitSliderSessions();
            return code == null ? View("Error") : View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RestablecerContraseña(RestablecerContraseñaViewModel model)
        {
            ViewBag.Error = null;
            InitSliderSessions();
            if (ModelState.IsValid)
            {
                var usuario = await _userManagerService.FindByEmailAsync(model.Email);
                if (usuario == null)
                {
                    ViewBag.Error = "Usuario no encontrado";
                    return View(model);
                }

                var resultado = await _userManagerService.ResetPasswordAsync(usuario, model.Code, model.Password);
                if (resultado.Succeeded)
                {
                    await SaveLog(1, "RestablecerContraseñaViewModel", true, "");
                    return RedirectToAction("ConfirmacionRestablecerContraseña");
                }

                ValidarErrores(resultado);
            }
            if (ModelState.ErrorCount > 0)
            {
                // Acceder al primer objeto ModelState en la colección
                var firstModelState = ModelState.Values.FirstOrDefault();
                if (firstModelState != null)
                {
                    // Acceder al primer error de este objeto ModelState
                    var firstError = firstModelState.Errors.FirstOrDefault();
                    if (firstError != null)
                    {
                        await SaveLog(2, "RestablecerContraseñaViewModel", false, firstError.ErrorMessage);
                        // Acceder al mensaje de error
                        ViewBag.Error = firstError.ErrorMessage;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionRestablecerContraseña()
        {
            InitSliderSessions();
            return View();
        }

        #endregion

        //Metodos de Acceso Externo
        #region Acceso Externo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccesoExterno(string proveedor, string returnurl = null)
        {
            var urlRetorno = Url.Action("AccesoExternoCallback", "Acceso", new { Returnurl = returnurl });
            var propiedades = _sinInManagerService.ConfigureExternalAuthenticationProperties(proveedor, urlRetorno);

            return Challenge(propiedades, proveedor);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AccesoExternoCallback(string returnurl = null, string error = null)
        {
            returnurl = returnurl ?? Url.Content("~/Home/Index");
            if (error != null)
            {
                ModelState.AddModelError(string.Empty, $"Error en el acceso externo,{error}");
                return View(nameof(IniciarSesion));
            }

            var info = await _sinInManagerService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View(nameof(IniciarSesion));
            }

            //Acceder con el usuario del proveedor externo
            var resultado = await _sinInManagerService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (resultado.Succeeded)
            {
                // Actualizar los tokens de acceso
                return LocalRedirect(returnurl);
            }
            else if (resultado.RequiresTwoFactor) //para autenticar dos factores
            {
                return null;
                //return RedirectToAction(nameof(VerificarCodigoAutenticador), new { Returnurl = returnurl });
            }
            else
            {
                ViewData["ReturnUrl"] = returnurl;
                ViewData["NombreAMostrarProveedor"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                return View("ConfirmacionAccesoExterno", new AccesoExternoViewModel { Email = email, Name = name });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmacionAccesoExterno(AccesoExternoViewModel model, string returnurl = null)
        {
            InitSliderSessions();
            returnurl = returnurl ?? Url.Content("~/Home/Index");

            if (ModelState.IsValid)
            {
                // optiene la informacion del usuario del proveedor externo
                var info = await _sinInManagerService.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("Error");
                }

                // Busca el usuario por correo electrónico
                var usuarioExistente = await _userManagerService.FindByEmailAsync(model.Email);
                if (usuarioExistente != null)
                {
                    // Verifica si el usuario ya tiene un login externo asociado
                    var logins = await _userManagerService.GetLoginsAsync(usuarioExistente);
                    var tieneLoginExterno = logins.Any(l => l.LoginProvider == info.LoginProvider && l.ProviderKey == info.ProviderKey);

                    if (!tieneLoginExterno)
                    {
                        // Si no tiene login externo asociado, asociar el login externo
                        var result = await _userManagerService.AddLoginAsync(usuarioExistente, info);
                        if (result.Succeeded)
                        {
                            await _sinInManagerService.SignInAsync(usuarioExistente, isPersistent: false);
                            await _sinInManagerService.UpdateExternalAuthenticationTokensAsync(info);
                            return LocalRedirect(returnurl);
                        }
                        ValidarErrores(result);

                    }
                    else
                    {
                        // Si ya tiene login externo asociado, iniciar sesión
                        await _sinInManagerService.SignInAsync(usuarioExistente, isPersistent: false);
                        await _sinInManagerService.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnurl);
                    }
                }
                else
                {
                    var usuario = new IdentityUser { UserName = model.Email, Email = model.Email };
                    var resultado = await _userManagerService.CreateAsync(usuario);

                    if (resultado.Succeeded)
                    {
                        PerfilUsuario perfil = new PerfilUsuario()
                        {
                            RefAspNetUser = usuario.Id,
                            CreadoFecha = DateTime.Now
                        };
                        await _usuarioService.Crear(perfil);

                        await _userManagerService.AddToRoleAsync(usuario, "CLIENTE");

                        //Confirmar correo de usuario
                        var code = await _userManagerService.GenerateEmailConfirmationTokenAsync(usuario);
                        var confirmed = await _userManagerService.ConfirmEmailAsync(usuario, code);
                        if (!confirmed.Succeeded) // si confirmacion es error : enviar correo de confirmacion
                        {
                            var urlRetorno = Url.Action("ConfirmarEmailRegistro", "Acceso", new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

                            var path = Path.Combine(_environment.WebRootPath, "Templates", "TemplateConfirCuenta.html");
                            string templateContent = System.IO.File.ReadAllText(path);

                            templateContent = templateContent.Replace("||link||", urlRetorno);
                            await _EmailQueueService.SaveEmailQueue("CPAL Portal del Usuario: Confirmar cuenta", templateContent, model.Email, "System", "bsalazar@cpal.edu.pe");

                        } 

                        // si el usuario es creado con exito : autenticar
                        resultado = await _userManagerService.AddLoginAsync(usuario, info);
                        if (resultado.Succeeded)
                        {
                            await _sinInManagerService.SignInAsync(usuario, isPersistent: false);
                            await _sinInManagerService.UpdateExternalAuthenticationTokensAsync(info);
                            return LocalRedirect(returnurl);
                        }
                    }
                    ValidarErrores(resultado);
                }
            }
            ViewData["ReturnUrl"] = returnurl;
            return View(model);
        }


        #endregion

        //Metodos comunes
        #region comunes
        public IActionResult AccesoDenegado()
        {
            InitSliderSessions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarSession()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _sinInManagerService.SignOutAsync();
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "CerrarSession", false, ex.Message);
            }
            return RedirectToAction("Index", "Acceso");
        }

        [AllowAnonymous]
        public void ValidarErrores(IdentityResult resultado)
        {
            foreach (var error in resultado.Errors)
            {

                ModelState.AddModelError(string.Empty, error.Description);
            }

        }

        private async void InitSliderSessions()
        {
            try
            {
                GetSlider(); // Llama al método para configurar las variables de sesión
                             // Leer los valores de la sesión
                var imgSlider1 = HttpContext.Session.GetString("ImgSlider1");
                var imgSlider2 = HttpContext.Session.GetString("ImgSlider2");
                var imgSlider3 = HttpContext.Session.GetString("ImgSlider3");

                if (!string.IsNullOrEmpty(imgSlider1))
                {
                    ViewBag.ImgSlider1 = imgSlider1;
                }
                if (!string.IsNullOrEmpty(imgSlider2))
                {
                    ViewBag.ImgSlider2 = imgSlider2;
                }
                if (!string.IsNullOrEmpty(imgSlider3))
                {
                    ViewBag.ImgSlider3 = imgSlider3;
                }
            }
            catch (Exception ex)
            {
                await SaveLog(3, "InitSlider", false, ex.Message);
            }

        }

        #endregion
    }
}
