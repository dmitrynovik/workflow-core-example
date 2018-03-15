using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    public class MainWorkflow : IWorkflow
    {
        public string Id => "Main Workflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<Start>()
                .Then<ScanImage>()
                .Then<Finish>();
        }
    }
}
