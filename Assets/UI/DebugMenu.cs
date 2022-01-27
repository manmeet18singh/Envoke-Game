using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    GameObject mDebugMenu = null;

 /*   private void Awake()
    {
        InputManager.controls.Debug.OpenDebugMenu.performed += ctx => ToggleDebugMenu();
    }*/

    private void ToggleDebugMenu()
    {
        mDebugMenu.SetActive(!mDebugMenu.activeInHierarchy);
    }

    public void NextRoomButton()
    {
        KillAllEnemiesButton();
        GameManager.Instance.ChangeRoom();
    }

    public void DebugRoomButton()
    {
        KillAllEnemiesButton();
        LevelManager.mCurrRoomIndex = 8;
        GameManager.Instance.ChangeRoom();
        Envoke.SceneManager.Instance.LoadScene(2);
    }

    public void KillAllEnemiesButton()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        SpawnerBase[] spawners = FindObjectsOfType<SpawnerBase>();

        BossHealth bossHealth = FindObjectOfType<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.Death();
            Destroy(bossHealth.gameObject);
        }

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null)
                enemy.Death();
        }

        foreach (SpawnerBase spawner in spawners)
        {
            if (spawner != null)
                Destroy(spawner.gameObject);
        }
    }

    public void UnlockAllLumes()
    {
        for (int i = 0; i < LumeInventory.NumLumeTypes; ++i)
        {
            LumeInventory.UnlockLume(i);
            SpellEvents.Instance.UnlockedLume(i);
            LumeInventory.SetLumeAmounts(i, 2, 2);
        }
    }

    public void ToggleImmortalityButton()
    {
        GameManager.Instance.mPlayerHealth.enabled = !GameManager.Instance.mPlayerHealth.enabled;
    }

    public void FullHealButton()
    {
        GameManager.Instance.mPlayerHealth.PercentHeal(100);
    }

    public void RefillLumesButton()
    {
        LumeInventory.TryAddLume(0, 100000);
        LumeInventory.TryAddLume(1, 100000);
        LumeInventory.TryAddLume(2, 100000);
    }

    public void IncreaseLumeCap()
    {
        LumeInventory.IncreaseMaxCapacity(0, 5);
        LumeInventory.IncreaseMaxCapacity(1, 5);
        LumeInventory.IncreaseMaxCapacity(2, 5);
        LumeInventory.TryAddLume(0, 100000);
        LumeInventory.TryAddLume(1, 100000);
        LumeInventory.TryAddLume(2, 100000);
    }

    public void InfiniteLumesButton()
    {
        LumeInventory.IncreaseMaxCapacity(0, 10000);
        LumeInventory.IncreaseMaxCapacity(1, 10000);
        LumeInventory.IncreaseMaxCapacity(2, 10000);
        LumeInventory.TryAddLume(0, 100000);
        LumeInventory.TryAddLume(1, 100000);
        LumeInventory.TryAddLume(2, 100000);
    }
}
