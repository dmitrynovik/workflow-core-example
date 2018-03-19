using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    public class PersonWorkflow : IChildWorkflow<DataContext>
    {
        public void Build(IWorkflowBuilder<DataContext> workflow)
        {
            workflow
                // Enter person name:
                .StartWith<MainWorkflow.EnterName>()
                .Output(data => data.Name, step => step.Name)
                .If(data => !string.IsNullOrEmpty(data.Name))
                .Do(y =>
                    // Take photo:
                        y.StartWith<MainWorkflow.TakePhoto>()
                            .Input(step => step.Name, data => data.Name)
                            .Output(data => data.Name, step => step.Name)
                            // handle camera errors => retry
                            // what it lacks here is the different handler / policy for different errors
                            //.OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(2))
                            // Say 'thank you' and loop to the next person:
                            .Then<MainWorkflow.Finish>()
                            .Input(step => step.Message, data => $"Thank you {data.Name}, you are all done.")
                );
        }

    }
}
