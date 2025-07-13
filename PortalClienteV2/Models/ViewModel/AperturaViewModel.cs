using Entity.ClinicaModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalClienteV2.Models.Model;
using PortalClienteV2.Utilities.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class AperturaViewModel
    {
        public List<UvPagoCitasSearch>? ListaPagoCitasSearch { get; set; }
        public int TipoComprobante { get; set; }
        public string TipoDocumento { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El 'Documento' debe contener solo números.")]
        public string NroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string? Direccion { get; set; }

        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [MinLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Teléfono' debe contener solo números.")]
        public string? Telefono { get; set; }
        public string? CitasSeleccionadas { get; set; }
        public int? sedeId { get; set; }
        public int? Hc { get; set; }
        public int? TriajeOnlineId { get; set; }
        public int? PagoCitaId { get; set; }
        public string? Monto { get; set; }
        public Cargo Cargo { get; set; }
    }

}
