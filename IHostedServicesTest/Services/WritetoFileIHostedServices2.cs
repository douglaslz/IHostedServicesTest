using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServicesTest.Services
{
    public class WritetoFileIHostedServices2 : IHostedService, IDisposable
    {
        private readonly IHostingEnvironment enviroment;
        private readonly string filename = "File 2.txt";
        private Timer timer;

        public WritetoFileIHostedServices2(IHostingEnvironment environment)
        {
            this.enviroment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteFile("WriteHostedService Started");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(7));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteFile("WriteHostedService Stoped");
            //stop the timer
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //this make that the process is going to be executed only if the timer is no null
            timer?.Dispose();
        }
        private void DoWork(object state)
        {
            WriteFile("WriteFileHostedServices: Doing some work at: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void WriteFile(string message)
        {
            var path = $@"{enviroment.ContentRootPath}\wwwroot\{filename}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}

