namespace AdvancedQueueDispatcher.Actions
{
    public class Offside : IAction
    {
        public string Message { get; private set; }
        public int Version { get; private set; }

        private Offside(string message)
        {
            Message = message;
        }

        public static Offside Create(string team)
        {
            return new Offside(string.Format("{0} is offside", team));
        }
    }
}
