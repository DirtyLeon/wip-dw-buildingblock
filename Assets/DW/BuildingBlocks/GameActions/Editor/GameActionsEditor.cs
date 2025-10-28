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
            float propertyHeight = EditorGUI.GetPropertyHeight(element, true);

            // Add extra padding.
            return propertyHeight + 10f;
            //return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
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
            //float padding = 4f;
            float buttonWidth = 50f;
            Rect fieldRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 4f, rect.height);
            Rect runButtonRect = new Rect(rect.x + rect.width - 70, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);
            Rect removeButtonRect = new Rect(rect.x + rect.width - 15, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);
            // Draw the property field
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(fieldRect, element, new GUIContent(label, EditorGUIUtility.IconContent("PlayButton").image), includeChildren: true);
            EditorGUI.indentLevel--;

            using (new EditorGUI.DisabledScope(obj == null))
            {
                if(obj is IGameBlock)
                {
                    if (GUI.Button(runButtonRect, "Run"))
                    {
                        if (Application.isPlaying)
                        {
                            var block = obj as IGameBlock;
                            block?.Run();
                        }
                    }
                }
                
            }

            using (new EditorGUI.DisabledScope(obj == null))
            {
                if (GUI.Button(removeButtonRect, EditorGUIUtility.IconContent("TreeEditor.Trash"), GUIStyle.none))
                {
                    // Delay the removal until after GUI events to avoid modification errors
                    EditorApplication.delayCall += () =>
                    {
                        _list.serializedProperty.DeleteArrayElementAtIndex(index);
                        _list.serializedProperty.serializedObject.ApplyModifiedProperties();
                    };
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
