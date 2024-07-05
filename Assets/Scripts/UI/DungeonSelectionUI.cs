using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonSelectionUI : MonoBehaviour
{
    public TMP_Dropdown dungeonLevelDropdown;
    public Button startDungeonButton;
    public DungeonLevel[] dungeonLevels;

    private void Start()
    {
        PopulateDungeonLevelDropdown();
        startDungeonButton.onClick.AddListener(OnStartDungeonButtonClicked);
    }

    private void PopulateDungeonLevelDropdown()
    {
        if (dungeonLevelDropdown == null)
        {
            Debug.LogError("DungeonLevelDropdown is not assigned.");
            return;
        }

        dungeonLevelDropdown.options.Clear();
        foreach (var level in dungeonLevels)
        {
            dungeonLevelDropdown.options.Add(new TMP_Dropdown.OptionData(level.level.ToString()));
        }
    }

    private void OnStartDungeonButtonClicked()
    {
        if (dungeonLevelDropdown == null)
        {
            Debug.LogError("DungeonLevelDropdown is not assigned.");
            return;
        }

        int selectedLevelIndex = dungeonLevelDropdown.value;

        if (dungeonLevels == null || selectedLevelIndex >= dungeonLevels.Length)
        {
            Debug.LogError("Invalid dungeon level selected.");
            return;
        }

        DungeonLevel selectedLevel = dungeonLevels[selectedLevelIndex];

        // Check if GameManager instance is available
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found. Ensure the GameManager script is attached to a GameObject in the scene and it is initialized.");
            return;
        }

        GameManager.Instance.SetSelectedDungeonLevel(selectedLevel);

        // Start the dungeon scene
        GameManager.Instance.StartDungeon();
    }
}
