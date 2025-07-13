using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalClienteV2.Models.ViewModel
{
    public class NuevoTriajeViewModel
    {
        [Required(ErrorMessage = "Seleccione una opción para continuar.")]
        [Display(Name = "Seleccionar")]
        public int? HistoriaClinica { get; set; }

        public IEnumerable<SelectListItem> ListHistoriaClinica { get; set; } = new List<SelectListItem>();
    }
}
