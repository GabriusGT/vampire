using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> stats;
    public int weaponLevel;

    [HideInInspector]
    public bool statsUpdated;

    public Sprite usedIcon;

    public Sprite icon, icon2;

    public void LevelUp()
    {
        if(weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
            UpdateIcon();
            statsUpdated = true;
            if(weaponLevel >= stats.Count - 1)
            {
                PlayerController.instance.fullyLevelledWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }
    }

    public void LevelDown()
    {
        weaponLevel--;
        UpdateIcon();
        statsUpdated = true;
    }

    void UpdateIcon()
    {
        if (weaponLevel < 5)
        {
            usedIcon = icon;
        }
        else
        {
            usedIcon = icon2;
        }
    }
}

[System.Serializable]
public class WeaponStats
{
    public float speed, damage, range, timeBetweenAttacks, amount, duration;
    public string upgradeText;
}