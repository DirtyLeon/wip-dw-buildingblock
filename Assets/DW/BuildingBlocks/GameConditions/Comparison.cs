using System;
using UnityEngine;

namespace DirtyWorks.GameBlocks.Conditions
{
    public enum CompareMethod
    {
        Not,
        Equal,
        LessThan,
        LessEqual,
        MoreEqual,
        MoreThan
    }

    [System.Serializable]
    public class Comparision
    {
        public CompareMethod comparedMethod;

        public bool Compare(object itemA, object itemB, bool isInverted)
        {
            bool compareResult = false;
            Type compareType = itemA.GetType();

            if (compareType != itemB.GetType())
            {
                Debug.LogError("Comparision type doesn't match: " + itemA.GetType() + " to " + itemB.GetType());
                return false;
            }

            var a = Convert.ChangeType(itemA, compareType);
            var b = Convert.ChangeType(itemB, compareType);

            compareResult =
                (compareType == typeof(bool)) ? CompareBool((bool)a, (bool)b) :
                (compareType == typeof(string)) ? CompareString((string)a, (string)b) :
                (compareType == typeof(int)) ? CompareInt((int)a, (int)b) :
                (compareType == typeof(float)) ? CompareFloat((float)a, (float)b) :
                false;

            return (isInverted) ? !compareResult : compareResult;
        }

        private bool CompareBool(bool a, bool b)
        {
            var result =
                (comparedMethod == CompareMethod.Not) ? (a != b) :
                (comparedMethod == CompareMethod.Equal) ? (a == b) :
                false;

            return result;
        }

        private bool CompareString(string a, string b)
        {
            return
                (comparedMethod == CompareMethod.Not) ? (a != b) :
                (comparedMethod == CompareMethod.Equal) ? (a == b) :
                false;
        }

        private bool CompareInt(int a, int b)
        {
            return
                (comparedMethod == CompareMethod.Not) ? (a != b) :
                (comparedMethod == CompareMethod.Equal) ? (a == b) :
                (comparedMethod == CompareMethod.LessThan) ? (a < b) :
                (comparedMethod == CompareMethod.LessEqual) ? (a <= b) :
                (comparedMethod == CompareMethod.MoreEqual) ? (a >= b) :
                (comparedMethod == CompareMethod.MoreThan) ? (a > b) :
                false;
        }

        private bool CompareFloat(float a, float b)
        {
            return
                (comparedMethod == CompareMethod.Not) ? (a != b) :
                (comparedMethod == CompareMethod.Equal) ? (a == b) :
                (comparedMethod == CompareMethod.LessThan) ? (a < b) :
                (comparedMethod == CompareMethod.LessEqual) ? (a <= b) :
                (comparedMethod == CompareMethod.MoreEqual) ? (a >= b) :
                (comparedMethod == CompareMethod.MoreThan) ? (a > b) :
                false;
        }
    }
}
