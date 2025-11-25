using UnityEngine;

namespace DirtyWorks.GameBlocks.Variables
{
    [System.Serializable]
    public class GetGameVariable
    {
        public GameVariables variables;
        public string variableName;

        public object GetVariableValue()
        {
            var variable = variables.GetVariable(variableName) as VariableBlock;
            return variable.GetValue();
        }
    }
}
