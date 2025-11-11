using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

namespace DirtyWorks.GameBlocks
{
    [ActionBlock("SetValue")]
    public class SetObjectVariable : ActionBlock, IGameBlock
    {
        //[Header("Target Object (GameObject or Component)")]
        public UnityEngine.Object beModObject;

        //[Header("Saved Selection (Editor Set)")]
        public string targetComponentType;
        public string targetFieldName;

        //[Header("Value To Set")]
        public bool newValueBool;
        public int newValueInt;
        public float newValueFloat;
        public string newValueString; // we'll parse this based on field type
        public Vector2 newValueVector2;
        public Vector3 newValueVector3;


        /// <summary>
        /// Call this in-game to apply the new value to the selected field.
        /// </summary>
        public void ChangeValue()
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
            FieldInfo field = compType.GetField(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
            {
                Debug.LogWarning($"Field '{targetFieldName}' not found in component '{compType.Name}'.");
                return;
            }

            // 3️. Parse and set value based on field type
            //object parsedValue = ParseValue(field.FieldType, newValueString);
            object parsedValue = ParseValue(field.FieldType);

            field.SetValue(component, parsedValue);
            //Debug.Log($"[InspectorValueChanger] Changed {compType.Name}.{targetFieldName} to {parsedValue}");
        }
        private void DoAction()
        {
            ChangeValue();
        }

        public void Run() => DoAction();

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }

        private object ParseValue(Type type)
        {
            if (type == typeof(bool))
                return newValueBool;
            else if (type == typeof(int))
                return newValueInt;
            else if (type == typeof(float))
                return newValueFloat;
            else if (type == typeof(string))
                return newValueString;
            else if (type == typeof(Vector2))
                return newValueVector2;
            else if (type == typeof(Vector3))
                return newValueVector3;

            return null;
        }
    }
}
