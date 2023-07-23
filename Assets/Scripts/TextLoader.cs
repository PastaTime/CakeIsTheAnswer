using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private int fontSize;
    [SerializeField] private int fontWidth;
    [SerializeField] private float typeSpeed = 0.1f;
    private TextMeshProUGUI _tmpText;
    private Manager _manager;

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
        _manager = FindObjectOfType<Manager>();

        FontWidth = fontWidth;
        FontHeight = fontSize;
        if (filePath != null)
            Text = filePath.text;
        else
            StartCoroutine(TypeWrite( KeyCode.Return));

        if (_manager.state == GameState.DayStart)
            _manager.PlaySong(_manager.days[_manager.dayNumber].dayStartSong);
        else if (_manager.state == GameState.DayEnd)
            _manager.PlaySong(_manager.days[_manager.dayNumber].dayEndSong);

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
                Text = text[left..right] + "\n\nPress ENTER to Continue";
                yield return WaitForKeyPress(key);
                //Enter Key
                FMODUnity.RuntimeManager.PlayOneShot("event:/Menus/MenuConfirm");
                left = right;
            }
            else if (text[right] == '\n')
            {
                count++;
                Debug.Log("New Line: " + count);

                if (count == 2)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/TypeWriterDing");
                }
                //DING
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
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