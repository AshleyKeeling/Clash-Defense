using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip WaveStartSFX;
    public AudioClip LevelCompleteSFX;
    public AudioClip gameOverSFX;

    private void PlayClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void OnEnable()
    {
        EnemyWaveSystem.OnStartWave += HandleOnWaveStart;
        EnemyWaveSystem.OnLevelEnded += HandleOnLevelEnded;
        PlayerBaseHealth.OnPlayerBaseDestroyed += HandleOnPlayerBaseDestroyed;
    }

    private void OnDisable()
    {
        EnemyWaveSystem.OnStartWave -= HandleOnWaveStart;
        EnemyWaveSystem.OnLevelEnded -= HandleOnLevelEnded;
        PlayerBaseHealth.OnPlayerBaseDestroyed -= HandleOnPlayerBaseDestroyed;
    }

    private void HandleOnWaveStart(GameMode gameMode, int levelNum, int waveNum)
    {
        PlayClip(WaveStartSFX);
    }
    private void HandleOnLevelEnded(LevelEndData data)
    {
        PlayClip(LevelCompleteSFX);
    }

    private void HandleOnPlayerBaseDestroyed()
    {
        PlayClip(gameOverSFX);
    }
}
