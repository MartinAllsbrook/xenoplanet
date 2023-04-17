using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private string[] instructions;
    // [SerializeField] private UnityEvent[] events;
    [SerializeField] private TextMeshProUGUI tutorialText;
    
    private int _tutorialStage = 0;

    // private void Start()
    // {
    //     StartStage(_tutorialStage);
    // }

    // private void StartStage(int stageNumber)
    // {
    //     // events[stageNumber].AddListener(AdvanceStage);
    // }

    private void AdvanceStage()
    {
        _tutorialStage++;
        tutorialText.text = instructions[_tutorialStage];
        // StartStage(_tutorialStage);
    }

    public void OnFirstMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        float magnitude = value.magnitude;
        if (magnitude > 0 && _tutorialStage == 0)
        {
            AdvanceStage();
        }
    }

    public void OnFirstLook(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        float magnitude = value.magnitude;
        if (magnitude > 0 && _tutorialStage == 1)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstJump(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 2)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstCrouch(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 3)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstSprint(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 4)
        {
            AdvanceStage();
        }
    }

    public void OnFirstCutTree()
    {
        if (_tutorialStage == 5)
        {
            AdvanceStage();
        }
    }

    public void OnFirstPickUpItem()
    {
        if (_tutorialStage == 6)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstOpenInventory(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 7)
        {
            AdvanceStage();
        }
    }

    public void OnFirstCraftArrow()
    {
        if (_tutorialStage == 8)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstSwapArrow(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 9)
        {
            AdvanceStage();
        }
    }
    
    public void OnFirstAim(InputAction.CallbackContext context)
    {
        if (context.started && _tutorialStage == 10)
        {
            AdvanceStage();
        }
    }

    public void OnFirstFire()
    {
        if (_tutorialStage == 11)
        {
            AdvanceStage();
        }
    }

    public void OnFirstOpenChest()
    {
        if (_tutorialStage == 12)
        {
            AdvanceStage();
        }
    }
}
