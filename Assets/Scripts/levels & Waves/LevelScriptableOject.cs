using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelScriptableOject", menuName = "Scriptable Objects/LevelScriptableOject")]
public class LevelScriptableOject : ScriptableObject
{
    public int StartGameBalance;
    public List<WaveScriptableObject> Waves;
}
