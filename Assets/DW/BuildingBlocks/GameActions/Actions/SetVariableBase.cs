using System;
using System.Collections;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    public class SetVariableBase : ActionBlock, IGameBlock
    {
        // values
        public bool newValueBool;
        public int newValueInt;
        public float newValueFloat;
        public string newValueString;
        public Vector2 newValueVector2;
        public Vector3 newValueVector3;

        public object ParseValue(Type type)
        {
            object useValue =
                (type == typeof(bool)) ? newValueBool :
                (type == typeof(int)) ? newValueInt :
                (type == typeof(float)) ? newValueFloat :
                (type == typeof(string)) ? newValueString :
                (type == typeof(Vector2)) ? newValueVector2 :
                (type == typeof(Vector3)) ? newValueVector3 :
                null;

            return useValue;
        }

        public virtual void Run()
        {
            
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield return null;
        }
    }
}
