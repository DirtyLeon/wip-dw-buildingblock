using DirtyWorks.GameBlocks.Variables;
using System;
using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [Serializable]
    [ActionBlock("SetValue")]
    public class SetGameVariable : ActionBlock, IGameBlock
    {
        public GameVariables gameVariables;

        [SerializeReference]
        public VariableBlock targetVariable;
        public string variableName;

        public bool newValueBool;
        public int newValueInt;
        public float newValueFloat;
        public string newValueString;
        public Vector2 newValueVector2;
        public Vector3 newValueVector3;

        public void SetVariable()
        {
            if(targetVariable == null)
            {
                targetVariable = gameVariables.GetVariable(variableName) as VariableBlock;
            }

            if(targetVariable == null)
            {
                Debug.LogError("Variable not assigned.");
                return;
            }

            Type valueType = targetVariable.GetValue().GetType();

            object useValue =
                (valueType == typeof(bool)) ? newValueBool :
                (valueType == typeof(int)) ? newValueInt :
                (valueType == typeof(float)) ? newValueFloat :
                (valueType == typeof(string)) ? newValueString :
                (valueType == typeof(Vector2)) ? newValueVector2 :
                (valueType == typeof(Vector3)) ? newValueVector3 :
                newValueString;

            targetVariable.SetValue(useValue);
        }

        public void Run()
        {
            SetVariable();
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }
}
