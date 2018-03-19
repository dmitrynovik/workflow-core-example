using WorkflowCore.Interface;

namespace WorkflowCoreSample
{
    public interface IChildWorkflow<TData>
    {
        void Build(IWorkflowBuilder<TData> workflow);
    }
}