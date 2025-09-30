using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class TemplateMonoBeheavior : MonoBehaviour
{
    // --- Member Variables and Properties ---
    // Declare variables here that need to persist across frames
    // (e.g., speed, references to other components, health, etc.)

    // --- Unity Message Functions (The Component Lifecycle) ---

    // **Initialization Phase**
    
    // Called when the script instance is first loaded. 
    // This happens even before 'Start'. It's often used for setting up **initial state**, 
    // getting **component references** on the same GameObject, or initializing manager scripts.
    void Awake()
    {
       
    }

    // Called right after 'Awake' and every time the GameObject is activated (becomes enabled).
    // Often used for **subscribing to events** (like input events or custom delegates).
    void OnEnable()
    {
        
    }

    // Called on the first frame that the script is enabled. It's only called once.
    // Use this for any **initial setup** that needs other components to be fully initialized 
    // (like setting object positions based on another object) or that depends on references 
    // set in the 'Awake' of other scripts.
    void Start()
    {
        
    }

    // **Update Phase (Game Loop)**
    void Update()
    {
       
    }

    // Called every fixed frame-rate frame (by default, 0.02 seconds or 50 times per second).
    // Use this for **physics calculations** and logic related to the Rigidbody component, 
    // as it runs in sync with the physics engine.
    void FixedUpdate()
    {
        
    }

    // Called every frame after 'Update' has finished.
    // Use this for **camera following** or any code that needs to run after all 
    // GameObjects have been processed in 'Update'.
    void LateUpdate()
    {
        
    }

    // Called when the behaviour becomes disabled or inactive (e.g., GameObject is deactivated).
    // The inverse of 'OnEnable'. Always use this to **unsubscribe from events** // to prevent memory leaks or calling code on a null object.
    void OnDisable()
    {
        
    }

    // Called when the script component is destroyed, e.g., when the scene is closed 
    // or when 'Destroy(gameObject)' is called.
    // Use for final **resource cleanup** that shouldn't happen on simple deactivation 
    // (like saving game data or cleaning up static references).
    void OnDestroy()
    {
        // Example: SaveGameData();
    }
}