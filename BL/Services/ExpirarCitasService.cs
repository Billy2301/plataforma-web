using BL.IServices;
using DA.IUOW;
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ExpirarCitasService : IHostedService, IDisposable
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public ExpirarCitasService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            // Se ejecutará cada 5 minutos (300,000 milisegundos)
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWorkClinica>();
                    var repository = unitOfWork.GetRepository<PagoCita>();

                    // Ejecutar el procedimiento almacenado
                    await repository.ExecuteQuery("EXEC sp_ExpirarCitas");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
