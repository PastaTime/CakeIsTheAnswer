using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitOnKey : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.Return;
    
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Quit key pressed.");
            Application.Quit();
        }
    }
}
