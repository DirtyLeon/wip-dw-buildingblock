using UnityEngine;

public class TestSubject : MonoBehaviour
{
    public string playerName = "Mike";
    public float health = 20f;
    public int gold = 10;
    public int fame = 20;
    public bool isWalking = false;
    public Vector2 animState;
    public Vector3 position;

    public void SetHealth(float value) => health = value;
}
