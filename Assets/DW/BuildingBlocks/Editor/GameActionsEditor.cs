using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DirtyWorks.GameBlocks
{
    [CustomEditor(typeof(GameActions))]
    public class GameActionsEditor : Editor
    {
        private SerializedProperty executeOnEnable, executeOnStart;
        private ReorderableList _list;
        
        private void OnEnable()
        {
            executeOnEnable = serializedObject.FindProperty("ExecuteOnEnable");
            executeOnStart = serializedObject.FindProperty("ExecuteOnStart");
            _list = 
                new ReorderableList(serializedObject, serializedObject.FindProperty("actionBlocks"),true, true, true, true)
                {
                    drawHeaderCallback = DrawListHeader,
                    elementHeightCallback = ElementHeightCallback,
                    drawElementCallback = DrawElementCallback,
                    onAddDropdownCallback = OnAddDropdownCallback
                };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(executeOnEnable);
            EditorGUILayout.PropertyField(executeOnStart);
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

        }

        private void DrawListHeader(Rect rect)
        {
            GUI.Label(rect, "Action Blocks");
        }

        public float ElementHeightCallback(int index)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);

            //return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
            float propertyHeight = EditorGUI.GetPropertyHeight(element, true);

            // add extra padding
            return propertyHeight + 10f;
        }

        public void DrawElementCallback_backup(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);

            var obj = element.managedReferenceValue;
            var label = obj.GetType().Name;

            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(rect, element, new GUIContent(label), includeChildren: true);
            EditorGUI.indentLevel--;
        }

        public void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var obj = element.managedReferenceValue;
            var label = obj != null ? obj.GetType().Name : "<Null>";

            // Calculate positions
            float buttonWidth = 50f;
            Rect fieldRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 4f, rect.height);
            Rect buttonRect = new Rect(rect.x + rect.width - buttonWidth, rect.y + 1f, buttonWidth, EditorGUIUtility.singleLineHeight);

            // Draw the property field
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(fieldRect, element, new GUIContent(label, EditorGUIUtility.IconContent("PlayButton").image), includeChildren: true);
            EditorGUI.indentLevel--;

            if (!(obj is IGameBlock))
                return;

            // Draw "Run" button
            using (new EditorGUI.DisabledScope(obj == null))
            {
                if (GUI.Button(buttonRect, "Run"))
                {
                    if (obj is IGameBlock block)
                    {
                        if (Application.isPlaying)
                            block.Run();
                        else
                            Debug.LogWarning("You can only run blocks in Play Mode.");
                    }
                    else
                    {
                        Debug.LogWarning($"{label} does not implement IGameBlock.");
                    }
                }
            }
        }

        public void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
        {
            var prop = serializedObject.FindProperty("actionBlocks");
            ActionBlockSearchProvider.Open(buttonRect, prop);
        }
    }
}
