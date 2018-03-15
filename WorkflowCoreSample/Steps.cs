using System;
using System.Diagnostics;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample
{
    public class DataContext
    {
        public string Name { get; set; }
    }

    public class StepBase : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Trace.TraceInformation($"Executing {GetType()}");
            RunImpl(context);
            return ExecutionResult.Next();
        }

        protected virtual void RunImpl(IStepExecutionContext context) {  }
    }

    public class SayHello : StepBase
    {
        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine("Welcome to our self service desk.");
        }
    }

    public class EnterName : StepBase
    {
        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine("Please enter your name or hit ENTER to exit");
            Name = Console.ReadLine();
        }
    }

    public class ScanImage : StepBase
    {
        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine($"{Name}, please scan your passport");
        }
    }

    public class Finish : StepBase
    {
        public string Message { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine(Message);
        }
    }
}
