using System;
using UnityEngine;
using System.Linq;

namespace DirtyWorks.GameBlocks
{
    [System.Serializable]
    //[CreateAssetMenu(fileName = "ActionBlock", menuName = "Scriptable Objects/ActionBlock")]
    public class ActionBlock : IBlockInfo
    {
        public string name = "Debug.Log";
        public bool enabled = true;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
#if UNITY_EDITOR
                //UnityEditor.EditorUtility.SetObjectEnabled(this, enabled);
#endif
            }
        }

        public string GetName() => name;

        /*
        public virtual void Reset()
        {
            this.name = GetType().Name;

#if UNITY_EDITOR
            var p = UnityEditor.AssetDatabase.GetAssetPath(this);
            var names = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(p).Where(a => a).Select(a => a.name).ToList();
            names.Remove(GetType().Name);
            this.name = UnityEditor.ObjectNames.GetUniqueName(names.ToArray(), GetType().Name);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        */
    }

}
