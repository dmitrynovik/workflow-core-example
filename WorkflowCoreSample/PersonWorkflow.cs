using System;
using System.Threading;
using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    public class PersonWorkflow : IChildWorkflow<DataContext>
    {
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

        public void Build(IWorkflowBuilder<DataContext> workflow)
        {
            workflow
                // Enter person name:
                .StartWith<EnterName>()
                .Output(data => data.Name, step => step.Name)
                .If(data => !string.IsNullOrEmpty(data.Name))
                .Do(y =>
                    // Take photo:
                        y.StartWith<TakePhoto>()
                            .Input(step => step.Name, data => data.Name)
                            .Output(data => data.Name, step => step.Name)
                            // handle camera errors => retry
                            // what it lacks here is the different handler / policy for different errors
                            //.OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(2))
                            // Say 'thank you' and loop to the next person:
                            .Then<Finish>()
                            .Input(step => step.Message, data => $"Thank you {data.Name}, you are all done.")
                );
        }

    }
}
