using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AdvancedQueueDispatcher.Actions;
using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Infrasctucture
{
    //Alternative implementation
    public class ActionServiceWithTimeout : IActionService
    {
        private readonly ConcurrentDictionary<int, BlockingCollection<Match>> _matchActionQueue;
        private readonly CancellationToken _cancellationToken;

        public ActionServiceWithTimeout()
        {
            _matchActionQueue = new ConcurrentDictionary<int, BlockingCollection<Match>>();
            _cancellationToken = new CancellationToken();
        }

        public void AddMatchAction(Match match)
        {
            if (!_matchActionQueue.ContainsKey(match.Id))
            {
                var actionQueue = new BlockingCollection<Match>();
                _matchActionQueue.TryAdd(match.Id, actionQueue);
                Task.Factory.StartNew(() => ProcessActions(actionQueue, _cancellationToken), _cancellationToken);
            }
            _matchActionQueue[match.Id].Add(match, _cancellationToken);
            if (match.Action is Ended)
                _matchActionQueue[match.Id].CompleteAdding();
        }

        private void ProcessActions(BlockingCollection<Match> actionQueue, CancellationToken cancellationToken)
        {
            var matchId = 0;
            while (!cancellationToken.IsCancellationRequested && !actionQueue.IsCompleted)
            {
                Match matchAction;
                actionQueue.TryTake(out matchAction, TimeSpan.FromSeconds(5));
                if (matchAction == null)
                {
                    actionQueue.CompleteAdding();
                    break;
                }

                matchId = matchAction.Id;
                Console.WriteLine("Thread Id {0} executed the action {1} for the match {2}, version {3}", Thread.CurrentThread.ManagedThreadId, matchAction.Action.Message, matchAction, matchAction.Version);
            }

            BlockingCollection<Match> disposingMatch;
            _matchActionQueue.TryRemove(matchId, out disposingMatch);
            disposingMatch.Dispose();
            Console.WriteLine("Thread disposed");
        }
    }
}
