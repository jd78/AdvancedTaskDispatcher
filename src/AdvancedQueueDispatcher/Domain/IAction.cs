namespace AdvancedQueueDispatcher.Domain
{
    public interface IAction
    {
        string Message { get; }
    }
}