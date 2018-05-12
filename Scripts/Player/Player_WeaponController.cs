using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeaponController : MonoBehaviour
{

    public GameObject[] weapons;
    Player playerScript;

    private void Awake()
    {
        playerScript = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (GameManager.state == GameManager.BATTLE_STATE)
        {
            weapons[GameManager.weaponIndex].SetActive(true);
            if (GameManager.SWITCH_WEAPON_INPUT)
            {
                ChangeWeapon();
            }
        }
        else
        {
            DisableAllWeapons();
        }
    }

    void ChangeWeapon()
    {
        playerScript.audioSource.PlayOneShot(playerScript.audioManager.weaponSwitchSound);
        weapons[GameManager.weaponIndex].SetActive(false);
        GameManager.weaponIndex = (GameManager.weaponIndex + 1) % weapons.Length;
        // weapons[currentWeaponIndex].Activate();
    }

    void DisableAllWeapons()
    {
        foreach (var weapon in weapons)
        {
            if (weapon.activeSelf == true)
                weapon.SetActive(false);
        }

    }
}
