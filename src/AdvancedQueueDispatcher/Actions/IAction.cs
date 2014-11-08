namespace AdvancedQueueDispatcher.Actions
{
    public interface IAction
    {
        string Message { get; }
        int Version { get; }
    }
}