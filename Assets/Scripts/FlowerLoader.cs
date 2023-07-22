using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class FlowerLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private Vector2 fontResolution;
    [SerializeField] private Vector2 windowSize;
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
    public int WindowWidth
    {
        get => (int)windowSize.x;
    }

    public int WindowHeight
    {
        get => (int)windowSize.y;
    }
    public int AsciiRows
    {
        get => Mathf.RoundToInt(_rectTransform.rect.width / FontWidth);
        set => _rectTransform.sizeDelta = new Vector2(FontWidth * value, _rectTransform.rect.height);
    }

    public int AsciiColumns
    {
        get => Mathf.RoundToInt(_rectTransform.rect.height / FontHeight / 1.15f);
        set => _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, value * FontHeight * 1.15f);
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

        FontWidth = (int)fontResolution.x;
        FontHeight = (int)fontResolution.y;

        AsciiRows = (int)windowSize.x;
        AsciiColumns = (int)windowSize.y;

        LoadFile(filePath); //example
    }

    public void LoadFile(TextAsset file)
    {
        Text = file.text;
    }
}
