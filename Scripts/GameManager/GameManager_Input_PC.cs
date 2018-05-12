using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Input_PC : GameManager
{
    void Update()
    {
        InputModeSwitchMachine();
    }

    public override void InputModeSwitchMachine()
    {
        switch (inputMode)
        {
            case InputMode.PC:
                MOVE_INPUT = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
                JUMP_INPUT = Input.GetKeyDown(KeyCode.Space);
                RUN_INPUT = Input.GetKey(KeyCode.LeftShift);

                SWING_START_INPUT = Input.GetMouseButtonDown(0);
                SWINGING_INPUT = Input.GetMouseButton(0);
                SWING_END_INPUT = Input.GetMouseButtonUp(0);

                BURST_START_INPUT = Input.GetMouseButtonDown(1);
                BURSTING_INPUT = Input.GetMouseButton(1);
                BURST_END_INPUT = Input.GetMouseButtonUp(1);

                // new
                SHOOT_INPUT = Input.GetMouseButtonDown(1);
                MELEE_INPUT = Input.GetMouseButtonDown(0);
                DASH_INPUT = Input.GetKeyDown(KeyCode.LeftShift);
                SWITCH_WEAPON_INPUT = Input.GetKeyUp(KeyCode.Alpha1);
                break;
        }
    }
}
