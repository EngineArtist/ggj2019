using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Whether the spaceship animation / flying is active
    private bool progressActive;

    // HUD components
    // General HUD component
    public GameObject HUDcomponent;
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

    // Start is called before the first frame update
    void Start()
    {
        // Create the dictionaries / arraylists etc
        HUDobjects = new Dictionary<string, GameObject>();
        resources = new Dictionary<string, int>();
        vars = new Dictionary<string, double>();
        // Game is on by default
        progressActive = true;
        // Event trigger logic
        vars.Add("baseTimeToEvent", 6.0f);
        vars.Add("timeToEvent", 6.0f);
        vars.Add("eventSpeed", 1.0f);
        vars.Add("difficulty", 1.0f);
        vars.Add("distanceTraveled", 0.0f);
        // Create the HUD
        CreateHUD();
        // Set default values for the various resources
        resources.Add("hull", 100);
        resources.Add("energy", 100);
        resources.Add("population", 100);
        // Dynamically allocated array of objects in space
        spaceObjects = new List<GameObject>();
        // Game specific variables

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
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Hull";
        HUDobjects.Add("hull", tmp);
        // Energy
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Energy";
        HUDobjects.Add("energy", tmp);
        // Population
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Population";
        HUDobjects.Add("population", tmp);
        // Base HUD update
        updateHUD("hull", 100.0f);
        updateHUD("energy", 100.0f);
        updateHUD("population", 100.0f);
        // HUD COMPONENTS INTENDED FOR DEBUGGING
        // Time until next event
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.1f, 10));
        tmp.GetComponent<TextMesh>().text = "TimerToEvent " + vars["timeToEvent"];
        HUDobjects.Add("timertoevent", tmp);
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
            // Trigger an event
            if(vars["timeToEvent"] < 0) 
            {
                // Event logic
                TriggerTimeEvent();
                // Reset time to next event
                vars["timeToEvent"] = vars["baseTimeToEvent"];
            }
            // Logic for space progression
            // Keep a counter for distance traveled
            vars["distanceTraveled"] += vars["distanceTraveled"] * Time.deltaTime;
        } 
        // Otherwise in menus or in an event yet to be resolved
        else
        {
            // Event scripts and/or menu state machine

            // After event is resolved, resume gaming

        }
    }

    // Time triggered spontaneous events
    private void TriggerTimeEvent()
    {
       
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
