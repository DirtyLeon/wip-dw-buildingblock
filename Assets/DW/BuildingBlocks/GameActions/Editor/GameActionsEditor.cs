using System;
using System.Linq;
using System.Collections.Generic;
using DirtyWorks.GameBlocks.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

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
            GameActions gameActions = (GameActions)target;
            serializedObject.Update();
            EditorGUILayout.PropertyField(executeOnEnable);
            EditorGUILayout.PropertyField(executeOnStart);
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            if(GUILayout.Button("Execute Actions"))
            {
                gameActions.ExecuteList();
            }
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

            // Calculate positions
            float buttonWidth = 50f;
            Rect runButtonRect = new Rect(rect.x + rect.width - 70, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);
            Rect removeButtonRect = new Rect(rect.x + rect.width - 15, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            // Rect for checkbox
            Rect checkboxRect = new Rect(rect.x + 4, rect.y, 27f, EditorGUIUtility.singleLineHeight);

            // Draw label.
            BlockDictionary.BlockName.TryGetValue(obj?.GetType().Name, out var tryGetName);
            var label = tryGetName ?? obj?.GetType().Name;
            var labelRect = new Rect(rect.x + 36, rect.y, rect.width - 18 - buttonWidth * 2 - 4f, rect.height);

            // Draw icon.
            BlockDictionary.AttributeIcon.TryGetValue((obj.GetType().GetCustomAttribute<ActionBlockAttribute>().Category), out var tryGetAttr);
            var icon = EditorGUIUtility.IconContent(tryGetAttr ?? "PlayButton").image;
            var iconRect = new Rect(rect.x, rect.y, 16, 16);
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

            // Draw the property field
            EditorGUI.indentLevel++;
            
            SerializedProperty enabledProp = element.FindPropertyRelative("enabled");
            if (enabledProp != null)
            {
                enabledProp.boolValue = EditorGUI.Toggle(checkboxRect, enabledProp.boolValue);
            }
            else
            {
                // If no property exists, draw an independent checkbox (e.g., for quick control)
                bool temp = EditorGUI.Toggle(checkboxRect, false);
            }

            EditorGUI.PropertyField(labelRect, element, new GUIContent(label), includeChildren: true);
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

                if (GUI.Button(removeButtonRect, EditorGUIUtility.IconContent("TreeEditor.Trash"), GUIStyle.none))
                {
                    // Delay the removal until after GUI events to avoid modification errors
                    EditorApplication.delayCall += () =>
                    {
                        _list.serializedProperty.DeleteArrayElementAtIndex(index);
                        _list.serializedProperty.serializedObject.ApplyModifiedProperties();
                    };
                }

                if (Event.current.type == EventType.Repaint)
                {
                    var lineRect = new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1);
                    EditorGUI.DrawRect(lineRect, new Color(0.3f, 0.3f, 0.3f)); // subtle gray line
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
