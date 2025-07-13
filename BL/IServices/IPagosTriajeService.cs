using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPagosTriajeService
    {
        Task<List<UvPagoTriajeSearch>> GetPagosTriajeList(string UserId);
        Task<List<UvPagoCitasSearch>> GetPagosCitasList(int pagoCitaId,string UserId);
        Task<List<UvPersonalEvaluacionSearch>> GetPersonalEvaluacionList(int evaluacionId);
        Task<List<upAgendaDiagnosticoV2Result>> GetAgendaDiagnostico(int personalID, DateTime dateFrom, DateTime dateTo, int sedeID);
        Task<UvPagoCitasSearch> GetPagosCitasDetalle(int pagoCitaDetalleId);
        Task<int> GuardarPagoTriaje(PagosTriaje entity);
        Task<PagosTriaje> GetPagosTriaje(int id);
        Task updatePagoCitasToPagado(int pagoCitaId, int pagoTriajeId);
        Task updatePagoCitaDetalleToPagado(int pagoCitaId, string? citasDetalle);
        Task NotificacionPago(PagosTriaje entityPago, string bodyEmail, string email);
        Task<string> GetDetallePagosTriaje(string CitasDetalleIds);
    }
}
