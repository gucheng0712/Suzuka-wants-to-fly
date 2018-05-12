using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViveControllerType
{
    Left,
    Right
}

public class GameManager_Input_Vive : GameManager
{
    public ViveControllerType controllerType;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        InputModeSwitchMachine();
    }

    public override void InputModeSwitchMachine()
    {
        if (inputMode == InputMode.VR)
        {
            switch (controllerType)
            {
                case ViveControllerType.Right:
                    Inputs();
                    break;
                case ViveControllerType.Left:
                    Inputs();
                    break;
            }
        }
    }

    void Inputs()
    {
        MOVE_INPUT = new Vector2(Controller.GetAxis().x, Controller.GetAxis().y).normalized;
        if (state == BATTLE_STATE)
        {
            SHOOT_INPUT = Controller.GetHairTriggerDown();
            MELEE_INPUT = SHOOT_INPUT;

            DASH_INPUT = Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            SWITCH_WEAPON_INPUT = Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
        }
        else
        {
            RUN_INPUT = true;

            SWING_START_INPUT = Controller.GetHairTriggerDown();
            SWINGING_INPUT = Controller.GetHairTrigger();
            SWING_END_INPUT = Controller.GetHairTriggerUp();


            BURST_START_INPUT = Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            BURSTING_INPUT = Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
            BURST_END_INPUT = Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);


            if (state != BURST_STATE)
            {
                JUMP_INPUT = Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            }
        }
    }
}
