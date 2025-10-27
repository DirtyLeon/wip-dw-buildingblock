using System;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    [SerializeReference]
    public TestUp[] testUps;
}
[Serializable]
public class TestUp
{
    public string[] str;
}
[Serializable]
public class TestUpInt : TestUp
{
    public int[] m_int;
}
[Serializable]
public class TestUpFloat : TestUp
{
    public float[] m_float;
}
[Serializable]
public class TestUpType : TestUp
{
    public enum Type1
    {
        None,
        Good
    }
    public Type1[] m_type;
}




