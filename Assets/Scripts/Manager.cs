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
}

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


    public static FMOD.Studio.EventInstance song1;
    public static FMOD.Studio.EventInstance song2;
    public static FMOD.Studio.EventInstance song3;
    public static FMOD.Studio.EventInstance song4;
    public static FMOD.Studio.EventInstance song5;
    public static FMOD.Studio.EventInstance song6;
    public static FMOD.Studio.EventInstance song7;

    public Songs CurrentSong = Songs.Song1;


    public bool ReadyToChangeState { get; set; } = false;

    private void Start()
    {
       song1 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Goodflower120bpm");
       song2 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Goodflower120bpm");
       song3 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Goodflower140bpm");
       song4 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/A Peculiar Customer");
       song5 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/A Peculiar Customer");
       song6 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/A Peculiar Customer");
       song7 = FMODUnity.RuntimeManager.CreateInstance("event:/Music/TheRitual");
    }

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

    public void PlaySong(Songs song)
    {
        switch(CurrentSong)
        {
            case Songs.Song1:
                song1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song2:
                song2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song3:
                song3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song4:
                song4.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song5:
                song5.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song6:
                song6.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case Songs.Song7:
                song7.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
        }

        CurrentSong = song;

        switch (song)
        {
            case Songs.Song1:
                song1.start();
                break;
            case Songs.Song2:
                song2.start();
                break;
            case Songs.Song3:
                song3.start();
                break;
            case Songs.Song4:
                song4.start();
                break;
            case Songs.Song5:
                song5.start();
                break;
            case Songs.Song6:
                song6.start();
                break;
            case Songs.Song7:
                song7.start();
                break;
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
