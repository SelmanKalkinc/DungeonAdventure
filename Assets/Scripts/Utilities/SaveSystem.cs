using UnityEngine;

public static class SaveSystem
{
    public static int LoadHighestCompletedDungeon()
    {
        if (PlayerPrefs.HasKey("HighestCompletedDungeon"))
        {
            return PlayerPrefs.GetInt("HighestCompletedDungeon");
        }
        else
        {
            return 0; // Default value if no save data is found
        }
    }

    public static void SaveHighestCompletedDungeon(int highestCompletedDungeon)
    {
        PlayerPrefs.SetInt("HighestCompletedDungeon", highestCompletedDungeon);
        PlayerPrefs.Save(); // Ensure data is saved
    }
}
