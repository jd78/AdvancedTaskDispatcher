using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Infrasctucture
{
    public interface IActionService
    {
        void AddMatchAction(Match match);
    }
}