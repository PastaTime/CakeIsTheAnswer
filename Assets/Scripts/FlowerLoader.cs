using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class FlowerLoader : MonoBehaviour
{
    //private string folderPath = "Assets/Prefabs/Ruixin";
    public List<TextAsset> flowerImageTexts = new();
    [SerializeField] private Vector2 fontResolution;
    [SerializeField] private Vector2 windowSize;
    private TextMeshProUGUI _tmpText;
    private RectTransform _rectTransform;
    public List<string> txtFiles;
    string flower;
    private int index = 0;
    public int FontHeight
    {
        get => (int)_tmpText.fontSize;
        set => _tmpText.fontSize = value;
    }
    
    public int Files
    {
        get => txtFiles.Count;
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
    public string Flower
    {
        get => Path.GetFileNameWithoutExtension(flower);
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

        //LoadFile(txtFiles[0]); //example

        //ScanFolderForTxtFiles(folderPath);
        for (int i = 0; i < flowerImageTexts.Count; i++)
        {
            txtFiles.Add(flowerImageTexts[i].text);
        }
        LoadFile(index);
    }
    void ScanFolderForTxtFiles(string path)
    {
        //     txtFiles = DirectoryInfo(path, "*.txt");
        //txtFiles = Directory.GetFiles(folderPath, "*.txt");

    }
    public void LoadFile(int _index)
    {
        //StreamReader reader = new StreamReader(filePath);
        Text = txtFiles[_index];
        flower = Path.GetFileName(flowerImageTexts[_index].name);
    }
    public void Next(int dir)
    {
        //string text = File.ReadAllText(file);
        index = (index + dir) % txtFiles.Count;
        if (index < 0) { index = txtFiles.Count - 1; };
        LoadFile(index);
    }

    // public void Back()
    // {
    //     //string text = File.ReadAllText(file);
    //     if (index == 0) { index = txtFiles.Length - 1; }
    //     else { index--; }
    //     LoadFile(txtFiles[index]);
    // }
}
