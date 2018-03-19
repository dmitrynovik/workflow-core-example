using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample
{
    public class MainWorkflow : IWorkflow<DataContext>
    {
        public const string Name = "Main Workflow";

        public string Id => Name;
        public int Version => 1;

        public void Build(IWorkflowBuilder<DataContext> builder)
        {
            // Welcome screen
            builder.StartWith<Welcome>()
                .Output(data => data.Name, step => step.Text)
                // Loop until the input is empty:
                .While(data => !string.IsNullOrEmpty(data.Name))
                    .Do(x => x                   
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
                                .Then<FinishPerson>()
                                    .Input(step => step.Message, data => $"Thank you {data.Name}, you are all done.")
                        )
                )
                // Finish:
                .Then<SayGoodbye>();
        }
    }
}
