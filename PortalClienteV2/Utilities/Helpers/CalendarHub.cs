using Microsoft.AspNetCore.SignalR;
using PortalClienteV2.Models.Model;

namespace PortalClienteV2.Utilities.Helpers
{
    public class CalendarHub : Hub
    {
        // Método que será llamado cuando un usuario seleccione una fecha
        public async Task LockDate(string eventId)
        {
            // Informar a todos los usuarios que la fecha ha sido bloqueada
            await Clients.Others.SendAsync("LockDate", eventId);
        }

        public async Task UnlockDate(string id, string star, string end)
        {
            // Informar a todos los usuarios que la fecha ha sido bloqueada
            await Clients.Others.SendAsync("UnlockDate", id, star, end);
        }
    }
}
