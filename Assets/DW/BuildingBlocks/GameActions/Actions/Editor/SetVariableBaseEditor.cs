using System;
using System.Reflection;
using DirtyWorks.GameBlocks;
using DirtyWorks.GameBlocks.Variables;
using UnityEngine;
using UnityEditor;

namespace DirtyWorks.GameBlocks.Editors
{
    public class SetVariableBaseEditor : PropertyDrawer
    {
        public SerializedProperty targetVariableIndexProp;
        public SerializedProperty newValueBoolProp;
        public SerializedProperty newValueIntProp;
        public SerializedProperty newValueFloatProp;
        public SerializedProperty newValueStringProp;
        public SerializedProperty newValueVector2Prop;
        public SerializedProperty newValueVector3Prop;

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
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float height = lineHeight;

            if (property.isExpanded)
            {
                // 1 for callByIndex, 1 for either index/name
                height += lineHeight;
            }
            return height;
        }

        public void DrawInputFields(Rect line, Type fieldType)
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

            if (inputProp != null)
                EditorGUI.PropertyField(line, inputProp, inputLabel);
            else
                EditorGUI.LabelField(line, "Not compatiable data type.");
        }
    }
}
