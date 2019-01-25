using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventMaster : MonoBehaviour
{
    public GameEvent currentEvent = null;
    public List<GameEvent> allEvents;
    public System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        this.allEvents = new List<GameEvent>();
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
}
