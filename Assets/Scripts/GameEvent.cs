using System.Collections;
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
    public int stress;
    // TODO: add result types
    public EventOption(string initialText, string resultText, int stress = 0)
    {
        this.initialText = initialText;
        this.stress = stress;
    }
}
