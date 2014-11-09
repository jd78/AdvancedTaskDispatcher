using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AdvancedQueueDispatcher.Actions;
using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Infrasctucture
{
    public class Consumer : IActionService
    {
        private readonly ConcurrentDictionary<int, BlockingCollection<Match>> _matchActionQueue;
        private readonly CancellationToken _cancellationToken;

        public Consumer()
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
        }

        private void ProcessActions(BlockingCollection<Match> actionQueue, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var matchAction = actionQueue.Take(cancellationToken);
                Console.WriteLine("Thread Id {0} executed the action {1} for the match {2}, version {3}", Thread.CurrentThread.ManagedThreadId, matchAction.Action.Message, matchAction, matchAction.Version);

                //Kill the thread and remove the item.
                if (matchAction.Action is Ended)
                {
                    BlockingCollection<Match> disposingMatch;
                    _matchActionQueue.TryRemove(matchAction.Id, out disposingMatch);
                    disposingMatch.Dispose();
                    Console.WriteLine("Thread disposed");
                    break;
                }
                
                Thread.Sleep(2000);
            }
        }
    }
}
