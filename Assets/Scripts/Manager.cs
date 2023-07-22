using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    DayStart,
    Grafting,
    DayEnd
}
public class Manager : MonoBehaviour
{
    public List<DayInfo> days = new();

    public Dictionary<string, string[]> AllLines;
    public int dayNumber = 0;
    public List<string> desired;
    public List<string> collected;

    public GameState State { get; set; } = GameState.DayStart;

    public bool ReadyToChangeState { get; set; } = false;
    
    // Start is called before the first frame update
    void Awake() 
    {
        Debug.Log("MANAGER AWAKE");
        DontDestroyOnLoad(gameObject);
        var dayObjects = FindObjectsOfType<DayInfo>();
        foreach (var g in dayObjects)
        {
            Debug.Log(g.name);
        }
        days = dayObjects.OrderBy(x => x.name).ToList();
    }

    void Update()
    {
        if (!ReadyToChangeState)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateDay();
            ReadyToChangeState = false;
        }
    }

    public string GetNarration()
    {
        Debug.Log(days);
        var day = days[dayNumber];
        switch (State)
        {
            case GameState.DayStart:
                return day.dayStart.text;
            case GameState.DayEnd:
                return day.GetDayEndByCount(GetScore());
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public int GetScore()
    {
        var hashSet = new HashSet<string>(desired);
        return collected.Count(x => hashSet.Contains(x));
    }

    private void UpdateDay()
    {
        switch (State)
        {
            case GameState.DayStart:
                SceneManager.LoadScene("GraftingGame");
                State = GameState.Grafting;
                return;
            case GameState.Grafting:
                SceneManager.LoadScene("Narration");
                State = GameState.DayEnd;
                return;
            case GameState.DayEnd:
                SceneManager.LoadScene("Narration");
                dayNumber++;
                State = GameState.DayStart;
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
