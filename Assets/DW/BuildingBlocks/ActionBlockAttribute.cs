using System;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ActionBlockAttribute : Attribute
    {
        public readonly Type blockClass;

        public ActionBlockAttribute(Type _blockClass)
        {
            this.blockClass = _blockClass;
        }
    }
}

