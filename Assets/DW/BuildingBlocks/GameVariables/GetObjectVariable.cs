using System;
using System.Reflection;
using UnityEngine;

namespace DirtyWorks.GameBlocks.Variables
{
    [Serializable]
    public class GetObjectVariable
    {
        public UnityEngine.Object comparedObject;
        public string targetFieldName;
        public string targetComponentType;

        public object GetVariableValue()
        {
            Component component = null;

            if (comparedObject is GameObject go)
            {
                if (string.IsNullOrEmpty(targetComponentType))
                {
                    Debug.LogWarning("No target component type stored.");
                    return null;
                }
                component = go.GetComponent(targetComponentType);
            }
            else if (comparedObject is Component c)
            {
                component = c;
            }

            Type fieldType = component.GetType();
            FieldInfo field = fieldType.GetField(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


            if (field == null)
            {
                Debug.LogWarning($"Field '{targetFieldName}' not found in component '{fieldType.Name}'.");
                return null;
            }

            return field.GetValue(component);
        }
    }
}
