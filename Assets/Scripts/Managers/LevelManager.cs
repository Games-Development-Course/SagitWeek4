using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Prefabs")]
    public GameObject level1Prefab;
    public GameObject level2Prefab;

    [Header("Spawn Point")]
    public Transform bottleSpawnPoint;

    [Header("Managers")]
    public BottleManager bottleManager;

    public int currentLevel = 1;
    public int shotsUsed = 0;

    private GameObject currentBottleSet;

    // ================= LOAD LEVEL =================
    public void LoadLevel()
    {
        if (currentBottleSet != null)
            Destroy(currentBottleSet);

        GameObject prefab = (currentLevel == 1) ? level1Prefab : level2Prefab;

        currentBottleSet = Instantiate(
            prefab,
            bottleSpawnPoint.position,
            bottleSpawnPoint.rotation
        );

        bottleManager.RegisterBottles(currentBottleSet);

        shotsUsed = 0;

        UIManager.Instance.SetLevel(currentLevel);
    }

    // ================= RECORD SHOT =================
    public void RegisterThrow()
    {
        shotsUsed++;
    }

    // ================= CHECK ATTEMPTS ==============
    public bool HasMoreShots()
    {
        return shotsUsed < 2;
    }

    // ================= LEVEL CHECK =================
    public bool IsLastLevel()
    {
        return currentLevel == 2;
    }

    public void AdvanceLevel()
    {
        currentLevel++;
    }
}
