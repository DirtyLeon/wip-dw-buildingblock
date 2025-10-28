using System;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ActionBlockAttribute : Attribute
    {
        public string Category;
        public readonly Type blockClass;

        public ActionBlockAttribute(string category)
        {
            Category = category;
        }
    }
}

