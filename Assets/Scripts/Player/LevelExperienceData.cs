using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelExperienceData", menuName = "ScriptableObjects/LevelExperienceData", order = 1)]
public class LevelExperienceData : ScriptableObject
{
    [System.Serializable]
    public class LevelExperience
    {
        public int level;
        public int experienceRequired;
    }

    public List<LevelExperience> levelExperiences = new List<LevelExperience>();

    public int GetExperienceForLevel(int level)
    {
        LevelExperience levelExperience = levelExperiences.Find(le => le.level == level);
        return levelExperience != null ? levelExperience.experienceRequired : 0;
    }

    public void AddLevelExperience(int level, int experienceRequired)
    {
        LevelExperience newLevelExperience = new LevelExperience { level = level, experienceRequired = experienceRequired };
        levelExperiences.Add(newLevelExperience);
    }
}
