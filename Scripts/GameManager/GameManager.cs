using UnityEngine;

public enum InputMode
{
    PC,
    VR
}

public abstract class GameManager : MonoBehaviour
{
    public InputMode inputMode;

    //States
    public static int state;
    public const int NORMAL_STATE = 0;
    public const int BATTLE_STATE = 1;
    public const int SWING_STATE = 2;
    public const int BURST_STATE = 3;
    public const int WALLWALK_STATE = 4;


    //DamageInFo
    public static float PLAYER_Bullet_DM = 20f;
    public static float PLAYER_HIDDENBLADE_DMG = 10f;
    public static float GHOST_ENERGYBALL_DMG = 30f;

    //Input--Movement
    public static Vector2 MOVE_INPUT;
    public static bool JUMP_INPUT;
    public static bool RUN_INPUT;
    //Input-Swing
    public static bool SWING_START_INPUT;
    public static bool SWINGING_INPUT;
    public static bool SWING_END_INPUT;
    //Inut--Burst
    public static bool BURST_START_INPUT;
    public static bool BURSTING_INPUT;
    public static bool BURST_END_INPUT;
    //Input--Abilities
    public static bool SHOOT_INPUT;
    public static bool MELEE_INPUT;
    public static bool DASH_INPUT;

    // Input--SwitchWeapon
    public static bool SWITCH_WEAPON_INPUT;

    //WeaponIndex
    public static int weaponIndex;
    public const int PISTOL_INDEX = 1;
    public const int HIDDENBLADE_INDEX = 0;

    //WeaponMode
    public virtual void AnimationStateMachine()
    {
        //
    }
    public virtual void InputModeSwitchMachine()
    {
        //
    }

}
