namespace WorkflowCoreSample
{
    public abstract class RegisteredWorkflow
    {
        public abstract string Id { get; }
        public int Version => 1;
    }
}