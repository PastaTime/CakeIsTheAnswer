using System;
using GoodFlower;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnKey : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private KeyCode key = KeyCode.Return;
    // Start is called before the first frame update
    
    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            _soundManager.PlaySoundEffect("ui_accept");
            SceneManager.LoadScene(SceneName);
        }
    }
}
