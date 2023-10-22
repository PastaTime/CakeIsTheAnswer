using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Songs
{
    Song1,
    Song2,
    Song3,
    Song4,
    Song5,
    Song6,
    Song7,
    Grafting
}

public enum GameState
{
    DayStart,
    DayEnd
}
public class Manager : MonoBehaviour
{
    public List<DayInfo> days = new();


    public int dayNumber = -1;
    public List<string> desired = new();
    public List<string> collected = new();

    public GameState state = GameState.DayEnd;
    
    // Start is called before the first frame update
    void Awake() 
    {
        DontDestroyOnLoad(gameObject);
        var dayObjects = FindObjectsOfType<DayInfo>();
        days = dayObjects.OrderBy(x => x.name).ToList();
    }

    public string GetNarration()
    {
        var day = days[dayNumber];
        return state switch
        {
            GameState.DayStart => day.dayStart.text,
            GameState.DayEnd => day.GetDayEndByCount(GetScore()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public int GetScore()
    {
        var hashSet = new HashSet<string>(desired);
        return collected.Count(x => hashSet.Contains(x));
    }

    public void TransitionState()
    {
        switch (state)
        {
            case GameState.DayStart:
                state = GameState.DayEnd;
                break;
            case GameState.DayEnd:
                state = GameState.DayStart;
                dayNumber++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ResetState()
    {
        state = GameState.DayEnd;
        dayNumber = -1;
    }
}
