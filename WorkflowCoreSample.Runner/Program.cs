using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;

namespace WorkflowCoreSample.Runner
{
    class Program
    {
        static void Main()
        {
            var host = ConfigureWorkflowHost();
            const string workflowName = nameof(MainWorkflow);
            var wId = host.StartWorkflow(workflowName, 1, null).Result;
            Trace.TraceInformation($"Started {workflowName} instance with Id: {wId}");

            // Imitate service host run:
            while (true)
            {
                Thread.Sleep(1000);
            }

            //Console.Read();
            //host.Stop();
        }

        private static IWorkflowHost ConfigureWorkflowHost()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            services.AddTransient<MainWorkflow>();

            var serviceProvider = services.BuildServiceProvider();
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<MainWorkflow, DataContext>();
            host.Start();
            return host;
        }
    }
}
