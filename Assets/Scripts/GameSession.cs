using UnityEngine;

public enum GameMode
{
    Levels,
    Endless
}

public enum EnemyType
{
    Light,
    Meduim,
    Heavy
}

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public GameMode selectedMode;
    public int levelIndex;


    private void Awake()
    {
        // If another instance exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the single instance
        Instance = this;

        // Make it persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    public void SetLevelMode()
    {
        Instance.selectedMode = GameMode.Levels;
    }

    public void SetEndlessMode()
    {
        Instance.selectedMode = GameMode.Endless;
    }

    public void SetLevelMode(int index)
    {
        levelIndex = index;
    }
}