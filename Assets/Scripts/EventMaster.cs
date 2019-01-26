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
                string id = gameEvent["id"];
                string mainMessage = gameEvent["description"];
                
                GameEvent ge = new GameEvent(id, mainMessage);

                foreach (JSONObject eventOption in gameEvent["option"])
                {
                    string optionId = eventOption["id"].Value;
                    string optionMsg = eventOption["description"].Value;
                    string resultMsg = eventOption["result"].Value;
                    Dictionary<string, int> reqDict = new Dictionary<string, int>();
                    Dictionary<string, int> resultDict = new Dictionary<string, int>();

                    foreach (KeyValuePair<string, SimpleJSON.JSONNode> kvp in eventOption["requirement"])
                    {
                        string key = kvp.Key;
                        int value = kvp.Value;
                        
                        reqDict[key] = value;
                    }

                    foreach (KeyValuePair<string, SimpleJSON.JSONNode> kvp in eventOption["resource"])
                    {
                        string key = kvp.Key;
                        int value = kvp.Value;

                        resultDict[key] = value;
                    }

                    EventOption opt = new EventOption(optionId, optionMsg, resultMsg, reqDict, resultDict);
                    ge.options.Add(opt);
                }

                this.allEvents.Add(ge);
            }
        }
    }
}
