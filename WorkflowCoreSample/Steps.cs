﻿using System;
using System.Diagnostics;
using System.Threading;
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

    public class Welcome : StepBase
    {
        public string Text { get; set; } = "Welcome to our self service desk\n";

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine(Text);
        }
    }

    public class EnterName : StepBase
    {
        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine("\nPlease enter your name or hit ENTER to exit");
            Name = Console.ReadLine();
        }
    }

    public class TakePhoto : StepBase
    {
        static readonly Random Rand = new Random(DateTime.Now.GetHashCode());

        public string Name { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine($"{Name}, let us take your photo.");

            // pretend we are doing something ...
            Thread.Sleep(1000);

            if (Rand.NextDouble() > 0.5)
            {
                // simulate error with probability of 50%
                const string error = "ERR: Camera failure";
                Console.WriteLine(error);
                throw new Exception(error);
            }
        }
    }

    public class FinishPerson : StepBase
    {
        public string Message { get; set; }

        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine(Message);
        }
    }

    public class SayGoodbye : StepBase
    {
        protected override void RunImpl(IStepExecutionContext context)
        {
            Console.WriteLine("\nSee you again.");
        }
    }
}
