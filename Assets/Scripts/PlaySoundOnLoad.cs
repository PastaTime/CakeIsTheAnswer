using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoodFlower;

public class PlaySoundOnLoad : MonoBehaviour
{

    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private string _songName;

    void Awake()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(LatePlay());
    }

    private IEnumerator LatePlay()
    {
        yield return new WaitForEndOfFrame();
        _soundManager.PlayBackgroundMusic(_songName);
    }
}
