using UnityEngine;
using System;
using System.Reflection;

public class InspectorValueChanger : MonoBehaviour
{
    [Header("Target Object (GameObject or Component)")]
    public UnityEngine.Object beModObject;

    [Header("Saved Selection (Editor Set)")]
    public string targetComponentType;
    public string targetFieldName;

    [Header("Value To Set")]
    public bool newValueBool;
    public int newValueInt;
    public float newValueFloat;
    public string newValueString; // we'll parse this based on field type
    public Vector2 newValueVector2;
    public Vector3 newValueVector3;
    

    /// <summary>
    /// Call this in-game to apply the new value to the selected field.
    /// </summary>
    [ContextMenu("Change Value")]
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
        FieldInfo field = compType.GetField(targetFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogWarning($"Field '{targetFieldName}' not found in component '{compType.Name}'.");
            return;
        }

        // 3️. Parse and set value based on field type
        //object parsedValue = ParseValue(field.FieldType, newValueString);
        object parsedValue = ParseValue(field.FieldType);

        field.SetValue(component, parsedValue);
        Debug.Log($"[InspectorValueChanger] Changed {compType.Name}.{targetFieldName} to {parsedValue}");
    }

    private object ParseValue(Type type, string value)
    {
        if (type == typeof(int))
            return int.TryParse(value, out var i) ? i : 0;
        if (type == typeof(float))
            return float.TryParse(value, out var f) ? f : 0f;
        if (type == typeof(bool))
            return bool.TryParse(value, out var b) && b;
        if (type == typeof(string))
            return value;
        if (type.IsEnum)
        {
            try { return Enum.Parse(type, value); }
            catch { return Activator.CreateInstance(type); }
        }
        return null;
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
