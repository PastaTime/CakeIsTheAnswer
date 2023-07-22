using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class TextLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private int fontSize;
    [SerializeField] private int fontWidth;
    [SerializeField] private float typeSpeed = 0.1f;
    private TextMeshProUGUI _tmpText;
    private RectTransform _rectTransform;
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
        _rectTransform = GetComponent<RectTransform>();

        FontWidth = fontWidth;
        FontHeight = fontSize;

        StartCoroutine(TypeWrite(filePath.text, KeyCode.Return));
    }
    
    private IEnumerator TypeWrite(string text, KeyCode key)
    {
        var left = 0;
        var right = 0;
        while (right < text.Length)
        {
            if (_tmpText.isTextOverflowing && text[right] == '\n')
            {
                Text = text[left..right] + "\n\nPress ENTER to Continue";
                yield return waitForKeyPress(key);
                left = right;
            }
            else if (text[right] == '\n')
                yield return new WaitForSeconds(0.5f);

                
            right++;
            Text = text[left..right];
            yield return new WaitForSeconds(typeSpeed);
        }
    }
    
    private IEnumerator waitForKeyPress(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }
}