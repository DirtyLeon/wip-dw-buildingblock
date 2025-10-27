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
    }
}