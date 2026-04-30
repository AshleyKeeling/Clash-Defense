using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelScriptableOject", menuName = "Scriptable Objects/LevelScriptableOject")]
public class LevelScriptableOject : ScriptableObject
{
    public List<WaveScriptableObject> waves;
}
