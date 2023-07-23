using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnKey : MonoBehaviour
{
    [SerializeField] private string SceneName;

    [SerializeField] private KeyCode key = KeyCode.Return;
    // Start is called before the first frame update
    
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Menus/MenuConfirm");
            SceneManager.LoadScene(SceneName);

        }
    }
}
