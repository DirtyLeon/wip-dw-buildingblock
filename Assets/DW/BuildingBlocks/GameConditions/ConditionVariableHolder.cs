using System;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;

namespace DirtyWorks.GameBlocks.Conditions
{
    [System.Serializable]
    public abstract class ConditionVariableHolder
    {

        public abstract object GetValueObject();
    }

    [System.Serializable]
    public class ObjectVariableHolder : ConditionVariableHolder
    {
        public GetObjectVariable getObjectVariable = new GetObjectVariable();

        public override object GetValueObject()
        {
            return getObjectVariable.GetVariableValue();
        }
    }

    [System.Serializable]
    public class GameVariableHolder : ConditionVariableHolder
    {
        public GetGameVariable getGameVariable = new GetGameVariable();

        public override object GetValueObject()
        {
            return getGameVariable.GetVariableValue();
        }
    }

    [System.Serializable]
    public class BoolVariableHolder : ConditionVariableHolder
    {
        public bool value = false;
        public bool isRandom = false;

        public override object GetValueObject()
        {
            bool result;
            if (isRandom)
            {
                System.Random random = new System.Random();
                result = random.Next(2) == 1;
            }
            else
            {
                result = value;
            }
            return result;
        }
    }

    [System.Serializable]
    public class IntVariableHolder : ConditionVariableHolder
    {
        public int value = 0;
        public bool isRandom = false;
        public int randomMin, randomMax;
        public override object GetValueObject()
        {
            return (isRandom) ? UnityEngine.Random.Range(randomMin, randomMax) :
                value;
        }
    }

    [System.Serializable]
    public class FloatVariableHolder : ConditionVariableHolder
    {
        public float value = 0;
        public bool isRandom = false;
        public float randomMin, randomMax;

        public override object GetValueObject()
        {
            return (isRandom) ? UnityEngine.Random.Range(randomMin, randomMax) :
                value;
        }
    }

    [System.Serializable]
    public class StringVariableHolder : ConditionVariableHolder
    {
        public string value;

        public override object GetValueObject()
        {
            return value;
        }
    }
}
