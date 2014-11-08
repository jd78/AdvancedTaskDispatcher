using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Infrasctucture
{
    public interface IConsumer
    {
        void AddMatchAction(Match match);
    }
}