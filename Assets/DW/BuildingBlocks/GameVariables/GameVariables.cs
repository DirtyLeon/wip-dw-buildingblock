using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DirtyWorks.GameBlocks.Variables
{
    public class GameVariables : MonoBehaviour
    {
        public static GameVariables Instance;
        public bool SetAsGlobalStatic = false;

        [SerializeReference]
        public List<VariableBlock> variableBlocks;

        [SerializeField, HideInInspector]
        public string variableGUID;

        [SerializeField]
        private string displayGUID;

        private void Awake()
        {
            if (SetAsGlobalStatic)
            {
                if(Instance != null && Instance!= this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        private void Reset()
        {
            GenerateNewGUID();
        }

        private void OnValidate()
        {
            displayGUID = variableGUID;
        }

        public object GetVariable(string variableName)
        {
            var variable = variableBlocks.Find(x => x.variableName == variableName);
            if (variable == null)
            {
                Debug.LogError("No variable found: " + variableName);
                return null;
            }
            else
                return variable;
        }

        public void SetVariable(string variableName, object value)
        {
            var variable = variableBlocks.Find(x => x.variableName == variableName);
            if (variable == null)
            {
                Debug.LogError("No variable found: " + variableName);
                return;
            }
            else
            {
                variable.SetValue(value);
            }
        }

        [ExecuteInEditMode]
        public void GenerateNewGUID()
        {
            variableGUID = Guid.NewGuid().ToString();
            displayGUID = variableGUID;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void AddVariable(Type _type)
        {
            if (_type == typeof(bool))
                variableBlocks.Add(new VariableBool());
            else if (_type == typeof(int))
                variableBlocks.Add(new VariableInt());
            else if (_type == typeof(float))
                variableBlocks.Add(new VariableFloat());
            else if (_type == typeof(string))
                variableBlocks.Add(new VariableString());
            else if (_type == typeof(Vector2))
                variableBlocks.Add(new VariableVector2());
            else if (_type == typeof(Vector3))
                variableBlocks.Add(new VariableVector3());
            else
                Debug.LogError("Non-compatible type: " + _type);
        }
    }
}
