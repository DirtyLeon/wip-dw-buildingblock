using System.Collections;
using UnityEngine.Events;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("UnityEvents")]
    public class CallUnityEvent : ActionBlock, IGameBlock
    {
        public UnityEvent triggerUnityEvents;

        private void DoAction()
        {
            triggerUnityEvents.Invoke();
        }

        public void Run() => DoAction();

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }
}
