using System;
using System.Reflection;
using DirtyWorks.GameBlocks;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI;

namespace DirtyWorks.GameBlocks.Editors
{
    [CustomPropertyDrawer(typeof(SetGameVariable))]
    public class SetGameVariableEditor : PropertyDrawer
    {
        private const int expandedHeight = 4;
        //private int targetVariableIndex = -1;
        //private int currentIndex = -1;

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

            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);

            var gameVariablesProp = property.FindPropertyRelative("gameVariables");
            //var targetVariableProp = property.FindPropertyRelative("targetVariable");
            var targetVariableIndexProp = property.FindPropertyRelative("targetVariableIndex");
            var newValueBoolProp = property.FindPropertyRelative("newValueBool");
            var newValueIntProp = property.FindPropertyRelative("newValueInt");
            var newValueFloatProp = property.FindPropertyRelative("newValueFloat");
            var newValueStringProp = property.FindPropertyRelative("newValueString");
            var newValueVector2Prop = property.FindPropertyRelative("newValueVector2");
            var newValueVector3Prop = property.FindPropertyRelative("newValueVector3");

            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Rect line = new Rect(
                position.x,
                position.y + lineHeight,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            // Draw GameVariables field.
            EditorGUI.PropertyField(line, gameVariablesProp);
            line.y += lineHeight;

            // Draw Variable Select dropdown.
            if(gameVariablesProp.objectReferenceValue != null)
            {
                var variables = gameVariablesProp.objectReferenceValue as GameVariables;
                
                if(variables!=null && variables.variableBlocks!=null && variables.variableBlocks.Count > 0)
                {
                    string[] options = new string[variables.variableBlocks.Count];

                    for (int i = 0; i < variables.variableBlocks.Count; i++)
                    {
                        var block = variables.variableBlocks[i];
                        string name = block != null ? (string.IsNullOrEmpty(block.variableName) ? $"Element {i}" : block.variableName) : $"Element {i}";
                        options[i] = name;
                    }

                    int currentIndex = targetVariableIndexProp.intValue;
                    currentIndex = Mathf.Clamp(currentIndex, -1, variables.variableBlocks.Count - 1);

                    int newIndex = EditorGUI.Popup(line, "Target Variable", currentIndex, options);
                    line.y += lineHeight;

                    if (newIndex != targetVariableIndexProp.intValue)
                    {
                        targetVariableIndexProp.intValue = newIndex;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    // Draw input field.
                    if (currentIndex != -1)
                    {
                        var fieldType = variables.variableBlocks[currentIndex].GetValue().GetType();
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
                    }
                }
                else
                {
                    EditorGUI.LabelField(line, "No VariableBlocks found in this Variables object.");
                    line.y += lineHeight;
                }
            }
            else
            {
                EditorGUI.LabelField(line, "Assign a GameVariables reference first.");
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
    }
}

/*



            
            

*/