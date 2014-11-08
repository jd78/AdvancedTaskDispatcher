namespace AdvancedQueueDispatcher.Actions
{
    public class Goal : IAction
    {
        public string Message { get; private set; }
        public int Version { get; private set; }

        private Goal(string message)
        {
            Message = message;
        }

        public static Goal Create(string team)
        {
            return new Goal(string.Format("{0} scored", team));
        }
    }
}