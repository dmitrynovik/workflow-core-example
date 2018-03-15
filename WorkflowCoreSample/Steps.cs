using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample
{
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

    public class Start : StepBase
    {
    }

    public class ScanImage : StepBase
    {
    }

    public class Finish : StepBase
    {
    }
}
