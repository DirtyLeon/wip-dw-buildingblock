using System;
using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("Debug")]
    public class Log : ActionBlock, IGameBlock
    {
        public string message = "";

        private void DoAction()
        {
            Debug.Log(message);
        }

        public void Run() => DoAction();

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }

}