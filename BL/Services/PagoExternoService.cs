using BL.IServices;
using DA.CentralContext;
using DA.IUOW;
using Entity.CentralModels;
using Entity.ClinicaModels;
using System;
using System.ComponentModel.Design;
using System.Net;
using System.Text;

namespace BL.Services
{
    public class PagoExternoService : IPagoExternoService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;
        private readonly ICPALCentralContextProcedures _centralProcedures;
        private readonly IPacienteService _pacienteService;
        private readonly IEmailQueueService _emailQueueService;

        public PagoExternoService(IUnitOfWorkCentral unitOfWork, ICPALCentralContextProcedures centralProcedures, IPacienteService pacienteService, IEmailQueueService emailQueueService)
        {
            _unitOfWork = unitOfWork;
            _centralProcedures = centralProcedures;
            _pacienteService = pacienteService;
            _emailQueueService = emailQueueService;
        }

        public async Task<PagosExterno> Guardar(PagosExterno entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<PagosExterno>();
                if (entity.PagoExternoId == 0)
                {
                    PagosExterno entity_created = await repository.Crear(entity);
                    if (entity_created.PagoExternoId == 0)
                    {
                        throw new TaskCanceledException("Error al realizar el pago");
                    }
                    return entity_created;
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al realizar el pago");
                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }

        public async Task<PagosExterno> getPagoExterno(int pagoExtId)
        {
            var repository = _unitOfWork.GetRepository<PagosExterno>();
            PagosExterno output = await repository.Obtener(p => p.PagoExternoId == pagoExtId);
            return output;
        }
        public async Task updatePagosDetalleReferencia(int pagoId, string referencia)
        {
            await _centralProcedures.upV2UpdatePagoDetalleReferenciaAsync(pagoId, referencia);
        }

        public async Task NotificacionPago(PagosExterno entityPago,string bodyEmail, string email)
        {

            string toEmail = ""; //separados por coma (,) si son varios
            if (!string.IsNullOrEmpty(email)) { toEmail = email; }
            if (entityPago.PagoExternoBeneficiario.HasValue)
            {
                Paciente mPaciente = await _pacienteService.GetPaciente(entityPago.PagoExternoBeneficiario.Value);
                if (mPaciente != null)
                {
                    if (!string.IsNullOrEmpty(mPaciente.EmailContacto))
                    {
                        if (toEmail.ToLower() != mPaciente.EmailContacto.ToLower()) { toEmail += "," + mPaciente.EmailContacto; }
                    }
                }
            }

            string subjectEmail = "CPAL Portal del Usuario : Confirmación de Pago";

            bodyEmail = bodyEmail.Replace("||descripcion||", entityPago.TransactionDesc);
            bodyEmail = bodyEmail.Replace("||monto||", entityPago.MonedaAbrev + " " + entityPago.PagoExternoMonto.ToString("F"));
            bodyEmail = bodyEmail.Replace("||fecha_pago||", entityPago.CreadoFecha.ToString("yyyy-MM-dd HH:mm"));
            
            if(entityPago.AreaNombre.ToUpper() == "TRATAMIENTO")
            {
                await _emailQueueService.SaveEmailQueueTrat(subjectEmail, bodyEmail, toEmail, "PORTAL_WEB", "diagnostico@cpal.edu.pe");
            }
            else if (entityPago.AreaNombre.ToUpper() == "DIAGNOSTICO")
            {
                await _emailQueueService.SaveEmailQueueDiag(subjectEmail, bodyEmail, toEmail, "PORTAL_WEB", "diagnostico@cpal.edu.pe");
            }
            else
            {
                await _emailQueueService.SaveEmailQueue(subjectEmail, bodyEmail, toEmail, "PORTAL_WEB", "tratamiento@cpal.edu.pe");
            }
        }
    }
}
