using UnityEngine;

public class ScoreManager
{
    public static int GetHighScore(int gameID)
    {
        return PlayerPrefs.GetInt($"HighScore_{gameID}", 0);
    }

    public static void SetHighScore(int gameID, int score)
    {
        PlayerPrefs.SetInt($"HighScore_{gameID}", score);
    }

    public static bool CheckAndSaveHighScore(int gameID, int scored)
    {
        int prevScore = GetHighScore(gameID);
        if (prevScore < scored)
        {
            SetHighScore(gameID, scored);
            return true;
        }

        return false;
    }
}
