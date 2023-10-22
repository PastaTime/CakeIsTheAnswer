using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLoader : MonoBehaviour
{
    [SerializeField] private TextAsset filePath;
    [SerializeField] private int fontSize;
    [SerializeField] private int fontWidth;
    private TextMeshProUGUI _tmpText;
    public Manager _manager;

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
    }
}