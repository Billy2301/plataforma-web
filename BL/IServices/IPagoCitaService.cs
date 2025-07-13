using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPagoCitaService
    {
        Task<PagoCita> getPagoCitaById(int pagoId);
        Task<List<UvPagoCitaDetalleSearch>> getPagoCitaDetalle(int pagoCitaId);
        Task<PagoCitaDetalle> getPagoCitaDetalleById(int id);
        Task<PagoCita> GuardarPagoCita(PagoCita entity);
        Task<PagoCitaDetalle> GuardarPagoCitaDetalle(PagoCitaDetalle entity);
        Task ExpirarCitasProgramadas(int id);
        Task<int> getNextNumeroCita(int pagoCitaId, int evaluacionId);
        Task DeletePagoCitaDetalleByEvaluacion(int pagoCitaId, int evaluacionId);
    }
}
