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
        EnemyWaveSystem.OnWaveStarted += HandleOnWaveStart;
        EnemyWaveSystem.OnLevelEnded += HandleOnLevelEnded;
        PlayerBaseHealth.OnPlayerBaseDestroyed += HandleOnPlayerBaseDestroyed;
    }

    private void OnDisable()
    {
        EnemyWaveSystem.OnWaveStarted -= HandleOnWaveStart;
        EnemyWaveSystem.OnLevelEnded -= HandleOnLevelEnded;
        PlayerBaseHealth.OnPlayerBaseDestroyed -= HandleOnPlayerBaseDestroyed;
    }

    private void HandleOnWaveStart(StartWaveData data)
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
