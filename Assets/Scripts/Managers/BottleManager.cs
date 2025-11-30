// ==================== BottleManager.cs ====================
using System.Linq;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    private Bottle[] bottles;
    public Bottle[] Bottles => bottles;

    // נקרא בכל פעם שטוענים פריפאב חדש של פינים
    public void RegisterBottles(GameObject bottleRoot)
    {
        bottles = bottleRoot.GetComponentsInChildren<Bottle>(true);
    }

    public int CountFallen()
    {
        return bottles.Count(b => b.IsFallen());
    }

    public void RemoveFallen()
    {
        foreach (var bottle in bottles)
        {
            if (bottle.IsFallen())
                bottle.gameObject.SetActive(false);
        }
    }

    public bool AllCleared()
    {
        return bottles.All(b => !b.gameObject.activeSelf);
    }
}
