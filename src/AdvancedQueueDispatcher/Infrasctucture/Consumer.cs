using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AdvancedQueueDispatcher.Domain;

namespace AdvancedQueueDispatcher.Infrasctucture
{
    public class Consumer : IConsumer
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
                Task.Factory.StartNew(() => ProcesActions(actionQueue, _cancellationToken), _cancellationToken);
            }
            _matchActionQueue[match.Id].Add(match, _cancellationToken);
        }

        private static void ProcesActions(BlockingCollection<Match> actionQueue, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var action = actionQueue.Take(cancellationToken);
                Console.WriteLine("Thread Id {0} executed the action {1} for the match {2}", Thread.CurrentThread.ManagedThreadId, action.Action.Message, action);
                Thread.Sleep(2000);
            }
        }
    }
}
