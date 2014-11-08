using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Actions
{
    public class Ended : IAction
    {
        public string Message { get; private set; }
        
        private Ended(string message)
        {
            Message = message;
        }

        public static Ended Create()
        {
            return new Ended("Match ended");
        }
    }
}