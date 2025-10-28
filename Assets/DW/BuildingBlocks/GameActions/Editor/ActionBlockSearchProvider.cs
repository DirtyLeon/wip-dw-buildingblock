using DirtyWorks.GameBlocks;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActionBlockSearchProvider : ScriptableObject, ISearchWindowProvider
{
    private SerializedProperty _targetProperty;

    public static void Open(Rect buttonRect, SerializedProperty targetProperty)
    {
        var provider = CreateInstance<ActionBlockSearchProvider>();
        provider._targetProperty = targetProperty;

        // Open at button rect (drop-down style)
        var screenPos = GUIUtility.GUIToScreenPoint(new Vector2(buttonRect.x, buttonRect.yMax));
        SearchWindow.Open(new SearchWindowContext(screenPos), provider);
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Add Action Block"), 0) // Title of the menu
        };

        // Dynamically list all ActionBlock-derived types
        var types = TypeCache.GetTypesDerivedFrom<ActionBlock>();

        // Organize by category
        var categorized = new Dictionary<string, List<Type>>();

        foreach (var type in types)
        {
            if (type.IsAbstract)
                continue;

            string category = "Uncategorized";
            var attr = type.GetCustomAttribute<ActionBlockAttribute>();
            if (attr != null)
                category = attr.Category;

            if (!categorized.ContainsKey(category))
                categorized[category] = new List<Type>();
            categorized[category].Add(type);
        }

        foreach (var kvp in categorized)
        {
            tree.Add(new SearchTreeGroupEntry(new GUIContent(kvp.Key, EditorGUIUtility.IconContent("Folder Icon").image)) { level = 1 });

            foreach (var type in kvp.Value)
            {
                var imgKey = "cs Script Icon";
                if (kvp.Key == "Debug")
                    imgKey = "console.infoicon";

                tree.Add(new SearchTreeEntry(new GUIContent(type.Name, EditorGUIUtility.IconContent(imgKey).image))
                {
                    level = 2,
                    userData = type
                });
            }
        }

        return tree;
    }

    
    public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
    {
        if (_targetProperty == null || entry.userData is not Type type)
            return false;

        int index = _targetProperty.arraySize;
        _targetProperty.InsertArrayElementAtIndex(index);
        var element = _targetProperty.GetArrayElementAtIndex(index);
        element.managedReferenceValue = Activator.CreateInstance(type);

        _targetProperty.serializedObject.ApplyModifiedProperties();

        return true;
    }
    
}
