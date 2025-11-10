using System;
using DirtyWorks.GameBlocks.Utils;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DirtyWorks.GameBlocks.Variables
{
    [CustomEditor(typeof(GameVariables))]
    public class GameVariablesEditor : Editor
    {
        private SerializedProperty guid, isGlobalStatic;
        private ReorderableList _list;

        private void OnEnable()
        {
            guid = serializedObject.FindProperty("displayGUID");
            isGlobalStatic = serializedObject.FindProperty("SetAsGlobalStatic");

            _list =
                new ReorderableList(serializedObject, serializedObject.FindProperty("variableBlocks"), true, true, true, true)
                {
                    drawHeaderCallback = DrawListHeader,
                    elementHeightCallback = ElementHeightCallback,
                    drawElementCallback = DrawElementCallback,
                    onAddDropdownCallback = OnAddDropdownCallback
                };
        }

        public override void OnInspectorGUI()
        {
            GameVariables gameActions = (GameVariables)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(isGlobalStatic);
            EditorGUILayout.Space(10);
            _list.DoLayoutList();
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(guid);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawListHeader(Rect rect)
        {
            GUI.Label(rect, "Variables");
        }

        public float ElementHeightCallback(int index)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
            float propertyHeight = EditorGUI.GetPropertyHeight(element, true);

            // Add extra padding.
            return propertyHeight + 10f;
        }

        public void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);
            var obj = element.managedReferenceValue;

            // Calculate positions
            float buttonWidth = 50f;
            Rect removeButtonRect = new Rect(rect.x + rect.width - 15, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            // Draw label.
            //BlockDictionary.BlockName.TryGetValue(obj?.GetType().Name, out var tryGetName);
            //var label = tryGetName ?? obj?.GetType().Name;
            var label = obj?.GetType().Name;
            if (obj is VariableBlock block)
            {
                var nameField = (block.variableName == "") ? obj.GetType().Name : block.variableName;
                var valueField = block.GetValue();
                label = nameField + " = " + valueField;
            }

            var labelRect = new Rect(rect.x + 36 + 10, rect.y, rect.width - 18 - buttonWidth * 2, rect.height);

            // Draw icon.

            // Draw the property field
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(labelRect, element, new GUIContent(label), includeChildren: true);
            EditorGUI.indentLevel--;

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

                if (Event.current.type == EventType.Repaint)
                {
                    var lineRect = new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1);
                    EditorGUI.DrawRect(lineRect, new Color(0.3f, 0.3f, 0.3f)); // subtle gray line
                }
            }
        }

        public void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
        {
            var prop = serializedObject.FindProperty("variableBlocks");
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Add Bool"), false, () => AddNewVariable(typeof(bool)));
            menu.AddItem(new GUIContent("Add Integer"), false, () => AddNewVariable(typeof(int)));
            menu.AddItem(new GUIContent("Add Float"), false, () => AddNewVariable(typeof(float)));
            menu.AddItem(new GUIContent("Add String"), false, () => AddNewVariable(typeof(string)));
            menu.AddItem(new GUIContent("Add Vector2"), false, () => AddNewVariable(typeof(Vector2)));
            menu.AddItem(new GUIContent("Add Vector3"), false, () => AddNewVariable(typeof(Vector3)));

            menu.ShowAsContext();
        }

        private void AddNewVariable(Type _type)
        {
            GameVariables gameActions = (GameVariables)target;
            gameActions.AddVariable(_type);
        }
    }
}
