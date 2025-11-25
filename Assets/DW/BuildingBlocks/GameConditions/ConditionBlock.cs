using System;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;

namespace DirtyWorks.GameBlocks.Conditions
{
    public enum VariableHolderType
    {
        ObjectVariable,
        GameVariable,
        Boolean,
        Int,
        Float,
        String
    }

    [System.Serializable]
    public class ConditionBlockBase
    {
        public VariableHolderType mainValueType, compareToType;

        [SerializeReference]
        public ConditionVariableHolder mainValue;

        [SerializeReference]
        public ConditionVariableHolder compareTo;

        public bool isInverted = false;

        public Comparision comparision = new Comparision();


        public virtual void SetMainValueTarget() => mainValue = DefineHolder(mainValueType);
        public virtual void SetCompareToTarget() => compareTo = DefineHolder(compareToType);

        private ConditionVariableHolder DefineHolder(VariableHolderType holderType)
        {
            return
                (holderType == VariableHolderType.ObjectVariable) ? new ObjectVariableHolder() :
                (holderType == VariableHolderType.GameVariable) ? new GameVariableHolder() :
                (holderType == VariableHolderType.Boolean) ? new BoolVariableHolder() :
                (holderType == VariableHolderType.Int) ? new IntVariableHolder() :
                (holderType == VariableHolderType.Float) ? new FloatVariableHolder() :
                (holderType == VariableHolderType.String) ? new StringVariableHolder() :
                null;
        }

        public virtual bool CompareResult()
        {
            bool result = comparision.Compare(mainValue.GetValueObject(), compareTo.GetValueObject(), isInverted);
            Debug.Log("Result = " + result);
            return result;
        }
    }
}
