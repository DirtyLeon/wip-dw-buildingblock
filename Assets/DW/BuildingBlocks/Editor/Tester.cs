using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TestCube))]
public class TestEditor : Editor
{
    private ReorderableList list;
    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("testUps"),
                true, true, true, true)
                {
                    elementHeightCallback = ElementHeightCallback,
                    drawElementCallback = DrawElementCallback,
                    onAddDropdownCallback = OnAddDropdownCallback
                };
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();

    }
    public float ElementHeightCallback(int index)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);


        return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
    }
    public void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);


        EditorGUI.indentLevel++;
        // EditorGUI.LabelField(rect, list.serializedProperty.arrayElementType);


        EditorGUI.PropertyField(rect, element, new GUIContent(element.managedReferenceFullTypename), includeChildren: true);
        EditorGUI.indentLevel--;

    }
    public void OnAddDropdownCallback(Rect buttonRect, ReorderableList l)
    {
        var menu = new GenericMenu();
        var testup = TypeCache.GetTypesDerivedFrom<TestUp>();
        var prop = serializedObject.FindProperty("testUps");
        foreach (var test in testup)
        {
            menu.AddItem(new GUIContent(test.Name), false, () =>
            {
                // prop.arraySize += 1;
                var index = prop.arraySize;
                prop.InsertArrayElementAtIndex(index);
                var elementProp = prop.GetArrayElementAtIndex(index);
                elementProp.managedReferenceValue = Activator.CreateInstance(test);
                Debug.Log(elementProp.managedReferenceFullTypename);
                serializedObject.ApplyModifiedProperties();
            });
        }

        menu.DropDown(buttonRect);
        //var guids = AssetDatabase.FindAssets();
    }
}