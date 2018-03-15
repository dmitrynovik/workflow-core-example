using System;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;

namespace WorkflowCoreSample.Runner
{
    class Program
    {
        static void Main()
        {
            var host = ConfigureWorkflowHost();
            var result = host.StartWorkflow("Main Workflow", 1, null).Result;
            Console.WriteLine(result);
            
            Console.Read();
            host.Stop();
        }

        private static IWorkflowHost ConfigureWorkflowHost()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            services.AddTransient<MainWorkflow>();

            var serviceProvider = services.BuildServiceProvider();
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<MainWorkflow>();
            host.Start();
            return host;
        }
    }
}
