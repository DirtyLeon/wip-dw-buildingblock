using UnityEditor;
using UnityEngine;

namespace DirtyWorks.GameBlocks
{
    [CustomPropertyDrawer(typeof(BaseSceneAction), true)]
    public class BaseSceneActionEditor : PropertyDrawer
    {
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

            var loadSceneModeProp = property.FindPropertyRelative("additiveScene");
            var sceneByIndexProp = property.FindPropertyRelative("sceneByIndex");
            var sceneIndexProp = property.FindPropertyRelative("sceneIndex");
            var sceneNameProp = property.FindPropertyRelative("sceneName");

            // Layout constants
            //float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Rect rect = new Rect(
                position.x,
                position.y + lineHeight,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            // Draw callByIndex
            EditorGUI.PropertyField(rect, sceneByIndexProp);
            rect.y += lineHeight;

            EditorGUI.PropertyField(rect, loadSceneModeProp);
            rect.y += lineHeight;

            // Draw either objectIndex or objectName
            if (sceneByIndexProp.boolValue)
                EditorGUI.PropertyField(rect, sceneIndexProp);
            else
                EditorGUI.PropertyField(rect, sceneNameProp);


            EditorGUI.EndProperty();
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // 1 line for foldout
            float height = lineHeight;

            // Only add lines if expanded
            if (property.isExpanded)
            {
                // 1 for callByIndex, 1 for either index/name
                height += lineHeight * 3;
            }

            return height;
        }
    }
}
