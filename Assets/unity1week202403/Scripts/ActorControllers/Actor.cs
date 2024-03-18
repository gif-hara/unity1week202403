using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Actor : MonoBehaviour
    {
        public ActorStatusController StatusController { get; private set; }

        public Actor Spawn(ActorStatus status)
        {
            var instance = Instantiate(this);
            instance.StatusController = new ActorStatusController(status);
            return instance;
        }

        public UniTask PerformActionAsync(Actor target, CancellationToken token)
        {
            Debug.Log($"{StatusController.Name} attacks {target.StatusController.Name}");
            return UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
        }
    }
}
