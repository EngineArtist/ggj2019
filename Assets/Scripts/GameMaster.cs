using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Whether the spaceship animation / flying is active
    private bool progressActive;
    private bool eventActivated;
    private bool endingActive;
    // Whether an event is currently on-going
    private GameEvent currentEvent;
    // General HUD component
    public GameObject HUDcomponent;
    // General event display screen
    public GameObject HUDevent;
    // Spaceship
    public GameObject spaceship;
    // Explosions 
    public GameObject explosion;
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
        endingActive = false;
        // Event trigger logic
        vars.Add("baseTimeToEvent", 6.0f);
        vars.Add("timeToEvent", 6.0f);
        vars.Add("eventSpeed", 1.0f);
        vars.Add("difficulty", 1.0f);
        vars.Add("distanceEarth", 200.0f);
        vars.Add("shipSpeed", 2.0f);
        vars.Add("timeToFuelClick", 3.0f);
        vars.Add("baseTimeToFuelClick", 3.0f);
        vars.Add("fuelConsumption", 1.0f);
        // Create the HUD
        CreateHUD();
        // Set default values for the various resources
        resources.Add("hull", 100);
        resources.Add("energy", 90);
        resources.Add("crew", 30);
        resources.Add("gameStatus", 0);
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
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.25f, 0.9f, 8));
        tmp.GetComponent<TextMesh>().text = "Hull";
        HUDobjects.Add("hull", tmp);
        // Energy
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.9f, 8));
        tmp.GetComponent<TextMesh>().text = "Energy";
        HUDobjects.Add("energy", tmp);
        // crew
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.75f, 0.9f, 8));
        tmp.GetComponent<TextMesh>().text = "crew";
        HUDobjects.Add("crew", tmp);
        // Base HUD update
        updateHUD("hull", 100.0f);
        updateHUD("energy", 90.0f);
        updateHUD("crew", 30.0f);
        // Distance to Earth
        tmp = GameObject.Instantiate(HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 8));
        HUDobjects.Add("distanceEarth", tmp);
    }

    // Update only a particular resource on the HUD
    private void updateHUD(string component, int blink)
    {
        switch (component)
        {
            case "hull":
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + resources[component] + "%";
                // Blink the corresponding HUD component with red/green depending if it's an addition or subtraction 
                if (blink > 0) {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.green, Color.white);
                }else if (blink < 0)
                {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.red, Color.white);
                }
                break;
            case "energy": case "crew":
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + resources[component] + "";
                // Blink the corresponding HUD component with red/green depending if it's an addition or subtraction 
                if (blink > 0)
                {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.green, Color.white);
                }
                else if (blink < 0)
                {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.red, Color.white);
                }
                break;
            case "distanceEarth":
                HUDobjects[component].GetComponent<TextMesh>().text = "Distance to Earth " + (int)vars[component] + " AU";
                break;
            default:
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + (int) resources[component];
                // Blink the corresponding HUD component with red/green depending if it's an addition or subtraction 
                if (blink > 0)
                {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.green, Color.white);
                }
                else if (blink < 0)
                {
                    HUDobjects[component].AddComponent<ColorLerp>();
                    HUDobjects[component].GetComponent<ColorLerp>().setColors(Color.red, Color.white);
                }
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
            case "distanceEarth":
                if ((int)newvalue > 0.0f)
                {
                    HUDobjects[component].GetComponent<TextMesh>().text = "Distance to Earth " + (int)newvalue + " AU";
                }
                else
                {
                    HUDobjects[component].GetComponent<TextMesh>().text = "Earth... but was space your true home?";
                }
                break;
            default:
                HUDobjects[component].GetComponent<TextMesh>().text = component + ": " + (int)newvalue;
                break;
        }
    }

    // Update a resource value
    private void updateResource(string component, double newvalue)
    {
        // Update the value itself in the table of resources
        switch (component) {
            case "hull": 
                // Upper cap at 100% for hull
                if (resources["hull"] + newvalue >= 100) resources["hull"] = 100;
                else resources[component] += (int)newvalue;
                // Lower cap
                if (resources[component] < 0) resources[component] = 0;
                break;
            // Lower cap of 0 for energy and crew (cannot go negative)
            case "energy": case "crew":
                resources[component] += (int)newvalue;
                if (resources[component] < 0) resources[component] = 0;
                break;
            default:
                resources[component] += (int)newvalue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Actively progressing along the 'trail'
        if (progressActive & !endingActive)
        {
            // Logic for generating new events in real time
            vars["timeToEvent"] -= vars["eventSpeed"] * Time.deltaTime;
            // When the progression is active, update the distance traveled in space
            vars["distanceEarth"] -= vars["shipSpeed"] * Time.deltaTime;
            // The ship uses fuel constantly on periodic intervals
            vars["timeToFuelClick"] -= vars["fuelConsumption"] * Time.deltaTime;
            if (vars["timeToFuelClick"] <= 0)
            {
                updateResource("energy", -3.0f);
                updateHUD("energy", -3);
                vars["timeToFuelClick"] = vars["baseTimeToFuelClick"];
            }

            // Logic for space progression
            // Keep a counter for distance traveled
            updateHUD("distanceEarth", vars["distanceEarth"]);
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

            // WIN CONDITIONS
            if (vars["distanceEarth"] <= 0)
            {
                // Ending boolean
                this.endingActive = true;
                // Stop the game progression
                this.progressActive = false;
                // Presumably made it to Earth
                this.TriggerWin(1);
            }

            // LOSE CONDITIONS
            if(resources["hull"] <= 0 | resources["energy"] <= 0 | resources["crew"] <= 0)
            {
                // Stop the game progression
                this.progressActive = false;
                // Ending boolean
                this.endingActive = true;
                // Placeholder, not yet indicating loss type
                this.TriggerLoss(1);
            }

        } 
        // Otherwise in menus or in an event yet to be resolved
        else if(!eventActivated & !endingActive)
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
        foreach (KeyValuePair<string, int> kvp in result)
        {
            // Update the corresponding resource variable as well as blink the HUD in the right direction (green = positive, red = negative)
            updateResource(kvp.Key, kvp.Value);
            updateHUD(kvp.Key, kvp.Value);
        }
        // Resume spacefaring
        this.progressActive = true;
        this.eventActivated = false;
    }

    // Time triggered spontaneous events
    private void TriggerTimeEvent()
    {
        // Temporary solution is to just generate a random event to solve
        currentEvent = em.GetRandomEvent();
    }

    // Player wins the game; int indicates the type of win
    private void TriggerWin(int type)
    {
        updateHUD("distanceEarth", 0.0f);
    }

    // Player loses the game; int indicates what type of loss occurs
    private void TriggerLoss(int type)
    {
        GameObject tmp = GameObject.Instantiate(this.explosion);
        tmp.transform.position = GameObject.Find("Spaceship").transform.position;
        GameObject.Destroy(GameObject.Find("Spaceship"));
    }

    public void OnMouseDown()
    {
        Debug.Log("Homebound clicked!");
        if (this.endingActive)
        {
            // On click while ending is active the game will restart
            Application.LoadLevel("main");
        }
    }
}
