using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreSample
{
    public class MainWorkflow : IWorkflow<DataContext>
    {
        public const string SID = "Main Workflow";

        public string Id => SID;
        public int Version => 1;

        public void Build(IWorkflowBuilder<DataContext> builder)
        {
            builder.StartWith<SayHello>()
                    .Output(step => step.Name, _ => "some non-empty text")
                .While(data => !string.IsNullOrEmpty(data.Name))
                    .Do(x => x                        
                        .StartWith<EnterName>()
                            .Output(step => step.Name, data => data.Name)
                        .If(data => !string.IsNullOrEmpty(data.Name))
                            .Do(y => 
                                y.StartWith<TakePhoto>()
                                    .Input(step => step.Name, data => data.Name)
                                    .Output(step => step.Name, data => data.Name)
                                    // handle camera errors => retry
                                    // what it lacks here is the different handler for different errors
                                    .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(2))
                                .Then<FinishPerson>()
                                    .Input(step => step.Message, data => $"Thank you {data.Name}, you are done.")
                        )
                )
                .Then<SayGoodbye>();
        }
    }
}
