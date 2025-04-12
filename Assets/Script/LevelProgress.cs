using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    private static HashSet<int> unlockedLevels = new HashSet<int> { 1 }; // Level 1 is always unlocked

    public static void UnlockLevel(int level)
    {
        unlockedLevels.Add(level);
    }

    public static bool IsLevelUnlocked(int level)
    {
        return unlockedLevels.Contains(level);
    }

    public static void ResetProgress()
    {
        unlockedLevels.Clear();
        unlockedLevels.Add(1);
    }
}
