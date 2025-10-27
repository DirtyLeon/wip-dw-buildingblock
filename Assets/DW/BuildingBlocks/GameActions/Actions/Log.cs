using System;
using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("Message")]
    public class Log : ActionBlock, IGameBlock
    {
        public string message = "";

        public void DoAction()
        {
            Debug.Log(message);
        }

        public void Run() => DoAction();

        public override IEnumerator RunCoroutine()
        {
            DoAction();
            yield break;
        }
    }

}