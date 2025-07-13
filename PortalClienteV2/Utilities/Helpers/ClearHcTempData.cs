using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PortalClienteV2.Utilities.Helpers
{
    public class ClearHcPacienteTempData : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            var actionName = context.RouteData.Values["action"]?.ToString();
            var allowedActions = new[] { "Evaluaciones", "Terapias" };

            if (controller != null && !allowedActions.Contains(actionName))
            {
                controller.TempData.Remove("HcPaciente");
            }

            base.OnActionExecuting(context);
        }
    }
}
