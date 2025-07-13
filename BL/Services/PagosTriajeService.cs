using BL.IServices;
using DA.ClinicaContext;
using DA.IUOW;
using Entity.CentralModels;
using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL.Services
{
    public class PagosTriajeService : IPagosTriajeService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;
        private readonly ICPALClinicaContextProcedures _clinicaProcedures;
        private readonly IEmailQueueService _emailQueueService;
        private readonly ICitasService _citasService;

        public PagosTriajeService(IUnitOfWorkClinica unitOfWork,ICPALClinicaContextProcedures cPALClinicaContextProcedures, IEmailQueueService emailQueueService, ICitasService citasService)
        {
            _unitOfWork = unitOfWork;
            _clinicaProcedures = cPALClinicaContextProcedures;
            _emailQueueService = emailQueueService;
            _citasService = citasService;
        }

        public async Task<List<UvPagoCitasSearch>> GetPagosCitasList(int pagoCitaId, string UserId)
        {
            var repository = _unitOfWork.GetRepository<UvPagoCitasSearch>();
            IQueryable<UvPagoCitasSearch> outList = await repository.Consultar(p => p.PagoCitaId == pagoCitaId && p.UsuarioId == UserId);
            return outList.ToList();
        }

        public async Task<List<UvPagoTriajeSearch>> GetPagosTriajeList(string UserId)
        {
            var repository = _unitOfWork.GetRepository<UvPagoTriajeSearch>();
            IQueryable<UvPagoTriajeSearch> outList = await repository.Consultar(p => p.UsuarioId == UserId && p.EliminadoFecha == null);
            return outList.ToList();
        }

        public async Task<List<UvPersonalEvaluacionSearch>> GetPersonalEvaluacionList(int evaluacionId)
        {
            var repository = _unitOfWork.GetRepository<UvPersonalEvaluacionSearch>();
            IQueryable<UvPersonalEvaluacionSearch> outList = await repository.Consultar(p => p.EvaluacionId == evaluacionId);
            return outList.ToList();
        }

        public async Task<List<upAgendaDiagnosticoV2Result>> GetAgendaDiagnostico(int personalID, DateTime dateFrom, DateTime dateTo, int sedeID)
        {
            List<upAgendaDiagnosticoV2Result> outList = await _clinicaProcedures.upAgendaDiagnosticoV2Async(personalID, dateFrom, dateTo, sedeID);
            return outList;
        }

        public async Task<UvPagoCitasSearch> GetPagosCitasDetalle(int pagoCitaDetalleId)
        {
            var repository = _unitOfWork.GetRepository<UvPagoCitasSearch>();
            UvPagoCitasSearch output =  await repository.Obtener(p => p.PagoCitaDetalleId == pagoCitaDetalleId);
            return output;
        }

        public async Task<int> GuardarPagoTriaje(PagosTriaje entity)
        {
            var repository = _unitOfWork.GetRepository<PagosTriaje>();
            PagosTriaje output;
            if (entity.PagoId == 0)
            {
                output = await repository.Crear(entity);
            }
            else
            {
                await repository.Editar(entity);
                output = entity;
            }
            _unitOfWork.SaveChanges();
            return output.PagoId;
        }

        public async Task<PagosTriaje> GetPagosTriaje(int id)
        {
            var repository = _unitOfWork.GetRepository<PagosTriaje>();
            PagosTriaje output = await repository.Obtener(p => p.PagoId == id);
            return output;
        }

        public async Task updatePagoCitasToPagado(int pagoCitaId, int pagoTriajeId)
        {
            var repository = _unitOfWork.GetRepository<PagoCita>();
            PagoCita model = await repository.Obtener(p => p.PagoCitaId == pagoCitaId);
            model.EstadoPago = "PAGADO";
            model.CreadoFecha = DateTime.Now;
            model.PagoTriajeIdRef = pagoTriajeId;
            await repository.Editar(model);
            _unitOfWork.SaveChanges();
        }
        public async Task updatePagoCitaDetalleToPagado(int pagoCitaId, string? citasDetalle)
        {
            try
            {
                if (string.IsNullOrEmpty(citasDetalle)) return;
                string[] citasDetalleArray = citasDetalle.Split(',');
                var repository = _unitOfWork.GetRepository<PagoCitaDetalle>();

                foreach (var item in citasDetalleArray)
                {
                    if (!int.TryParse(item, out int citaDetalleId)) continue;
                    PagoCitaDetalle? model = await repository.Obtener(p =>
                        p.PagoCitaId == pagoCitaId &&
                        p.PagoCitaDetalleId == citaDetalleId);
                    if (model != null)
                    {
                        model.IsPagado = true;
                        await repository.Editar(model);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public async Task NotificacionPago(PagosTriaje entityPago, string bodyEmail, string email)
        {

            string toEmail = ""; //separados por coma (,) si son varios
            string TablaDetalle = "";
            if (!string.IsNullOrEmpty(email)) { toEmail = email; } //email; 

            if (!string.IsNullOrEmpty(entityPago.CitasDetalle))
            {
                IQueryable<UvPagoCitasSearch> list = await _citasService.getPagoCitaByDetalleIds(entityPago.CitasDetalle);

                string html = "<div><table border='1' cellpadding='5' cellspacing='0' style='border-color: lightblue; margin: auto; width:90%; border-collapse:collapse'>"
                            + "<thead><tr style='text-align:center;'><th width='80%'>Evaluación</th><th style='text-align:right;' width='20%'>Tarifa</th></tr></thead><tbody>";

                foreach (var item in list)
                {
                    // Escapar datos si es necesario
                    string descripcion = System.Net.WebUtility.HtmlEncode(item.Nombre + " - " + item.CitaNombre);
                    string precio = System.Net.WebUtility.HtmlEncode("S/ " + item.Precio?.ToString("F0"));
                    string sede = System.Net.WebUtility.HtmlEncode(item.Sede.ToString());

                    html += $"<tr><td>{descripcion}</td><td style='text-align:right;'>{precio}</td></tr>";
                }

                html += "</tbody></table></div>";
                TablaDetalle = html;
            }
            else
            {
                TablaDetalle = "<b>Descripción:</b> " + entityPago.Descripcion;
            }

            string subjectEmail = "CPAL Portal de Usuarios : Confirmación de Pago";

            bodyEmail = bodyEmail.Replace("||descripcion||", TablaDetalle);
            bodyEmail = bodyEmail.Replace("||monto||", entityPago.SimboloMonedaPedido + " " + (entityPago.Monto/100)!.Value.ToString("F0"));
            bodyEmail = bodyEmail.Replace("||fecha_pago||", entityPago.FechaCreacion!.Value.ToString("yyyy-MM-dd HH:mm"));

            await _emailQueueService.SaveEmailQueueDiag(subjectEmail, bodyEmail, toEmail, "PORTAL_WEB", "diagnostico@cpal.edu.pe");

        }

        public async Task<string> GetDetallePagosTriaje(string CitasDetalleIds)
        {
            string html = "";
            if (!string.IsNullOrEmpty(CitasDetalleIds))
            {
                IQueryable<UvPagoCitasSearch> list = await _citasService.getPagoCitaByDetalleIds(CitasDetalleIds);

                 html = "<div style='overflow-y: auto; overflow-x: hidden; max-height: 300px; '><table border='1' cellpadding='5' cellspacing='0' style='border-color: lightblue; margin-left: 15px; width:100%; border-collapse:collapse'>"
                            + "<thead class='border-1'><tr style='text-align:center;'><th style='width: 80%;'>Evaluación</th><th style='width: 20%;'>Tarifa</th></tr></thead><tbody>";

                foreach (var item in list)
                {
                    // Escapar datos si es necesario
                    string descripcion = System.Net.WebUtility.HtmlEncode(item.Nombre + " - " + item.CitaNombre);
                    string precio = System.Net.WebUtility.HtmlEncode("S/ " + item.Precio?.ToString("F0"));
                    string sede = System.Net.WebUtility.HtmlEncode(item.Sede.ToString());

                    html += $"<tr style='border-bottom: 1px solid #ddf3f9'><td>{descripcion.Replace("-", "<br>")}</td><td style='text-align:center;'>{precio}</td></tr>";
                }

                html += "</tbody></table></div>";
            }
            else
            {
                html = "<div>No hay detalle para mostrar</div>";
            }

            return html;
        }
    }
}
