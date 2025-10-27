using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    //[ActionBlock(typeof(Log))]
    public class Log : ActionBlock
    {
        public string message = "";

        public void DoAction()
        {
            Debug.Log(message);
        }
    }

}