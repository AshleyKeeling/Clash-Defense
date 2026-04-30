using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "Scriptable Objects/WaveScriptableObject")]
public class WaveScriptableObject : ScriptableObject
{
    public int WaveDuration;


    public List<EnemyType> SpawnOrder;
}
