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
            VariableBlock addedBlock;
            addedBlock =
                (_type == typeof(bool)) ? new VariableBool() :
                (_type == typeof(int)) ? new VariableInt() :
                (_type == typeof(float)) ? new VariableFloat() :
                (_type == typeof(string)) ? new VariableString() :
                (_type == typeof(Vector2)) ? new VariableVector2() :
                (_type == typeof(Vector3)) ? new VariableVector3() :
                null;

            if(addedBlock == null)
            {
                Debug.LogError("Non-compatible type: " + _type);
                return;
            }
            else
            {
                variableBlocks.Add(addedBlock);
            }
        }
    }
}
