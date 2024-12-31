using WorkerService1;

namespace WorkerServiceExemploAula
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    //services.AddHostedService<Worker>();
                    services.AddHostedService<MonitoramentoClima>();
                })
                .UseWindowsService()
                .Build();

            host.Run();
        }
    }
}