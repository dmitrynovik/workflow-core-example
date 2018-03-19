using System;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Primitives;

namespace WorkflowCoreSample
{
    public class MainWorkflow : RegisteredWorkflow, IWorkflow<DataContext>
    {
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

        public class Finish : StepBase
        {
            public string Message { get; set; }

            protected override void RunImpl(IStepExecutionContext context)
            {
                Console.WriteLine(Message);
            }
        }

        public class Shutdown : StepBase
        {
            protected override void RunImpl(IStepExecutionContext context)
            {
                Console.WriteLine("\nThe machine is shutting down...");
            }
        }

        public override string Id => nameof(MainWorkflow);

        public void Build(IWorkflowBuilder<DataContext> builder)
        {
            builder
                .StartWith<Welcome>()
                .Output(data => data.Name, step => step.Text)
                // Loop until the input name is empty,
                // processing that person:
                .While(data => !string.IsNullOrEmpty(data.Name))
                .Do(x => new PersonWorkflow().Build(x))
                .Then<Shutdown>();
        }
    }
}
