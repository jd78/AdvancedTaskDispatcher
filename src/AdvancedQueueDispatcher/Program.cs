using System;
using AdvancedQueueDispatcher.Actions;
using AdvancedQueueDispatcher.Domain;
using AdvancedQueueDispatcher.Infrasctucture;

namespace AdvancedQueueDispatcher
{
    class Program
    {
        static void Main()
        {
            //Create two matches
            var matchOne = Match.Create(1, "Lazio", "Inter");
            var matchTwo = Match.Create(2, "Milan", "Roma");

            var consumer = new Consumer();

            for (var i = 0; i < 10; i++)
            {
                Match match;
                if (i % 2 == 1)
                    match = matchOne.WithAction(Goal.Create(matchOne.HomeTeam));
                else
                    match = matchOne.WithAction(Offside.Create(matchOne.AwayTeam));

                consumer.AddMatchAction(match);
            }

            for (var i = 0; i < 10; i++)
            {
                Match match;
                if (i % 2 == 1)
                    match = matchTwo.WithAction(Goal.Create(matchTwo.HomeTeam));
                else
                    match = matchTwo.WithAction(Offside.Create(matchTwo.AwayTeam));

                consumer.AddMatchAction(match);
            }

            Console.ReadLine();
        }
    }
}
