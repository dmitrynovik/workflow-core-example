using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    public class MainWorkflow : IWorkflow<DataContext>
    {
        public const string SID = "Main Workflow";

        public string Id => SID;
        public int Version => 1;

        public void Build(IWorkflowBuilder<DataContext> builder)
        {
            builder.StartWith<EnterName>()
                    .Output(step => step.Name, data => data.Name)
                .Then<ScanImage>()
                    .Input(step => step.Name, data => data.Name)
                    .Output(step => step.Name, data => data.Name)
                .Then<Finish>()
                    .Input(step => step.Message, data => $"\t{data.Name}, you are done.");
        }
    }
}
