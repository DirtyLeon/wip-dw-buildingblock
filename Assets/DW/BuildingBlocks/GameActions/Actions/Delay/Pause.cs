using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("Delay")]
    public class PauseForSeconds : ActionBlock
    {
        public float PauseTime = 0f;

        public override IEnumerator RunCoroutine()
        {
            yield return new WaitForSeconds(PauseTime);
        }
    }

    [ActionBlock("Delay")]
    public class PauseForFrames : ActionBlock
    {
        public int PauseFrames = 1;

        public override IEnumerator RunCoroutine()
        {
            var frameCount = 0;
            while(frameCount < PauseFrames)
            {
                yield return null;
                frameCount++;
            }
        }
    }

    [ActionBlock("Delay")]
    public class PauseForFixedUpdate : ActionBlock
    {
        public override IEnumerator RunCoroutine()
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
