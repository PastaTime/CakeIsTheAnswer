using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Text;
using GoodFlower;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI frame;
    
    [SerializeField] private FlowerLoader textWindow;
    [SerializeField] private Manager _manager;
    [SerializeField] private SoundManager _soundManager;

    // Sounds
    public AudioSource audioSource;
    public List<AudioClip> forwardClips;
    public List<AudioClip> backwardClips;
    public List<AudioClip> scissorClips;
    
    private Dictionary<string, string[]> AllLines = new();
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
    private KeyCode next = KeyCode.Q;
    private KeyCode back = KeyCode.E;
    
    private int bottomBorder = 3;
    private string[] originalLines;
    private string[] currentLines;
    private bool haltInput = false;
    
    void Start()
    {
        _manager = FindObjectOfType<Manager>();
        _soundManager = FindObjectOfType<SoundManager>();
        _manager.collected.Clear();
        
        Debug.Log("FOUND: " +  _manager);
        // for (int i = 0; i < textWindow.Files; i++)
        // { allLines.Add(null); }
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
        if (haltInput) return;
        
        timer += Time.deltaTime;
        cursorTimer += Time.deltaTime;
            
        if (!(timer > waitTime)) return;
            
        if (Input.GetKey(down)) { TriggerDown(); timer = 0f; }
        else if (Input.GetKey(left)) { TriggerLeft(); timer = 0f; }
        else if (Input.GetKey(right)) { TriggerRight(); timer = 0f; }
        else if (Input.GetKey(up)) { TriggerUp(); timer = 0f; }
        else if (Input.GetKey(delete)) { TriggerDelete(); timer = 0f; }
        else if (Input.GetKey(next))
        {
            SaveImage(); 
            LoadNewImage(-1); 
            _soundManager.PlaySoundEffect("menu_forward"); 
            timer = 0f;
        }
        else if (Input.GetKey(back))
        {
            SaveImage(); 
            LoadNewImage(1);
            _soundManager.PlaySoundEffect("menu_back"); 
            timer = 0f;
        }
        else { Flicker(); }
    }
    
    private void SaveImage()
    {
        
        AllLines[textWindow.Flower] = originalLines;
    }
    
    private void LoadNewImage(int dir)
    {
        textWindow.Next(dir);
        if (dir >= 0)
        {
            audioSource.clip = forwardClips[Random.Range(0, forwardClips.Count)];
        }
        else
        {
            audioSource.clip = backwardClips[Random.Range(0, backwardClips.Count)];
        }
        audioSource.Play();
        resolutionWidth = textWindow.WindowWidth;
        if (!AllLines.ContainsKey(textWindow.Flower))
        {
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
        }
        else
        {
            originalLines = AllLines[textWindow.Flower];
        }
    
        currentLines = (string[])originalLines.Clone();
        RefreshFrame();
    
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
        if (cursorPos.y < currentLines.Length - 3 - bottomBorder)
        {
            cursorPos.y += 1;
        }
        cursorOn = true;
        _soundManager.PlaySoundEffect("minigame_movement");
        RefreshFrame();
    }
    
    private void TriggerLeft()
    {
        if (cursorPos.x > 0)
        {
            cursorPos.x -= step;
        }
        cursorOn = true;
        _soundManager.PlaySoundEffect("minigame_movement");
        RefreshFrame();
    }
    
    private void TriggerRight()
    {
        if (cursorPos.x < resolutionWidth - 1 - cursorLength - step)
        { 
            cursorPos.x += step;
        }
        cursorOn = true;
        _soundManager.PlaySoundEffect("minigame_movement");
        RefreshFrame();
    }
    
    private void TriggerUp()
    {
        if (cursorPos.y > 1)
        {
            cursorPos.y -= 1;
        }
        cursorOn = true;
        _soundManager.PlaySoundEffect("minigame_movement");
        RefreshFrame();
    }
    
    private void TriggerDelete()
    {
        if (cursorPos.y > 1 && !_manager.collected.Contains(textWindow.Flower))
        {
            cursorOn = true;
            originalLines[cursorPos.y] = originalLines[cursorPos.y][..cursorPos.x] + new StringBuilder().Insert(0, " ", cursorLength).ToString() + originalLines[cursorPos.y][(cursorPos.x + cursorLength)..];
            DFS2(cursorPos.x - 1, cursorPos.y);
            DFS2(cursorPos.x + cursorLength, cursorPos.y);
            for (var i = 0; i < cursorLength; i++)
            {
                if (originalLines[cursorPos.y - 1][cursorPos.x + i].ToString() == " ") continue;
                
                DFS(cursorPos.x + i, cursorPos.y - 1);
                _manager.collected.Add(textWindow.Flower);
                    
                RefreshFrame();
                haltInput = true;
                //add
                AddMutation();
                StartCoroutine(PauseCoroutine());
                //RefreshFrame();
                //remove
                haltInput = false;
                audioSource.clip = scissorClips[Random.Range(0,scissorClips.Count)];
                audioSource.Play();
                if (_manager.collected.Count == 3)
                {
                    SceneManager.LoadScene("Narration");
                }
                    
                break;

            }
        }
    }
    
    private void AddMutation()
    {
        List<char> chars = new() { 'G', 'R', 'O', 'W', 'T', 'H' };
        for (var i = 0; i < 10; i++)
        {
            var randomY = Random.Range(0, currentLines.Length - 2);
            var randomX = Random.Range(0, resolutionWidth - 2);
            var char_ = chars[Random.Range(0, chars.Count)];
            currentLines[randomY] = currentLines[randomY][..randomX] + char_ + currentLines[randomY][(randomX + 1)..];
        }
        frame.text = string.Join("\n", currentLines);
        
        _soundManager.PlaySoundEffect("scissor");
    }
    IEnumerator PauseCoroutine()
    {
        yield return new WaitForSeconds(2f);
        RefreshFrame();
    }
    
    private void DFS(int x, int y)
    {
        if (x < 0 || x >= resolutionWidth - 1 || y < 1 || y > currentLines.Length - 1 - bottomBorder || originalLines[y][x].ToString() == " ")
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
    private void DFS2(int x, int y)
    {
        if (x < 0 || x >= resolutionWidth - 1 || y < 1 || y >= currentLines.Length - 1 - bottomBorder || originalLines[y][x].ToString() == " ")
        {
            return;
        }
        else
        {
            originalLines[y] = originalLines[y][..x] + " " + originalLines[y][(x + 1)..];
            //transfer to other grid
            DFS2(x - 1, y);
            DFS2(x + 1, y);
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
        if (_manager.collected != null) { currentLines[^1] = "Collected Grafts: " + string.Join(", ", _manager.collected); }
        frame.text = string.Join("\n", currentLines);
        AllLines[textWindow.Flower] = originalLines;
        //go to scene if 3 grafts collected
    }
}
