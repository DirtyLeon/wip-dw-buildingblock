using System;
using System.Reflection;
using DirtyWorks.GameBlocks;
using UnityEngine;
using UnityEditor;

namespace DirtyWorks.GameBlocks.Editors
{
    [CustomPropertyDrawer(typeof(SetObjectVariable))]
    public class SetObjectValueEditor : PropertyDrawer
    {
        private int _componentIndex = -1;
        private int _fieldIndex = -1;
        private Component[] _components;
        private FieldInfo[] _fields;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw foldout and check if it's expanded
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded,
                label,
                true
            );

            if (!property.isExpanded)
                return;

            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;

            // Fetch sub-properties
            var beModObjectProp = property.FindPropertyRelative("beModObject");
            var targetComponentTypeProp = property.FindPropertyRelative("targetComponentType");
            var targetFieldNameProp = property.FindPropertyRelative("targetFieldName");
            var newValueBoolProp = property.FindPropertyRelative("newValueBool");
            var newValueIntProp = property.FindPropertyRelative("newValueInt");
            var newValueFloatProp = property.FindPropertyRelative("newValueFloat");
            var newValueStringProp = property.FindPropertyRelative("newValueString");
            var newValueVector2Prop = property.FindPropertyRelative("newValueVector2");
            var newValueVector3Prop = property.FindPropertyRelative("newValueVector3");

            // Layout helper
            //float lineHeight = EditorGUIUtility.singleLineHeight + 2f;
            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Rect line = new Rect(
                position.x, 
                position.y + lineHeight, 
                position.width, 
                EditorGUIUtility.singleLineHeight
            );

            // Draw beModObject
            GUIContent beModObjectLabel = new GUIContent("Target Object");
            EditorGUI.PropertyField(line, beModObjectProp, beModObjectLabel);
            line.y += lineHeight;

            UnityEngine.Object beModObj = beModObjectProp.objectReferenceValue;
            line.y += lineHeight;
            if (beModObj == null)
            {
                EditorGUI.HelpBox(line, "Assign a GameObject or Component.", MessageType.Info);
                EditorGUI.indentLevel = indent;
                EditorGUI.EndProperty();
                EditorGUI.indentLevel--;
                return;
            }

            Component selectedComponent = null;
            GameObject go = null;

            if (beModObj is GameObject g)
            {
                go = g;
                _components = g.GetComponents<Component>();

                string[] componentNames = new string[_components.Length];
                for (int i = 0; i < _components.Length; i++)
                    componentNames[i] = _components[i]?.GetType().Name ?? "Missing";

                // Restore saved component if possible
                if (_componentIndex == -1 && !string.IsNullOrEmpty(targetComponentTypeProp.stringValue))
                {
                    for (int i = 0; i < _components.Length; i++)
                        if (_components[i]?.GetType().Name == targetComponentTypeProp.stringValue)
                            _componentIndex = i;
                }

                _componentIndex = EditorGUI.Popup(line, "Component", _componentIndex, componentNames);
                line.y += lineHeight;

                if (_componentIndex >= 0 && _componentIndex < _components.Length)
                {
                    selectedComponent = _components[_componentIndex];
                    targetComponentTypeProp.stringValue = selectedComponent.GetType().Name;
                }
            }
            else if (beModObj is Component c)
            {
                selectedComponent = c;
                targetComponentTypeProp.stringValue = c.GetType().Name;
            }

            if (selectedComponent == null)
            {
                EditorGUI.HelpBox(line, "No component selected.", MessageType.Info);
                EditorGUI.indentLevel = indent;
                EditorGUI.EndProperty();
                EditorGUI.indentLevel--;
                return;
            }

            // Get fields
            Type compType = selectedComponent.GetType();
            _fields = compType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.NonPublic);

            if (_fields.Length == 0)
            {
                EditorGUI.HelpBox(line, "No modifiable fields found.", MessageType.Info);
                EditorGUI.indentLevel = indent;
                EditorGUI.EndProperty();
                EditorGUI.indentLevel--;
                return;
            }

            string[] fieldNames = new string[_fields.Length];
            for (int i = 0; i < _fields.Length; i++)
                fieldNames[i] = $"{_fields[i].Name} ({_fields[i].FieldType.Name})";

            // Restore saved field
            if (_fieldIndex == -1 && !string.IsNullOrEmpty(targetFieldNameProp.stringValue))
            {
                for (int i = 0; i < _fields.Length; i++)
                    if (_fields[i].Name == targetFieldNameProp.stringValue)
                        _fieldIndex = i;
            }

            _fieldIndex = EditorGUI.Popup(line, "Variable", _fieldIndex, fieldNames);
            line.y += lineHeight;

            if (_fieldIndex >= 0 && _fieldIndex < _fields.Length)
            {
                targetFieldNameProp.stringValue = _fields[_fieldIndex].Name;
                var fieldType = _fields[_fieldIndex].FieldType;
                GUIContent inputLabel = new GUIContent("Set value (" + fieldType.Name + ")");
                EditorGUILayout.Space();
                var inputProp =
                    (fieldType == typeof(bool)) ? newValueBoolProp :
                    (fieldType == typeof(int)) ? newValueIntProp :
                    (fieldType == typeof(float)) ? newValueFloatProp :
                    (fieldType == typeof(string)) ? newValueStringProp :
                    (fieldType == typeof(Vector2)) ? newValueVector2Prop :
                    (fieldType == typeof(Vector3)) ? newValueVector3Prop :
                    newValueStringProp;

                EditorGUI.PropertyField(line, inputProp, inputLabel);
                //line.y += lineHeight;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float height = lineHeight;

            if (property.isExpanded)
            {
                // 1 for callByIndex, 1 for either index/name
                height += lineHeight * 5;
            }
            return height;
        }
    }

}