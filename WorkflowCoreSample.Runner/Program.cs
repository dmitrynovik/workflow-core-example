﻿using System;
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
            var wId = host.StartWorkflow(MainWorkflow.SID, 1, null).Result;
            Console.WriteLine($"Started {MainWorkflow.SID} instance with Id: {wId}");

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
