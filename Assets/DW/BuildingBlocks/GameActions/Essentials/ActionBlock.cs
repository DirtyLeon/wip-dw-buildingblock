using System;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace DirtyWorks.GameBlocks
{
    [System.Serializable]
    //[CreateAssetMenu(fileName = "ActionBlock", menuName = "Scriptable Objects/ActionBlock")]
    public abstract class ActionBlock
    {
        [HideInInspector]
        public bool enabled = true;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        public abstract IEnumerator RunCoroutine();
    }
}
