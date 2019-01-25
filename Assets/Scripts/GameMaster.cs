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
    // Specific HUD components that are updated on the run
    public Dictionary<string, GameObject> HUDobjects;
    // Values for the resources
    public Dictionary<string, int> resources;
    // Objects in space
    public List<GameObject> spaceObjects;

    // Start is called before the first frame update
    void Start()
    {
        progressActive = true;
        CreateHUD();
        // Set default values for the various resources
        resources.Add("hull", 100);
        resources.Add("energy", 1);
        resources.Add("population", 1);
        // Dynamically allocated array of objects in space
        spaceObjects = new List<GameObject>();
    }

    // Create an interface for the player
    private void CreateHUD()
    {
        GameObject tmp;
        // Create the player HUD
        // Use viewport to position the HUD elements; 
        // {0,0} is lower-left corner, {1,1} is top-right and the 3rd component is the depth
        // Hull 
        tmp = GameObject.Instantiate(this.HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Hull";
        HUDobjects.Add("hull", tmp);
        // Energy
        tmp = GameObject.Instantiate(this.HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Energy";
        HUDobjects.Add("energy", tmp);
        // Population
        tmp = GameObject.Instantiate(this.HUDcomponent);
        tmp.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 1, 10));
        tmp.GetComponent<TextMesh>().text = "Population";
        HUDobjects.Add("population", tmp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
