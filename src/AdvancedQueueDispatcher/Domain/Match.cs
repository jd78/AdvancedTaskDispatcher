using AdvancedQueueDispatcher.Actions;

namespace AdvancedQueueDispatcher.Domain
{
    public class Match
    {
        public int Id { get; private set; }
        public string HomeTeam { get; private set; }
        public string AwayTeam { get; private set; }
        public IAction Action { get; private set; }

        private Match(int id, string homeTeam, string awayTeam)
        {
            Id = id;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        private Match(int id, string homeTeam, string awayTeam, IAction action)
        {
            Id = id;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Action = action;
        }

        public static Match Create(int id, string homeTeam, string awayTeam)
        {
            return new Match(id, homeTeam, awayTeam);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", HomeTeam, AwayTeam);
        }

        public Match WithAction(IAction action)
        {
            return new Match(Id, HomeTeam, AwayTeam, action);
        }
    }
}
