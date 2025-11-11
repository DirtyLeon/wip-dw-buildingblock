using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(InspectorValueChanger))]
public class InspectorValueChangerEditor : Editor
{
    private int _componentIndex = -1;
    private int _fieldIndex = -1;

    private string[] _componentNames;
    private string[] _fieldNames;
    private Component[] _components;
    private FieldInfo[] _fields;

    private InspectorValueChanger changer;
    //private SerializedProperty inputBool, inputInt, inputFloat, inputString, inputVector2, inputVector3;

    private void OnEnable()
    {
        changer = (InspectorValueChanger)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Step 1: Draw the target field
        changer.beModObject = EditorGUILayout.ObjectField("Be Mod Object", changer.beModObject, typeof(UnityEngine.Object), true);

        if (changer.beModObject == null)
        {
            EditorGUILayout.HelpBox("Assign a GameObject or Component.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Step 2: Component selection
        Component selectedComponent = null;

        if (changer.beModObject is GameObject go)
        {
            _components = go.GetComponents<Component>();
            _componentNames = new string[_components.Length];
            for (int i = 0; i < _components.Length; i++)
                _componentNames[i] = _components[i] != null ? _components[i].GetType().Name : "Missing Component";

            // Try to restore previously saved component selection
            if (_componentIndex == -1 && !string.IsNullOrEmpty(changer.targetComponentType))
            {
                for (int i = 0; i < _components.Length; i++)
                {
                    if (_components[i].GetType().Name == changer.targetComponentType)
                    {
                        _componentIndex = i;
                        break;
                    }
                }
            }

            _componentIndex = EditorGUILayout.Popup("Select Component", _componentIndex, _componentNames);

            if (_componentIndex >= 0 && _componentIndex < _components.Length)
                selectedComponent = _components[_componentIndex];
        }
        else if (changer.beModObject is Component c)
        {
            selectedComponent = c;
            if (string.IsNullOrEmpty(changer.targetComponentType))
                changer.targetComponentType = c.GetType().Name;
        }

        if (selectedComponent == null)
        {
            EditorGUILayout.HelpBox("No component selected.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // Step 3: Field selection
        Type compType = selectedComponent.GetType();
        _fields = compType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (_fields.Length == 0)
        {
            EditorGUILayout.HelpBox("No modifiable fields found.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        _fieldNames = new string[_fields.Length];
        for (int i = 0; i < _fields.Length; i++)
            _fieldNames[i] = $"{_fields[i].Name} ({_fields[i].FieldType.Name})";

        // Try to restore saved field name
        if (_fieldIndex == -1 && !string.IsNullOrEmpty(changer.targetFieldName))
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                if (_fields[i].Name == changer.targetFieldName)
                {
                    _fieldIndex = i;
                    break;
                }
            }
        }

        _fieldIndex = EditorGUILayout.Popup("Select Variable", _fieldIndex, _fieldNames);

        // Save selections to the component
        if (_fieldIndex >= 0 && _fieldIndex < _fields.Length)
        {
            changer.targetComponentType = compType.Name;
            changer.targetFieldName = _fields[_fieldIndex].Name;
            var fieldType = _fields[_fieldIndex].FieldType;

            EditorGUILayout.Space();
            if (fieldType == typeof(bool))
                changer.newValueBool = EditorGUILayout.Toggle("New Value (bool)", changer.newValueBool);
            else if (fieldType == typeof(int))
                changer.newValueInt = EditorGUILayout.IntField("New Value (int)", changer.newValueInt);
            else if (fieldType == typeof(float))
                changer.newValueFloat = EditorGUILayout.FloatField("New Value (float)", changer.newValueFloat);
            else if (fieldType == typeof(string))
                changer.newValueString = EditorGUILayout.TextField("New Value (string)", changer.newValueString);
            else if (fieldType == typeof(Vector2))
                changer.newValueVector2 = EditorGUILayout.Vector2Field("New Value (Vector2)", changer.newValueVector2);
            else if (fieldType == typeof(Vector3))
                changer.newValueVector3 = EditorGUILayout.Vector3Field("New Value (Vector3)", changer.newValueVector3);

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Apply Value Now"))
            {
                changer.ChangeValue();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(changer);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
