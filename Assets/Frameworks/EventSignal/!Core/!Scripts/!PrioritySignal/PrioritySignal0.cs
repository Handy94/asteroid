namespace HandyPackage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PrioritySignal
    {
        private SortedDictionary<int, List<Func<bool>>> actionQueues = new SortedDictionary<int, List<Func<bool>>>();

        public IDisposable Listen(Func<bool> action, int priority)
        {
            if (action == null)
            {
                throw new NullReferenceException("Null Action");
            }

            if (!actionQueues.ContainsKey(priority))
            {
                actionQueues.Add(priority, new List<Func<bool>>());
            }

            Action disposeAction = () =>
            {
                actionQueues[priority].Remove(action);
            };
            var disposableAction = new EventSignalDisposable(disposeAction);
            actionQueues[priority].Add(action);

            return disposableAction;
        }

        public bool Fire()
        {
            if (actionQueues == null)
                throw new NullReferenceException("Null Action Queue");
            if (actionQueues.Count == 0)
            {
                return false;
            }

            foreach (var item in actionQueues)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (!item.Value[i].Invoke()) return false;
                }
            }
            return true;
        }
    }
}
