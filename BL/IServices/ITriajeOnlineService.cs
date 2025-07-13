using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface ITriajeOnlineService
    {
        /// <summary>
        /// Crea una nueva instancia de TriageOnline junto con sus entidades relacionadas TriageCotejoExt y TriageCotejo
        /// y las guarda en la base de datos dentro de una transacción.
        /// </summary>
        /// <param name="modelTriaje">Instancia de TriageOnline a crear.</param>
        /// <param name="modelCotejoExt">Instancia de TriageCotejoExt relacionada a crear.</param>
        /// <param name="modelCotejo">Instancia de TriageCotejo relacionada a crear.</param>
        /// <returns>El ID de la instancia de TriageOnline creada.</returns>
        /// <exception cref="Exception">Se lanza cuando ocurre un error durante la creación o guardado de las entidades.</exception>
        Task<int> Crear(TriageOnline modelTriaje, TriageCotejoExt modelCotejoExt, TriageCotejo modelCotejo);

        Task<UvPacienteTriajeSearch> GetTriageOnline(int id);

        Task<String> SearchTriaje(string tipoDoc, string dni);

        Task<List<UvPacienteTriajeSearch>> GetListTriajeOnlinePendienteAsync(string userId);
        Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnosticoPendienteByDniAsync(string userId, string? dni);
        Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnosticoPendienteByDniAsync(string userId, string? dni, string estado);

        Task<TriageOnline> GetTriageOnlineById(int id);

        Task<Triage> GetTriageById(int id);
        Task<Triage> GuardarTriage(Triage entity);

        Task<TriageOnline> GuardarTriageOnline(TriageOnline entity);
        Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnostico(string userId);
    }
}
