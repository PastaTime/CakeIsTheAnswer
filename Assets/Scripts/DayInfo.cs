using System;
using System.Collections.Generic;
using UnityEngine;

public class DayInfo : MonoBehaviour
{
    public List<string> desired = new();
    public TextAsset dayStart;
    public List <TextAsset> dayEnd = new();

    public string GetDayEndByCount(int count)
    {
        switch (count)
        {
            case 3:
                return dayEnd[0].text;
            case 2:
            case 1:
                return dayEnd[1].text;
            case 0:
                return dayEnd[2].text;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
