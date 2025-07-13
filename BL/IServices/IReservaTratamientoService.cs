using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IReservaTratamientoService
    {
        Task<ReservaTratamiento> Guardar(ReservaTratamiento entity);
        Task<ReservaTratamiento?> GetReserva(int id);
        Task<DetalleReservaTratamiento> GuardarDetalle(DetalleReservaTratamiento entity);
        Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteAsync(string userId);
        Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteTipoAsync(string userId,int tipo);
        Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteTipoWhitHistoriaAsync(string userId, int tipo, int hc);
    }
}
