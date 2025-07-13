using BL.IServices;
using Entity.CentralModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;
using PortalClienteV2.Utilities.Response;
using SkiaSharp;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalClienteV2.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        private readonly IUserManagerService<IdentityUser> _userManagerService;
        private readonly IRoleManagerService<IdentityRole> _roleManagerService;
        private readonly ISingInManagerService<IdentityUser> _sinInManagerService;
        private readonly UrlEncoder _urlEncoder;
        private readonly IUsuarioService _usuarioService;
        private readonly IUbigeoService _ubigeoService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string? imagesDirectory;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioController(IUserManagerService<IdentityUser> userManagerService, IRoleManagerService<IdentityRole> roleManagerService, ISingInManagerService<IdentityUser> sinInManagerService, UrlEncoder urlEncoder, IUsuarioService usuarioService, IUbigeoService ubigeoService, IWebHostEnvironment hostingEnvironment , IHttpContextAccessor accesor, IConfiguration configuration, ILogService logService, UserManager<IdentityUser> userManager)
        : base(accesor, configuration, logService)
        {
            _userManagerService = userManagerService;
            _roleManagerService = roleManagerService;
            _sinInManagerService = sinInManagerService;
            _urlEncoder = urlEncoder;
            _usuarioService = usuarioService;
            _ubigeoService = ubigeoService;
            _hostingEnvironment = hostingEnvironment;
            imagesDirectory = CurrentConfig().pathFile;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ClearHcPacienteTempData]
        public  IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData(int draw, int start, int length, string value, string field)
        {
            try
            {
                IQueryable<UvAspnetUserRole> query = await _usuarioService.ListarPerfilUsuarios();

                int totalRecords = await query.CountAsync();
                int totalFilteredRecords = totalRecords;

                // Aplicar filtro si hay un valor de búsqueda
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(field))
                {
                    switch (field.ToLower())
                    {
                        case "email":
                            query = query.Where(u => u.Email.Contains(value));
                            break;
                        case "nombres":
                            query = query.Where(u => u.Nombres.Contains(value));
                            break;
                        case "apellido":
                            query = query.Where(u => u.ApellidoPaterno.Contains(value));
                            break;
                        default:
                            break;
                    }

                    totalFilteredRecords = await query.CountAsync();
                }
                query = query.OrderByDescending(u => u.CreadoFecha);
                // Aplicar paginación
                var filteredUserRoles = await query.Skip(start).Take(length).ToListAsync();
                await SaveLog(1, "UvAspnetUserRole", true, "");
                return Json(new { draw = draw, recordsFiltered = totalFilteredRecords, recordsTotal = totalRecords, data = filteredUserRoles });
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvAspnetUserRole", false, ex.Message);
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = "" });
            }

        }

        #region Editar
        [HttpGet]
        [ClearHcPacienteTempData]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(string id)
        {
            try
            {
                if(id == null)
                {
                    var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                    id = appUsuario.Id;
                }

                UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(id);
                var roles = await _roleManagerService.ListarRoles();

                if (userRole == null)
                {
                    return NotFound();
                }       
                UsuarioViewModel model = new UsuarioViewModel();
                model.Id = userRole.Id;
                model.Nombre = userRole.Nombres;
                model.ApellidoPaterno = userRole.ApellidoPaterno;
                model.ApellidoMaterno = userRole.ApellidoMaterno;
                model.Bloqueo = userRole.LockoutEnabled == null? false : userRole.LockoutEnabled;
                model.EsConfirmado = userRole.EmailConfirmed == null ? false : userRole.EmailConfirmed;
                model.Email = userRole.Email;
                model.IdPerfil = userRole.IdPerfil;
                model.Rol = userRole.Rol;
                model.IdRol = userRole.IdRol;
                model.UrlImagen = userRole.UrlImagen;
                model.EsActivo = (userRole.LockoutEnd != null && userRole.LockoutEnd > DateTime.Now) ? false : true;
                model.ListaRoles = roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id,
                });
                model.fechaRegistro = userRole.CreadoFecha?.ToString("dd/MM/yyyy HH:mm");

                await SaveLog(1, "UvAspnetUserRole", true, "");
                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvAspnetUserRole", false, ex.Message);
                return NotFound();
            }  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(UsuarioViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDB = await _userManagerService.FindByIdAsync(model.Id);
                    if (usuarioDB == null)
                    {
                        ViewBag.Error = "Error al guardar cambios, intentelo mas tarde.";
                        return View(model);
                    }
                    
                    UvAspnetUserRole? userRole = await _usuarioService.ObtenerUserRoles(model.Id);
                    var rolActual = userRole.Rol;
                    var nuevoRol = model.Rol;
                    //Actualizar Rol
                    if (rolActual != nuevoRol)
                    {
                        //eliminar usuario actual
                        await _userManagerService.RemoveFromRoleAsync(usuarioDB, rolActual.ToString());
                        //Agregar nuevo rol 
                        await _userManagerService.AddToRoleAsync(usuarioDB, nuevoRol);
                    }
                    // bloquear/ desbloquear inicio de session
                    if (model.EsActivo)
                    {
                        usuarioDB.LockoutEnd = DateTime.Now; // Usuario Desbloqueado
                    }
                    else
                    {
                        usuarioDB.LockoutEnd = DateTime.Now.AddYears(100); // Usuario bloqueado
                    }
                    // bloqueo por intentos
                    usuarioDB.LockoutEnabled = model.Bloqueo;
                    usuarioDB.EmailConfirmed = model.EsConfirmado;

                    await _userManagerService.UpdateAsync(usuarioDB);
                    ViewBag.Exito = "Los datos se guardaron correctamente";

                    var roles = await _roleManagerService.ListarRoles();
                    model.ListaRoles = roles.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id,
                    });
                    return View(model);
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "UvAspnetUserRole", false, ex.Message);
                    ViewBag.Error = ex.Message;
                    var roles = await _roleManagerService.ListarRoles();
                    model.ListaRoles = roles.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id,
                    });
                    return View(model);
                }              
            }
            else
            {
                ViewBag.Error = "Error al guardar datos";
                return View(model);
            }
        }
        #endregion

        #region Eliminar
        [HttpGet]
        [ClearHcPacienteTempData]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Eliminar(string id)
        {
            var usuarioDB = await _userManagerService.FindByIdAsync(id);
            if (usuarioDB == null)
            {
                return NotFound();
            }
            await _userManagerService.RemoveUser(usuarioDB);
            await SaveLog(1, "IdentityUser", true, "");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Perfil
        //Editar perfil
        [HttpGet]
        [ClearHcPacienteTempData]
        public async Task<IActionResult> EditarPerfil(string idRef)
        {
            try
            {
                //if (idRef == null)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                var appUsuario = await _userManager.FindByNameAsync(User.Identity.Name);
                //if(appUsuario.Id != idRef)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                PerfilUsuario perfil = await _usuarioService.ObtenerPorIdRef(appUsuario.Id);
                if (perfil == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                List<Pais> listPaises = await _ubigeoService.ListaPaises();
                var filePath = "~/user/";
                EditarPerfilViewModel model = new EditarPerfilViewModel()
                {
                    Id = perfil.Id,
                    Nombres = perfil.Nombres,
                    ApellidoPaterno = perfil.ApellidoPaterno,
                    ApellidoMaterno = perfil.ApellidoMaterno,
                    Dirección = perfil.Dirección,
                    FechaNacimiento = perfil.FechaNacimiento,
                    Pais = perfil.Pais,
                    TipoDoc = perfil.TipoDoc,
                    DNI = perfil.Dni,
                    Telefono = perfil.Telefono,
                    RefAspNetUser = perfil.RefAspNetUser,
                    ImagenExternal = perfil.ImagenExternal,
                    ImagenInternal = perfil.ImagenInternal,
                    ImagenExtension = perfil.ImagenExtension,
                    FilePath = filePath,
                    Sexo = perfil.Sexo == null? 0: perfil.Sexo,

                    UrlImagen = !string.IsNullOrEmpty(perfil.UrlImagen) ? perfil.UrlImagen : "user.png",
                    listPaises = listPaises.Select(p => new SelectListItem
                    {
                        Value = p.CodigoPais.ToString(),
                        Text = p.Nombre
                    })
                };

                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "EditarPerfilViewModel", false, ex.Message);
                return NotFound();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(EditarPerfilViewModel model)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            if (ModelState.IsValid)
            {
                PerfilUsuario perfil = await _usuarioService.FindAsync(model.Id);
                if (perfil != null)
                {
                    perfil.Nombres = model.Nombres?.ToUpper();
                    perfil.ApellidoPaterno = model.ApellidoPaterno?.ToUpper();
                    perfil.ApellidoMaterno = model.ApellidoMaterno?.ToUpper();
                    perfil.Dirección = model.Dirección;
                    perfil.Telefono = model.Telefono;
                    perfil.TipoDoc = model.TipoDoc;
                    perfil.Dni = model.DNI;
                    perfil.Pais = model.Pais;
                    perfil.FechaNacimiento = model.FechaNacimiento;
                    perfil.ActualizadoPor = User.Identity.Name;
                    perfil.ImagenExternal = model.ImagenExternal;
                    perfil.ImagenInternal = model.ImagenInternal;
                    perfil.ImagenExtension = model.ImagenExtension;
                    perfil.UrlImagen = model.UrlImagen;
                    perfil.Sexo = model.Sexo;
                    //etc

                    await _usuarioService.Editar(perfil);
                    ViewBag.Exito = "Se guardaron los cambios correctamente";

                    List<Pais> listPaises = await _ubigeoService.ListaPaises();
                    model.listPaises = listPaises.Select(p => new SelectListItem
                    {
                        Value = p.CodigoPais.ToString(),
                        Text = p.Nombre
                    });
                    
                    return View(model);
                }
                else
                {
                    ViewBag.Error = "Error al editar el perfil";
                    return View(model);
                }

            }
            else
            {
                ViewBag.Error = "Error al editar el perfil, formulario invalido o faltan datos";
                return View(model);            
            }


        }


        #endregion

        #region Contraseña
        [HttpGet]
        [ClearHcPacienteTempData]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel model, string email)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = await _userManagerService.FindByEmailAsync(email);
                    if (usuario == null)
                    {
                        ViewBag.Error = "Error al actualizar el password";
                        return RedirectToAction("Error");
                    }
                    var token = await _userManagerService.GeneratePasswordResetTokenAsync(usuario);
                    var resultado = await _userManagerService.ResetPasswordAsync(usuario, token, model.Password);
                    if (resultado.Succeeded)
                    {
                        await SaveLog(1, "CambiarPasswordViewModel", true, "");
                        ViewBag.Exito = "Se actualizo el password";
                        return View();
                    }
                    else
                    {
                        await SaveLog(2, "CambiarPasswordViewModel", true, "resultado.failure");
                        ViewBag.Error = "Error al actualizar el password";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    await SaveLog(3, "CambiarPasswordViewModel", false, ex.Message);
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            }
            else
            {
                ViewBag.Error = "Error al actualizar el password";
                return View(model);
            }
        }

        [HttpGet]
        [ClearHcPacienteTempData]
        public IActionResult ConfirmacionCambioPassword()
        {
            return View();
        }


        #endregion

        #region upload

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
                await SaveLog(3, "ImagenViewModel", false, ex.Message);
                response.Estado = false;
                response.Mensaje = ex.Message;
                return StatusCode(StatusCodes.Status200OK, response.Mensaje + "path: " + imagesDirectory);
            }
        }

        public IActionResult VerImagen(string img, int h, int w)
        {
            try
            {
                string imageUrl = Path.Combine(imagesDirectory, img);
                if (!System.IO.File.Exists(imageUrl))
                {
                    // Devuelve un error o una imagen predeterminada si la imagen no existe
                    return NotFound(); // 404 Not Found
                }

                // Carga la imagen desde el archivo
                using (var image = System.Drawing.Image.FromFile(imageUrl))  // => solo se admite en windows
                {
                    // Calcula las dimensiones de la miniatura
                    int newWidth = w; // Ancho deseado
                    int newHeight = h; // Alto deseado
                    if (newWidth <= 0 || newHeight <= 0)
                    {
                        // Si no se especifican dimensiones, se conservan las dimensiones originales
                        newWidth = image.Width;
                        newHeight = image.Height;
                    }

                    // Crea la miniatura
                    using (var thumbnail = new Bitmap(newWidth, newHeight))
                    {
                        using (var graphics = Graphics.FromImage(thumbnail))
                        {
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                        }

                        // Guarda la miniatura en un MemoryStream para enviarla al cliente
                        using (var memoryStream = new MemoryStream())
                        {
                            thumbnail.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return File(memoryStream.ToArray(), "image/jpeg");
                        }
                    }
                }
            }
            catch (Exception)
            {
                // puedes registrar el error y devolver la imagen por defecto
                // Log.Error(ex, "Error al cargar la imagen");
                string defaultImagePath = Path.Combine(imagesDirectory, "user.png");
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
                return File(defaultImageBytes, "image/jpeg");
            }

        }

        public IActionResult VerAvatar(string initials)
        {
            try
            {
                var imageBytes = GenerateAvatar(initials);
                return File(imageBytes, "image/png");
            }
            catch (Exception)
            {
                // puedes registrar el error y devolver la imagen por defecto
                // Log.Error(ex, "Error al cargar la imagen");
                string defaultImagePath = Path.Combine(imagesDirectory, "user.png");
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
                return File(defaultImageBytes, "image/jpeg");
            }

        }

        public byte[] GenerateAvatar(string initials, int width = 100, int height = 100, string bgColor = "#3498db", string textColor = "#ffffff")
        {
            try
            {
                var bitmap = new SKBitmap(width, height);
                var canvas = new SKCanvas(bitmap);

                // Convertir colores de string a SKColor
                var backgroundColor = SKColor.Parse(GetColorByInitial(initials.First()));
                var fontColor = SKColor.Parse(textColor);

                // Rellenar el fondo
                canvas.Clear(backgroundColor);

                // Definir la fuente
                var paint = new SKPaint
                {
                    Color = fontColor,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center,
                    TextSize = width / 2 // Tamaño de la fuente basado en el tamaño de la imagen
                };

                // Medir el texto para centrarlo
                var textBounds = new SKRect();
                paint.MeasureText(initials, ref textBounds);

                // Dibujar el texto en el centro
                float x = width / 2;
                float y = height / 2 - textBounds.MidY;
                canvas.DrawText(initials, x, y, paint);

                // Convertir el bitmap a byte array
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                return data.ToArray();
            }
            catch (Exception)
            {

                return null;
            }

        }

        private string GetColorByInitial(char initial)
        {
            int index = char.ToUpper(initial) - 'A';
            string[] colors = new string[]
            {
                "#FF6347", // A
                "#24B0AC", // B
                "#3357FF", // C
                "#f34864", // D
                "#BA55D3", // E
                "#33FFA1", // F
                "#04A86B", // G
                "#00dec0", // H
                "#5733FF", // I
                "#87CEEB", // J
                "#00CED1", // K
                "#FFA133", // L
                "#33A1FF", // M
                "#33ceff", // N
                "#FF6347", // O
                "#FF3533", // P
                "#ffc637", // Q
                "#A1A1FF", // R
                "#FFA1A1", // S
                "#A1FF57", // T
                "#F98028", // U
                "#3357A1", // V
                "#57A1FF", // W
                "#33FFA1", // X
                "#fb9f4d", // Y
                "#0047AB"  // Z
            };

            if (index < 0 || index >= colors.Length)
            {
                index  = 1; 
            }

            return colors[index];
        }
        #endregion

        #region importar
        [HttpGet]
        [ClearHcPacienteTempData]
        [Authorize(Roles = "Administrador")]
        public IActionResult ImportarUsuarios()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ImportarUsuarios(int min, int max)
        {
            ViewBag.Error = null;
            ViewBag.Exito = null;
            var sbErrores = new StringBuilder();

            try
            {
                List<UsuariosV2> listaUsuarios = await _usuarioService.ListarUsuariosV2(min, max);
                if(listaUsuarios != null && listaUsuarios.Count() == 0)
                {
                    sbErrores.AppendLine("No se encontraron usuarios en este rango");
                }
                foreach (var usuario in listaUsuarios)
                {
                    // Busca el usuario por correo electrónico
                    var usuarioExistente = await _userManagerService.FindByEmailAsync(usuario.UserEmail.Trim());
                    if (usuarioExistente == null && !string.IsNullOrEmpty(usuario.UserEmail.Trim()))
                    {
                        var model = new IdentityUser()
                        {
                            UserName = usuario.UserEmail.Trim(),
                            Email = usuario.UserEmail.Trim(),
                        };
                        // Crea el usuario asincrónicamente
                        var resultado = await _userManagerService.CreateAsync(model, "Cpal2024!");
                        if (resultado.Succeeded)
                        {
                            // Crea el perfil del usuario asincrónicamente
                            PerfilUsuario perfil = new PerfilUsuario()
                            {
                                RefAspNetUser = model.Id,
                                CreadoFecha = DateTime.Now
                            };
                            await _usuarioService.Crear(perfil);

                            // Asigna el usuario al rol CLIENTE
                            await _userManagerService.AddToRoleAsync(model, "CLIENTE");
                            ViewBag.Exito = "Datos importados";
                        }
                        else
                        {
                            if (resultado.Errors != null && resultado.Errors.Count() > 0)
                            {
                                foreach (var error in resultado.Errors)
                                {
                                    sbErrores.AppendLine("id_"+ usuario.UsuarioId + ": " + error.Description);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sbErrores.AppendLine(ex.Message);
            }

            if (sbErrores.Length > 0)
            {
                ViewBag.Error = sbErrores.ToString();
            }

            return View();
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> LoadLog(int draw, int start, int length, string user, string fecha = null, string clase = null, int? orderColumn = null, string orderDirection = "asc")
        {
            try
            {
                IQueryable<PortalUsuarioLog> query = await _logService.ListarLogUsuario(user);

                int totalRecords = await query.CountAsync();
                int totalFilteredRecords = totalRecords;

                // Aplicar filtro si hay un valor en la clase
                if (!string.IsNullOrEmpty(clase))
                {
                    query = query.Where(u => u.Clase.Contains(clase));
                    totalFilteredRecords = await query.CountAsync();
                }

                // Aplicar filtro si hay un valor en la fecha
                if (!string.IsNullOrEmpty(fecha))
                {
                    query = query.Where(u => u.FechaHora > Convert.ToDateTime(fecha) && u.FechaHora < Convert.ToDateTime(fecha).AddDays(1));
                    totalFilteredRecords = await query.CountAsync();
                }

                // Aplicar ordenación en base al parámetro 'orderColumn' y 'orderDirection'
                if (orderColumn == 0)
                {
                    query = orderDirection.ToLower() == "asc" ? query.OrderBy(u => u.Clase) : query.OrderByDescending(u => u.Clase);
                }
                else if (orderColumn == 1)
                {
                    query = orderDirection.ToLower() == "asc" ? query.OrderBy(u => u.Nivel) : query.OrderByDescending(u => u.Nivel);
                }
                else if (orderColumn == 2)
                {
                    query = orderDirection.ToLower() == "asc" ? query.OrderBy(u => u.FechaHora) : query.OrderByDescending(u => u.FechaHora);
                }
                else if (orderColumn == 3)
                {
                    query = orderDirection.ToLower() == "asc" ? query.OrderBy(u => u.Mensaje) : query.OrderByDescending(u => u.Mensaje);
                }

                // Aplicar paginación
                var filteredUserRoles = await query.Skip(start).Take(length).ToListAsync();
                return Json(new { draw = draw, recordsFiltered = totalFilteredRecords, recordsTotal = totalRecords, data = filteredUserRoles });
            }
            catch (Exception ex)
            {
                await SaveLog(3, "UvAspnetUserRole", false, ex.Message);
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = "" });
            }

        }



    }
}
