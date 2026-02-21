using System;
using System.Net;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject resumeButton;
    public GameObject quitButton;

    public Texture2D pauseCursor;
    public Vector2 hotspot = Vector2.zero;
    private bool isGamePaused = false;
    private InputAction pauseGameAction;
    private void Awake()
    {
        #if UNITY_EDITOR
            pauseGameAction = InputSystem.actions.FindAction("PauseInEditor");
        #else
            pauseGameAction = InputSystem.actions.FindAction("Pause");
        #endif
    }

    private void OnEnable()
    {
        pauseGameAction.Enable();

        pauseGameAction.started += OnPause;

        pauseUI.SetActive(false);
    }
    private void Start()
    {

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
        pauseUI.SetActive(false);

        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        GameEvents.current.TogglePause(true);
        pauseUI.SetActive(true);

        // Cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}