using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Whether the spaceship animation / flying is active
    private bool progressActive;
    private bool eventActivated;
    // Whether an event is currently on-going
    private GameEvent currentEvent;
    // General HUD component
    public GameObject HUDcomponent;
    // General event display screen
    public GameObject HUDevent;
    // Spaceship
    public GameObject spaceship;
    // Specific HUD components that are updated on the run
    public Dictionary<string, GameObject> HUDobjects;
    // Values for the resources
    public Dictionary<string, int> resources;
    // Other numeric variables useful in game logic
    public Dictionary<string, double> vars;
    // Objects in space
    public List<GameObject> spaceObjects;
    // Reference to the EventMaster
    private EventMaster em;
    // Start is called before the first frame update
    void Start()
    {
        // Create the dictionaries / arraylists etc
        HUDobjects = new Dictionary<string, GameObject>();
        resources = new Dictionary<string, int>();
        vars = new Dictionary<string, double>();
        // Game is on by default
        progressActive = true;
        eventActivated = false;
        // Event trigger logic
        vars.Add("baseTimeToEvent", 6.0f);
        vars.Add("timeToEvent", 6.0f);
        vars.Add("eventSpeed", 1.0f);
        vars.Add("difficulty", 1.0f);
        vars.Add("distanceTraveled", 0.0f);
        vars.Add("shipSpeed", 2.0f);
        // Create the HUD
        CreateHUD();
        // Set default values for the various resources
        resources.Add("hull", 100);
        resources.Add("energy", 100);
        resources.Add("population", 100);
        // Dynamically allocated array of objects in space
        spaceObjects = new List<GameObject>();
        // Game specific variables

        // Game event master script
        em = this.GetComponent<EventMaster>();
    }

    // Create an interface for the player
    private void CreateHUD()
    {
        GameObject tmp;
        // Create the player HUD
        // Use viewport to position the HUD elements; 
        // {0,0} is lower-left corner, {1,1} is top-right and the 3rd component is the depth
        // Hull 
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 8));
        tmp.GetComponent<TextMesh>().text = "Hull";
        HUDobjects.Add("hull", tmp);
        // Energy
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 1, 8));
        tmp.GetComponent<TextMesh>().text = "Energy";
        HUDobjects.Add("energy", tmp);
        // Population
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 1, 8));
        tmp.GetComponent<TextMesh>().text = "Population";
        HUDobjects.Add("population", tmp);
        // Base HUD update
        updateHUD("hull", 100.0f);
        updateHUD("energy", 100.0f);
        updateHUD("population", 100.0f);
        // HUD COMPONENTS INTENDED FOR DEBUGGING
        // Time until next event
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.1f, 8));
        HUDobjects.Add("timertoevent", tmp);
        // Distance traveled
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 0.1f, 8));
        HUDobjects.Add("distancetraveled", tmp);
    }

    // Update only a particular resource on the HUD
    private void updateHUD(string component)
    {
        switch (component)
        {
            case "hull":
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + resources[component] + "%";
                break;
            default:
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + (int) resources[component];
                break;
        }
    }

    // Update a text mesh displaying something on the HUD
    private void updateHUD(string component, double newvalue)
    {
        switch (component)
        {
            case "hull":
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + (int)newvalue + "%";
                break;
            default:
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + (int)newvalue;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Actively progressing along the 'trail'
        if (progressActive)
        {
            // Logic for generating new events in real time
            vars["timeToEvent"] -= vars["eventSpeed"] * Time.deltaTime;
            updateHUD("timertoevent", vars["timeToEvent"]);
            // When the progression is active, update the distance traveled in space
            vars["distanceTraveled"] += vars["shipSpeed"] * Time.deltaTime;
            // Logic for space progression
            // Keep a counter for distance traveled
            updateHUD("distancetraveled", vars["distanceTraveled"]);
            // Trigger an event
            if(vars["timeToEvent"] < 0) 
            {
                // Halt space progress
                progressActive = false;
                // Event logic
                TriggerTimeEvent();
                // Reset time to next event
                vars["timeToEvent"] = vars["baseTimeToEvent"];
            }
        } 
        // Otherwise in menus or in an event yet to be resolved
        else if(!eventActivated)
        {
            // Event scripts and/or menu state machine
            Debug.Log("Current event id activated: " + currentEvent.id);
            // After event is resolved, resume gaming
            eventActivated = true;
            GameObject tmp = GameObject.Instantiate(this.HUDevent);
            tmp.GetComponent<EventPad>().setGE(currentEvent);
        }
        else
        {

        }
    }

    public void ResolveEvent(Dictionary<string, int> result)
    {
        Debug.Log("Modified resources count: " + result.Count);
        foreach (KeyValuePair<string, int> kvp in result)
        {
            Debug.Log("Key: " + kvp.Key + " Val: " + kvp.Value);
            resources[kvp.Key] += kvp.Value;
            updateHUD(kvp.Key);
        }
        this.progressActive = true;
        this.eventActivated = false;
    }

    // Time triggered spontaneous events
    private void TriggerTimeEvent()
    {
        // Temporary solution is to just generate a random event to solve
        currentEvent = em.GetRandomEvent();
    }
}

public class HUDwrapper{
    private string name;
    private double val;

    public string Name { get => name; set => name = value; }
    public double Val { get => val; set => val = value; }

    // Create a string representation of the HUDcomponent
    public override string ToString()
    {
        return name + ": " + val;
    }
}
