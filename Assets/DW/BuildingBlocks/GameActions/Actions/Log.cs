using System;
using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("Debug")]
    public class Log : ActionBlock, IGameBlock
    {
        public string message = "";

        public void Run() => DoAction();

        public void DoAction()
        {
            Debug.Log(message);
        }

        public override IEnumerator RunCoroutine()
        {
            DoAction();
            yield break;
        }
    }

}