using System;
using System.Net;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused = false;
    private InputAction pauseGameAction;
    private void Awake()
    {
        pauseGameAction = InputSystem.actions.FindAction("Pause");
    }

    private void OnEnable()
    {
        pauseGameAction.Enable();

        pauseGameAction.started += OnPause;
    }
    private void Update()
    {
       
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isGamePaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        GameEvents.current.TogglePause(false);

    }

    private void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        GameEvents.current.TogglePause(true);
    }
}