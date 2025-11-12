using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("SetValue")]
    public class SetObjectVariable : SetVariableBase
    {
        //[Header("Target Object (GameObject or Component)")]
        public UnityEngine.Object beModObject;

        //[Header("Saved Selection (Editor Set)")]
        public string targetComponentType;
        public string targetFieldName;


        /// <summary>
        /// Call this in-game to apply the new value to the selected field.
        /// </summary>
        public void SetValue()
        {
            if (!Application.isPlaying)
                return;

            if (beModObject == null)
            {
                Debug.LogWarning("No target object assigned.");
                return;
            }

            Component component = null;

            // 1️. Get the target component
            if (beModObject is GameObject go)
            {
                if (string.IsNullOrEmpty(targetComponentType))
                {
                    Debug.LogWarning("No target component type stored.");
                    return;
                }

                component = go.GetComponent(targetComponentType);
            }
            else if (beModObject is Component c)
            {
                component = c;
            }

            if (component == null)
            {
                Debug.LogWarning("Target component not found on object.");
                return;
            }

            // 2️. Find the field
            Type compType = component.GetType();
            FieldInfo field = compType.GetField(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogWarning($"Field '{targetFieldName}' not found in component '{compType.Name}'.");
                return;
            }

            // 3️. Parse and set value based on field type
            object parsedValue = ParseValue(field.FieldType);

            field.SetValue(component, parsedValue);
        }
        private void DoAction()
        {
            SetValue();
        }

        public override void Run()
        {
            DoAction();
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }
}
