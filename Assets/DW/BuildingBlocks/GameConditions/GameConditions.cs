using UnityEngine;

namespace DirtyWorks.GameBlocks.Conditions
{
    public class GameConditions : MonoBehaviour
    {
        // Has a "if" list.

        // Check "if" condition list one by one.

        // "False": action

        // "True": 

        public ConditionBlockBase condition = new ConditionBlockBase();

        public void SetMainValue()
        {
            condition.SetMainValueTarget();
        }

        public void SetCompareTo()
        {
            condition.SetCompareToTarget();
        }

        public void Compare()
        {
            condition.CompareResult();
        }
    }
}