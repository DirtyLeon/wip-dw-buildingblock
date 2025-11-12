using System;
using System.Reflection;
using DirtyWorks.GameBlocks;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;
using UnityEditor;

namespace DirtyWorks.GameBlocks.Editors
{
    [CustomPropertyDrawer(typeof(SetGameVariable))]
    public class SetGameVariableEditor : PropertyDrawer
    {
        private const int expandedHeight = 4;
        private SerializedProperty targetVariableIndexProp;
        private SerializedProperty newValueBoolProp;
        private SerializedProperty newValueIntProp;
        private SerializedProperty newValueFloatProp;
        private SerializedProperty newValueStringProp;
        private SerializedProperty newValueVector2Prop;
        private SerializedProperty newValueVector3Prop;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded,
                label,
                true
            );

            if (!property.isExpanded)
                return;

            targetVariableIndexProp = property.FindPropertyRelative("targetVariableIndex");
            newValueBoolProp = property.FindPropertyRelative("newValueBool");
            newValueIntProp = property.FindPropertyRelative("newValueInt");
            newValueFloatProp = property.FindPropertyRelative("newValueFloat");
            newValueStringProp = property.FindPropertyRelative("newValueString");
            newValueVector2Prop = property.FindPropertyRelative("newValueVector2");
            newValueVector3Prop = property.FindPropertyRelative("newValueVector3");

            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Rect line = new Rect(
                position.x,
                position.y + lineHeight,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);

            var gameVariablesProp = property.FindPropertyRelative("gameVariables");

            // Draw GameVariables field.
            EditorGUI.PropertyField(line, gameVariablesProp);
            line.y += lineHeight;

            // Draw Variable Select dropdown.
            var variables = gameVariablesProp.objectReferenceValue as GameVariables;
            if (gameVariablesProp.objectReferenceValue == null)
            {
                // If none found, returns.
                EditorGUI.LabelField(line, "Assign a GameVariables reference first.");
                line.y += lineHeight;
                EditorGUI.EndProperty();
                EditorGUI.indentLevel--;
                return;
            }

            // List and draw popup menu.
            if (variables != null && variables.variableBlocks != null && variables.variableBlocks.Count > 0)
            {
                string[] options = new string[variables.variableBlocks.Count];

                // Get currentIndex from base class.
                int currentIndex = targetVariableIndexProp.intValue;
                currentIndex = Mathf.Clamp(currentIndex, -1, variables.variableBlocks.Count - 1);

                // Get list variableBlocks from GameVariables.
                for (int i = 0; i < variables.variableBlocks.Count; i++)
                {
                    // Name options by block's variableName.
                    var block = variables.variableBlocks[i];
                    string name = (block != null) ? (string.IsNullOrEmpty(block.variableName) ? $"Element {i}" : block.variableName) : $"Element {i}";
                    options[i] = name;
                }

                // Draw popup menu and wait for selection.
                int newIndex = EditorGUI.Popup(line, "Target Variable", currentIndex, options);
                line.y += lineHeight;

                // Set selected index to base class.
                if (newIndex != targetVariableIndexProp.intValue)
                {
                    targetVariableIndexProp.intValue = newIndex;
                    property.serializedObject.ApplyModifiedProperties();
                }

                // Draw input field.
                if (currentIndex >= 0)
                {
                    Type dataType = variables.variableBlocks[currentIndex].GetValue().GetType();
                    DrawInputFields(line, dataType);
                }
            }
            else
            {
                // If variables is not found or no variables available.
                EditorGUI.LabelField(line, "No VariableBlocks found in this Variables object.");
                line.y += lineHeight;
            }

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
                height += lineHeight * expandedHeight;
            }
            return height;
        }

        private void DrawInputFields(Rect line, Type fieldType)
        {
            // Draw inputfield by current data type.
            GUIContent inputLabel = new GUIContent("Set value (" + fieldType.Name + ")");
            EditorGUILayout.Space();
            var inputProp =
                (fieldType == typeof(bool)) ? newValueBoolProp :
                (fieldType == typeof(int)) ? newValueIntProp :
                (fieldType == typeof(float)) ? newValueFloatProp :
                (fieldType == typeof(string)) ? newValueStringProp :
                (fieldType == typeof(Vector2)) ? newValueVector2Prop :
                (fieldType == typeof(Vector3)) ? newValueVector3Prop :
                null;

            if(inputProp!=null)
                EditorGUI.PropertyField(line, inputProp, inputLabel);
            else
                EditorGUI.LabelField(line, "Not compatiable data type.");
        }
    }
}

/*



            
            

*/