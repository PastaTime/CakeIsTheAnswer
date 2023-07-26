using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private int fontSize;
    [SerializeField] private int fontWidth;
    [SerializeField] private float typeSpeed = 0.0f;
    private TextMeshProUGUI _tmpText;
    public Manager _manager;
    public AudioSource audioSource;
    public List<AudioClip> keyClips;
    public AudioClip acceptClip;

    public int FontHeight
    {
        get => (int)_tmpText.fontSize;
        set => _tmpText.fontSize = value;
    }

    private int _fontWidth;
    public int FontWidth
    {
        get => _fontWidth;
        set
        {
            _fontWidth = value;
            Text = Text;
        }
    }
    public string Text
    {
        get => _tmpText.GetParsedText();
        set => _tmpText.text = $"<mspace={FontWidth}px>\n" + value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
        if (_manager == null)
        {
            _manager = FindObjectOfType<Manager>();
        }

        FontWidth = fontWidth;
        FontHeight = fontSize;
        if (filePath != null)
            Text = filePath.text;
        else
            StartCoroutine(TypeWrite( KeyCode.Return));

        if (_manager != null)
        {
            if (!_manager.wasRevealed)
            {
                if (_manager.state == GameState.DayStart && _manager.dayNumber != 2 && SceneManager.GetActiveScene().name != "GameStart")
                    _manager.PlaySong(_manager.days[_manager.dayNumber].dayStartSong);
                else if (_manager.state == GameState.DayEnd)
                    _manager.PlaySong(_manager.days[_manager.dayNumber].dayEndSong);
            }
            else
            {
                _manager.PlaySong(Songs.Song6);
            }
        }
    }

    private IEnumerator TypeWrite(KeyCode key)
    {
        var count = 0;
        var text = _manager.GetNarration();
        Debug.Log("TEXT:" + text);  
        var left = 0;
        var right = 0;
        while (right < text.Length)
        {
            if (_tmpText.isTextOverflowing && text[right] == '\n')
            {
                Text = text[left..right] + "\n\n<b>Press ENTER to Continue</b>";
                //Enter Key
                yield return WaitForKeyPress(key);
                _manager.totalContinuePresses += 1;
                Debug.Log("Total continues pressed:" + _manager.totalContinuePresses);
                
                if (_manager.totalContinuePresses == 26) // Detective enters shop
                {
                    _manager.audioSource.clip = _manager.songs[4];
                    _manager.audioSource.Play();
                }
                if (_manager.totalContinuePresses == 31) // Detective asks the question
                {
                    _manager.audioSource.clip = _manager.songs[6];
                    _manager.audioSource.Play();
                }

                audioSource.clip = acceptClip;
                audioSource.Play();
                left = right;
            }
            else if (text[right] == '\n')
            {
                count++;
                Debug.Log("New Line: " + count);

                if (count == 2)
                {
                    //DING
                }
                //DING
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (!audioSource.isPlaying || audioSource.time > 0.1f)
                {
                    audioSource.clip = keyClips[Random.Range(0, keyClips.Count)];
                    audioSource.Play();
                }

                count = 0;
            }


            right++;
            Text = text[left..right];
            yield return new WaitForSeconds(typeSpeed);
        }

        _manager.ReadyToChangeState = true;
    }
    
    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }
}