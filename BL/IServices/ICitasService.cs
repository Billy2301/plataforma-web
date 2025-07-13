using Entity.ClinicaModels;

namespace BL.IServices
{
    public interface ICitasService
    {
        Task<IQueryable<upCitasProximasPorUsuarioResult>> getCitasProximas(string userName);
        Task<IQueryable<UvPagoCitasSearch>> getPagoCitaByDetalleIds(string ids);
    }
}
