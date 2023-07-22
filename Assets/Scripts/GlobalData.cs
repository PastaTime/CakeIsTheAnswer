using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


public enum NarrationType 
{
    DayStart,
    DayEnd
}

// string sFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\Orders\Test.xml");  
// string sFilePath = Path.GetFullPath(sFile); 

public static class GlobalData
{
    public static Dictionary<string, string[]> allLines = new();
    public static List<string> desired;
    public static List<string> collected = new();

    public static int DayNumber { get; set; } = 1;
    // private static string NarrationFolder = @"..";
    //
    // public static string GetNarration(NarrationType type)
    // {
    // }


}

[Serializable]
public class GlobalData2
{
    public Dictionary<string, string[]> AllLines = new();
    public List<string> Desired = new();
    public List<string> Collected = new();
}