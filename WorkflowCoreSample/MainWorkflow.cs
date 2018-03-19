using System;
using WorkflowCore.Interface;

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
