using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("Delay")]
    public class Pause : ActionBlock
    {
        public float PauseTime = 0f;

        public override IEnumerator RunCoroutine()
        {
            yield return new WaitForSeconds(PauseTime);
        }
    }
}
