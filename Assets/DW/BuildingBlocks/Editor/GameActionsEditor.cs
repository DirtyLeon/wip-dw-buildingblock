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
        private ReorderableList _list;

        
        private void OnEnable()
        {
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

            return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
        }

        public void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _list.serializedProperty.GetArrayElementAtIndex(index);

            var obj = element.managedReferenceValue;
            var label = obj.GetType().Name;


            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(rect, element, new GUIContent(label), includeChildren: true);
            EditorGUI.indentLevel--;
        }

        public void OnAddDropdownCallback(Rect buttonRect, ReorderableList l)
        {
            var menu = new GenericMenu();
            var actions = TypeCache.GetTypesDerivedFrom<ActionBlock>();
            var prop = serializedObject.FindProperty("actionBlocks");
            foreach (var action in actions)
            {
                menu.AddItem(new GUIContent(action.Name), false, () =>
                {
                    // prop.arraySize += 1;
                    var index = prop.arraySize;
                    prop.InsertArrayElementAtIndex(index);
                    var elementProp = prop.GetArrayElementAtIndex(index);
                    elementProp.managedReferenceValue = Activator.CreateInstance(action);
                    Debug.Log(elementProp.managedReferenceFullTypename);
                    serializedObject.ApplyModifiedProperties();
                });
            }

            menu.DropDown(buttonRect);
            //var guids = AssetDatabase.FindAssets();
        }
    }
}
