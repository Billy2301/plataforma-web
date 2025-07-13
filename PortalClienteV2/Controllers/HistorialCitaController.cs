using BL.IServices;
using culqi.net;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entity.ClinicaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalClienteV2.Models.ViewModel;
using PortalClienteV2.Utilities.Helpers;

namespace PortalClienteV2.Controllers
{
    public class HistorialCitaController : BaseController
    {
        private readonly IPacienteService _pacienteService;
        private readonly IConverter _converter;
        public readonly IConfiguration _configuration;

        public HistorialCitaController(IHttpContextAccessor accesor, IConfiguration configuration, IPacienteService paciente, IConverter converter, ILogService logService) : base(accesor, configuration, logService)
        {
            _pacienteService = paciente;
            _converter = converter;
            _configuration = configuration;
        }

        [Authorize]
        [ClearHcPacienteTempData]
        [HttpGet]
        public async Task<IActionResult> Informe()
        {
            try
            {
                HistorialCitaViewModel model = new HistorialCitaViewModel();
                List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);
                var listaHistoriaClinica = Pacientes.Select(p => new SelectListItem
                {
                    Text = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                    Value = p.NumeroHistoriaClinica.ToString(),
                }).ToList();
                model.ListHistoriaClinica = listaHistoriaClinica;
                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "HistorialCitaViewModel", false, ex.Message);
                return RedirectToAction("Historias", "HistoriaClinica");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Informe(HistorialCitaViewModel model)
        {
            try
            {
                List<Paciente> Pacientes = await _pacienteService.ListarPacientesPorUsuario(User.Identity.Name);
                var listaHistoriaClinica = Pacientes.Select(p => new SelectListItem
                {
                    Text = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                    Value = p.NumeroHistoriaClinica.ToString(),
                }).ToList();
                model.ListHistoriaClinica = listaHistoriaClinica;
                model.ListaHistorialDiagnostico = await _pacienteService.getHistorialCitaDiag(model.HistoriaClinica.ToString());
                model.ListaHistorialTratamiento = await _pacienteService.getHistorialCitaTrat(model.HistoriaClinica.ToString()!);
                model.paciente = await _pacienteService.GetPaciente(Convert.ToInt32(model.HistoriaClinica));
                return View(model);
            }
            catch (Exception ex)
            {
                await SaveLog(3, "List<Paciente>", false, ex.Message);
                return RedirectToAction("Historias", "HistoriaClinica");
            }
        }

        [HttpGet("generate-pdf")]
        public IActionResult Imprimir(string hc, string area)
        {
            try
            {
                string htmlContent = "";
                string nameInf = "Informe.pdf";
                string appName = _configuration.GetValue<string>("appName") + "/";

                if (_configuration.GetValue<string>("is_production") == "0") { appName = string.Empty; }

                if (area == "dx")
                {
                    htmlContent = $"{this.Request.Scheme}://{this.Request.Host}/{appName}HistorialCita/DiagnosticoPDF?hc={hc}";
                    nameInf = "Informe-Dx-" + hc + ".pdf";
                }
                else if (area == "tx")
                {
                    htmlContent = $"{this.Request.Scheme}://{this.Request.Host}/{appName}HistorialCita/TratamientoPDF?hc={hc}";
                    nameInf = "Informe-Tx-" + hc + ".pdf";
                }
                else
                {
                    htmlContent = "<h1>Error en el Informe</h1><p>No se ha posiso mostrar el informe ...</p>";
                }


                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = new GlobalSettings()
                    {
                        PaperSize = PaperKind.A4,
                        Orientation = Orientation.Portrait,
                        Margins = new MarginSettings { Top = 10, Bottom = 15, Left = 10, Right = 10 },
                        DocumentTitle = "Informe PDF"
                    },

                    Objects =  {
                        new ObjectSettings()
                        {
                            Page = htmlContent,
                            PagesCount = true,
                            WebSettings = { DefaultEncoding = "utf-8" },
                            FooterSettings = {
                                FontName = "Arial",
                                FontSize = 9,
                                Line = false, // Línea sobre el pie de página
                                Center = "",
                                Right = "Página [page] de [toPage]", // Número de página actual y total
                                Spacing = 5 // Espaciado entre el pie de página y el contenido
                            }
                        }
                    }
                };
                var archivoPDF = _converter.Convert(pdf);
                return File(archivoPDF, "application/pdf", nameInf);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IActionResult> DiagnosticoPDF(string hc)
        {

            HistorialCitaViewModel model = new HistorialCitaViewModel();
            model.ListaHistorialDiagnostico = await _pacienteService.getHistorialCitaDiag(hc.ToString());
            model.paciente = await _pacienteService.GetPaciente(Convert.ToInt32(hc));
            return View(model);
        }

        public async Task<IActionResult> TratamientoPDF(string hc)
        {

            HistorialCitaViewModel model = new HistorialCitaViewModel();
            model.ListaHistorialTratamiento = await _pacienteService.getHistorialCitaTrat(hc.ToString());
            model.paciente = await _pacienteService.GetPaciente(Convert.ToInt32(hc));
            return View(model);
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                },
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            return _converter.Convert(doc);
        }
    }
}
