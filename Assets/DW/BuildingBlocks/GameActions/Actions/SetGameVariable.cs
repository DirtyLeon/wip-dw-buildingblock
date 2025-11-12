using DirtyWorks.GameBlocks.Variables;
using System;
using System.Collections;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [Serializable]
    [ActionBlock("SetValue")]
    public class SetGameVariable : SetVariableBase
    {
        public GameVariables gameVariables;

        //[SerializeReference]
        public VariableBlock targetVariable;
        public string variableName;
        public int targetVariableIndex = -1;

        public void SetVariable()
        {
            // Get targetVariable from variableBlocks list first.
            targetVariable = gameVariables.variableBlocks[targetVariableIndex];

            if (targetVariable == null)
            {
                Debug.LogError("Variable not assigned.");
                return;
            }

            Type valueType = targetVariable.GetValue().GetType();
            object useValue = ParseValue(valueType);

            targetVariable.SetValue(useValue);
        }

        public override void Run()
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
