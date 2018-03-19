using System;
using System.Diagnostics;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample
{
    public class StepBase : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Trace.TraceInformation($"Executing {GetType()}");
            try
            {
                RunImpl(context);
            }
            catch (Exception e)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ForegroundColor = color;
            }
            return ExecutionResult.Next();
        }

        protected virtual void RunImpl(IStepExecutionContext context) {  }
    }
}
