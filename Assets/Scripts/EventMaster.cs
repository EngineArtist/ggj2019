using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SimpleJSON;

public class EventMaster : MonoBehaviour
{
    public GameEvent currentEvent = null;
    public List<GameEvent> allEvents;
    public System.Random rand;
    private string eventDataFilename = "/Text/events.json";

    // Start is called before the first frame update
    void Start()
    {
        this.allEvents = new List<GameEvent>();
        this.loadEventData();
        this.rand = new System.Random();
    }

    GameEvent GetRandomEvent()
    {
        int index = this.rand.Next(allEvents.Count);
        return this.allEvents[index];
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    List<GameEvent> InitializeEvents()
    {
        return null;
    }

    private void loadEventData()
    {
        string filePath = Application.dataPath + eventDataFilename;
        string jsonText;
        if (File.Exists(filePath))
        {
            jsonText = File.ReadAllText(filePath);
            var N = SimpleJSON.JSON.Parse(jsonText);
            var events = N["event"];

            foreach (JSONObject gameEvent in events)
            {
                string mainMessage = gameEvent["description"];
                GameEvent ge = new GameEvent(mainMessage);

                foreach (JSONObject eventOption in gameEvent["option"])
                {
                    Debug.Log(eventOption);
                    string optionMsg = eventOption["description"].Value;
                    Debug.Log(optionMsg);
                    string resultMsg = eventOption["result"].Value;
                    Debug.Log(resultMsg);
                    Dictionary<string, int> reqDict = new Dictionary<string, int>();
                    Dictionary<string, int> resultDict = new Dictionary<string, int>();

                    foreach (KeyValuePair<string, SimpleJSON.JSONNode> kvp in eventOption["requirement"])
                    {
                        string key = kvp.Key;
                        int value = kvp.Value;
                        
                        reqDict[key] = value;
                        Debug.Log(key + ": " + reqDict[key]);
                    }

                    foreach (KeyValuePair<string, SimpleJSON.JSONNode> kvp in eventOption["resource"])
                    {
                        string key = kvp.Key;
                        int value = kvp.Value;

                        resultDict[key] = value;
                        Debug.Log(key + ": " + resultDict[key]);
                    }

                    EventOption opt = new EventOption(optionMsg, resultMsg, reqDict, resultDict);
                    ge.options.Add(opt);
                }

                this.allEvents.Add(ge);
            }
        }
    }
}
