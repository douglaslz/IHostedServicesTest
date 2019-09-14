using IHostedServicesTest.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServicesTest.Services
{
    public class ConsumeScopedService : IHostedService, IDisposable
    {
        public ConsumeScopedService(IServiceProvider service)
        {
            Services = service;
        }
        private Timer _timer;
        public IServiceProvider Services;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //this make that the process is going to be executed only if the timer is no null
            _timer?.Dispose();
        }
        private void DoWork(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var message = "ConsumeScopedService. Received message at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                var log = new IHostedServiceLog() { Message = message };
                context.HostedServiceLogs.Add(log);
                context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

    }
}
