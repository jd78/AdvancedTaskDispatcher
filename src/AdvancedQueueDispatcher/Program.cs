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
            Console.WriteLine("Match {0} created", matchOne);

            var matchTwo = Match.Create(2, "Milan", "Roma");
            Console.WriteLine("Match {0} created", matchTwo);

            var consumer = new Consumer();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 1)
                    matchOne = matchOne.WithAction(Goal.Create(matchOne.HomeTeam));
                else
                    matchOne = matchOne.WithAction(Offside.Create(matchOne.AwayTeam));

                consumer.AddMatchAction(matchOne);
            }
            matchOne = matchOne.WithAction(Ended.Create());
            consumer.AddMatchAction(matchOne);

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 1)
                    matchTwo = matchTwo.WithAction(Goal.Create(matchTwo.HomeTeam));
                else
                    matchTwo = matchTwo.WithAction(Offside.Create(matchTwo.AwayTeam));

                consumer.AddMatchAction(matchTwo);
            }
            matchTwo = matchTwo.WithAction(Ended.Create());
            consumer.AddMatchAction(matchTwo);

            Console.ReadLine();
        }
    }
}
