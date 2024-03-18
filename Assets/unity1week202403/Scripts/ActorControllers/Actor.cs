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
    }
}
