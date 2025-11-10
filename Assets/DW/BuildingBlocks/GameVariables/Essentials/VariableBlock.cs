using UnityEngine;

namespace DirtyWorks.GameBlocks.Variables
{
    [System.Serializable]
    public abstract class VariableBlock
    {
        public string variableName;

        public abstract object GetValue();
        public abstract void SetValue(object _value);
    }

    public class VariableBool : VariableBlock
    {
        public bool value;

        public override object GetValue()
        {
            return (bool)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is bool v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }

    public class VariableInt : VariableBlock
    {
        public int value;

        public override object GetValue()
        {
            return (int)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is int v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }

    public class VariableFloat : VariableBlock
    {
        public float value;

        public override object GetValue()
        {
            return (float)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is float v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }

    public class VariableString : VariableBlock
    {
        public string value;

        public override object GetValue()
        {
            return (string)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is string v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }

    public class VariableVector3 : VariableBlock
    {
        public Vector3 value;

        public override object GetValue()
        {
            return (Vector3)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is Vector3 v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }

    public class VariableVector2 : VariableBlock
    {
        public Vector2 value;

        public override object GetValue()
        {
            return (Vector2)value;
        }

        public override void SetValue(object _value)
        {
            if (_value is Vector2 v)
                value = v;
            else
                Debug.LogError(variableName + " needs a " + value.GetType() + " yet received a " + _value.GetType());
        }
    }
}
