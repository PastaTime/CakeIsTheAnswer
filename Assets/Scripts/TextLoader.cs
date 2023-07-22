using TMPro;
using UnityEngine;

public class TextLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private int fontSize;
    [SerializeField] private int fontWidth;
    private TextMeshProUGUI _tmpText;
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

        FontWidth = fontWidth;
        FontHeight = fontSize;

        LoadFile(filePath); //example
    }

    public void LoadFile(TextAsset file)
    {
        Text = file.text;
    }
}