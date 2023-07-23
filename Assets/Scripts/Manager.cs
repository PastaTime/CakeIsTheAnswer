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


    public int dayNumber = 0;
    public List<string> desired = new();
    public List<string> collected = new();

    public GameState state = GameState.DayStart;
    
    public bool ReadyToChangeState { get; set; } = false;
    
    // Start is called before the first frame update
    void Awake() 
    {
        Debug.Log("AWAKEN");
        DontDestroyOnLoad(gameObject);
        var dayObjects = FindObjectsOfType<DayInfo>();
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
        var day = days[dayNumber];
        switch (state)
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

    public void UpdateDay()
    {
        print("Updating ... State: " + state + " Day Number: " + dayNumber);
        switch (state)
        {
            case GameState.DayStart:
                state = GameState.Grafting;
                SceneManager.LoadScene("GraftingGame");
                print("Grafting State: " + state + " Day Number: " + dayNumber);
                return;
            case GameState.Grafting:
                desired = days[dayNumber].desired;
                state = GameState.DayEnd;
                SceneManager.LoadScene("Narration");
                print("DayEnd ... State: " + state + " Day Number: " + dayNumber);
                return;
            case GameState.DayEnd:
                dayNumber++;
                state = GameState.DayStart;
                SceneManager.LoadScene("Narration");
                print("Done ... State: " + state + " Day Number: " + dayNumber);
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
