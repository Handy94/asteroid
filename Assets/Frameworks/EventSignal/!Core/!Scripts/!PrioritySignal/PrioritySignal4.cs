namespace HandyPackage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PrioritySignal<T1, T2, T3, T4>
    {
        private SortedDictionary<int, List<Func<T1, T2, T3, T4, bool>>> actionQueues = new SortedDictionary<int, List<Func<T1, T2, T3, T4, bool>>>();

        public IDisposable Listen(Func<T1, T2, T3, T4, bool> action, int priority)
        {
            if (action == null)
            {
                throw new System.NullReferenceException("Null Action");
            }

            if (!actionQueues.ContainsKey(priority))
            {
                actionQueues.Add(priority, new List<Func<T1, T2, T3, T4, bool>>());
            }

            Action disposeAction = () =>
            {
                actionQueues[priority].Remove(action);
            };
            var disposableAction = new EventSignalDisposable(disposeAction);
            actionQueues[priority].Add(action);

            return disposableAction;
        }

        public bool Fire(T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (actionQueues == null)
                throw new System.NullReferenceException("Null Action Queue");
            if (actionQueues.Count == 0)
            {
                return false;
            }
            foreach (var item in actionQueues)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (!item.Value[i].Invoke(param1, param2, param3, param4)) return false;
                }
            }
            return true;
        }
    }
}
