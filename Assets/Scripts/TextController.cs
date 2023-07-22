using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Text;
using static GlobalData;

public class textController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI frame;

    [SerializeField] private FlowerLoader textWindow;
    private int resolutionWidth;
    private float timer;
    private float cursorTimer;
    //private bool haltInput;
    private bool cursorOn = true;
    private float waitTime = 0.15f;
    private float cursorWaitTime = 0.5f;
    private string cursor = "_";
    private int cursorLength = 5;
    private Vector2Int cursorPos = new Vector2Int(0, 1);
    private int step = 4;

    private KeyCode right = KeyCode.RightArrow;
    private KeyCode down = KeyCode.DownArrow;
    private KeyCode left = KeyCode.LeftArrow;
    private KeyCode up = KeyCode.UpArrow;
    private KeyCode delete = KeyCode.Backspace;
    private KeyCode water = KeyCode.W;
    private KeyCode next = KeyCode.Q;
    private KeyCode back = KeyCode.E;


    private string[] originalLines;
    private string[] currentLines;
    void Start()
    {
        for (int i = 0; i < textWindow.Files; i++)
        { allLines.Add(null); }
        resolutionWidth = textWindow.WindowWidth;
        string text = frame.text;
        originalLines = text.Split('\n');
        for (int i = 0; i < originalLines.Length; i++)
        {
            string formatted = originalLines[i];
            while (formatted.Length < resolutionWidth)
            {
                formatted += " ";
            }
            originalLines[i] = formatted;
        }
        currentLines = (string[])originalLines.Clone();
        RefreshFrame();
    }

    // Update is called once per frame
    void Update()
    {
        //        if (!haltInput)
        //        {
        timer += Time.deltaTime;
        cursorTimer += Time.deltaTime;
        if (timer > waitTime)
        {
            if (Input.GetKey(down)) { TriggerDown(); timer = 0f; }
            else if (Input.GetKey(left)) { TriggerLeft(); timer = 0f; }
            else if (Input.GetKey(right)) { TriggerRight(); timer = 0f; }
            else if (Input.GetKey(up)) { TriggerUp(); timer = 0f; }
            else if (Input.GetKey(delete)) { TriggerDelete(); timer = 0f; }
            else if (Input.GetKey(water)) { TriggerWater(); timer = 0f; }
            else if (Input.GetKey(next)) { SaveImage(); LoadNewImage(-1); timer = 0f; }
            else if (Input.GetKey(back)) { SaveImage(); LoadNewImage(1); timer = 0f; }
            else { Flicker(); }
        }
        //        }
    }

    private void SaveImage()
    {
        allLines[textWindow.Index] = originalLines;
    }

    private void LoadNewImage(int dir)
    {
        textWindow.Next(dir);
        resolutionWidth = textWindow.WindowWidth;
        if (allLines[textWindow.Index] == null)
        {
            string text = frame.text;
            originalLines = text.Split('\n');
        }
        else
        {
            originalLines = allLines[textWindow.Index];
        }
        for (int i = 0; i < originalLines.Length; i++)
        {
            string formatted = originalLines[i];
            while (formatted.Length < resolutionWidth)
            {
                formatted += " ";
            }
            originalLines[i] = formatted;
        }
        currentLines = (string[])originalLines.Clone();
        RefreshFrame();

    }

    private void TriggerWater()
    {
        //currentLines[cursorPos.y] = currentLines[cursorPos.y][..cursorPos.x] + "water" + currentLines[cursorPos.y][(cursorPos.x + cursorLength)..];
    }

    private void Flicker()
    {
        if (cursorTimer > cursorWaitTime)
        {
            cursorOn = !cursorOn;
            RefreshFrame();
            cursorTimer = 0f;
        }
    }

    private void TriggerDown()
    {
        if (cursorPos.y < currentLines.Length - 1)
        {
            cursorPos.y += 1;
        }
        cursorOn = true;
        RefreshFrame();
    }

    private void TriggerLeft()
    {
        if (cursorPos.x > 0)
        {
            cursorPos.x -= step;
        }
        cursorOn = true;
        RefreshFrame();
    }

    private void TriggerRight()
    {
        if (cursorPos.x < resolutionWidth - 1 - cursorLength - step)
        { cursorPos.x += step; }
        cursorOn = true;
        RefreshFrame();
    }

    private void TriggerUp()
    {
        if (cursorPos.y > 1)
        {
            cursorPos.y -= 1;
        }
        cursorOn = true;
        RefreshFrame();
    }

    private void TriggerDelete()
    {
        if (cursorPos.y > 1)
        {
            cursorOn = true;
            originalLines[cursorPos.y] = originalLines[cursorPos.y][..cursorPos.x] + new StringBuilder().Insert(0, " ", cursorLength).ToString() + originalLines[cursorPos.y][(cursorPos.x + cursorLength)..];
            for (int i = 0; i < cursorLength; i++)
            {
                if (originalLines[cursorPos.y - 1][cursorPos.x + i].ToString() != " ")
                {
                    DFS(cursorPos.x + i, cursorPos.y - 1);
                    RefreshFrame();
                    return;
                }
            }
        }
    }

    private void DFS(int x, int y)
    {
        if (x < 0 || x >= resolutionWidth - 1 || y < 1 || y >= currentLines.Length - 1 || originalLines[y][x].ToString() == " ")
        {
            return;
        }
        else
        {
            originalLines[y] = originalLines[y][..x] + " " + originalLines[y][(x + 1)..];
            //transfer to other grid
            DFS(x - 1, y);
            DFS(x + 1, y);
            DFS(x, y - 1);
            DFS(x, y + 1);
        }

    }

    private void RefreshFrame()
    {
        currentLines = (string[])originalLines.Clone();
        if (cursorOn)
        {
            currentLines[cursorPos.y] = currentLines[cursorPos.y][..cursorPos.x] + new StringBuilder().Insert(0, cursor, cursorLength).ToString() + currentLines[cursorPos.y][(cursorPos.x + cursorLength)..];
        }
        else
        {
            currentLines[cursorPos.y] = currentLines[cursorPos.y][..cursorPos.x] + new StringBuilder().Insert(0, " ", cursorLength).ToString() + currentLines[cursorPos.y][(cursorPos.x + cursorLength)..];
            //currentLines[cursorPos.y] = currentLines[cursorPos.y][..resolutionWidth];
        }
        frame.text = string.Join("\n", currentLines);
        allLines[textWindow.Index] = originalLines;
    }
}
