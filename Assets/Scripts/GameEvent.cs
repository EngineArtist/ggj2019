﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEvent
{
    public string mainMessage;
    public List<EventOption> options;
    
    public GameEvent(string mainMessage)
    {
        this.mainMessage = mainMessage;
        this.options = new List<EventOption>();
    }
    
}

[System.Serializable]
public class EventOption
{
    public string initialText;
    public string resultText;
    public Dictionary<string, int> reqDict;
    public Dictionary<string, int> resultDict;

    public EventOption(string initialText, string resultText, Dictionary<string, int> reqDict, Dictionary<string, int> resultDict)
    {
        this.initialText = initialText;
        this.resultText = resultText;
        this.reqDict = reqDict;
        this.resultDict = resultDict;
    }
}
