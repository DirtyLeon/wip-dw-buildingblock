using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    public class GameActions : MonoBehaviour
    {
        [SerializeReference]
        public List<ActionBlock> actionBlocks;

        public bool ExecuteOnEnable = false;
        public bool ExecuteOnStart = false;

        private void OnEnable()
        {
            if (ExecuteOnEnable)
                ExecuteList();
        }

        private void Start()
        {
            if (ExecuteOnEnable)
                return;

            if(ExecuteOnStart)
                ExecuteList();
        }

        public void ExecuteList()
        {
            StartCoroutine(ExecuteListCoroutine());
        }

        IEnumerator ExecuteListCoroutine()
        {
            foreach (var block in actionBlocks)
            {
                if(block.enabled)
                    yield return block.RunCoroutine();
            }
        }
    }
}