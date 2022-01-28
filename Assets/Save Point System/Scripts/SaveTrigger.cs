using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveData();
            NotificationManager.Instance.AddNotification("Checkpoint Reached!", 1.5f, NotificationColors.Positive);
            gameObject.SetActive(false);
        }
    }

    private void SaveData()
    {
        if (SavePointSystem.LastRoomSaved == LevelManager.mCurrRoomIndex)
            return;

        GameManager.Instance.SaveGame();
        SavePointSystem.SavedStats = true;
        SavePointSystem.LastRoomSaved = LevelManager.mCurrRoomIndex;
        SavePointSystem.Health = GameManager.Instance.mPlayerHealth.CurrentHealth;
        SavePointSystem.MaxHealth = GameManager.Instance.mPlayerHealth.MaxHealth;
        SavePointSystem.Gold = CurrencyManager.Instance.CoinBalance;
        SavePointSystem.CurrentLumes = LumeInventory.GetCurrentLumes();
        SavePointSystem.MaxLumes = LumeInventory.GetMaxLumes();
        SavePointSystem.UnlockedLumes = LumeInventory.GetLumesUnlocked();
        SavePointSystem.NumLumesUnlocked = LumeInventory.NumLumesUnlocked;
/*        SavePointSystem.UnlockedLumes = LumeInventory.GetLumesUnlocked();
        SavePointSystem.NumUnlockedLumes = LumeInventory.NumLumesUnlocked;*/
    }
}
