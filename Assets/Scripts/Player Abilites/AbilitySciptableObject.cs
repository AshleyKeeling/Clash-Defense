using UnityEngine;

[CreateAssetMenu(fileName = "AbiltySciptableObject", menuName = "Scriptable Objects/AbiltySciptableObject")]
public class AbilitySciptableObject : ScriptableObject
{
    public string name;
    public int cost;
    public float duration;
    public Sprite image;
}
