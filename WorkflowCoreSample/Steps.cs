using System;
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
            Console.WriteLine($"Executing {GetType()}");
            RunImpl(context);
            return ExecutionResult.Next();
        }

        protected virtual void RunImpl(IStepExecutionContext context) {  }
    }

    public class EnterName : StepBase
    {
        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine("Please enter your name");
            Name = Console.ReadLine();
        }
    }

    public class ScanImage : StepBase
    {
        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine($"\t{Name}, please scan your passport");
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
