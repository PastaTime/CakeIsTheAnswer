using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevealEnd : MonoBehaviour
{ 
    void Start()
    {
        Manager manager = FindObjectOfType<Manager>();
        if (manager != null)
        {
            manager.wasRevealed = true;
            manager.songs[0] = manager.songs[5];
            manager.PlaySong(Songs.Song6);
            manager.dayNumber = 0;
        }
        SceneManager.LoadScene("GameEnd");
    }
    
}
