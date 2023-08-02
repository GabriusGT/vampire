using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{
    public TMP_Text upgradeDescText, nameLevelText;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    PlayerController playerControllerScript;

    public void Start()
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        theWeapon.LevelUp();

        if (theWeapon.gameObject.activeSelf == true)
        {
            upgradeDescText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            nameLevelText.text = theWeapon.name + " - Lvl " + theWeapon.weaponLevel;
        } else
        {
            upgradeDescText.text = "Unlock " + theWeapon.name;
            nameLevelText.text = theWeapon.name;
        }

        weaponIcon.sprite = theWeapon.usedIcon;
        theWeapon.LevelDown();

        assignedWeapon = theWeapon;
    }

    public void SelectUpgrade()
    {
        if(assignedWeapon != null)
        {
            if (assignedWeapon.gameObject.activeSelf == true)
            {
                assignedWeapon.LevelUp();
                playerControllerScript.UpdateWeaponSlots();
            } else
            {
                PlayerController.instance.AddWeapon(assignedWeapon);
            }

            UIController.instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}