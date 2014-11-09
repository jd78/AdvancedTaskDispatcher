using System;
using System.Threading;
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

            var actionService = new ActionService();
            //var consumer = new ActionServiceWithTimeout();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 1)
                    actionService.AddMatchAction(matchOne.WithAction(Goal.Create(matchOne.HomeTeam)));
                else
                    actionService.AddMatchAction(matchOne.WithAction(Offside.Create(matchOne.AwayTeam)));
            }
            actionService.AddMatchAction(matchOne.WithAction(Ended.Create()));

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 1)
                    actionService.AddMatchAction(matchTwo.WithAction(Goal.Create(matchTwo.HomeTeam)));
                else
                    actionService.AddMatchAction(matchTwo.WithAction(Offside.Create(matchTwo.AwayTeam)));

                if (i == 5)
                    Thread.Sleep(6000);
            }
            actionService.AddMatchAction(matchTwo.WithAction(Ended.Create()));
            
            Console.ReadLine();
        }
    }
}
