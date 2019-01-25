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
    private GameObject HUDhull;
    private GameObject HUDenergy;
    private GameObject HUDpopulation;
    // Values for the resources
    private double hull;
    private int energy;
    private int population;

    // Start is called before the first frame update
    void Start()
    {
        progressActive = true;
        CreateHUD();
        hull = 100.0;
        energy = 1;
        population = 1;
    }

    // Create an interface for the player
    private void CreateHUD()
    {
        // Create the player HUD
        // Use viewport to position the HUD elements
        // Hull 
        HUDhull = GameObject.Instantiate(this.HUDcomponent);
        HUDhull.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10));
        HUDhull.GetComponent<TextMesh>().text = "Hull";
        // Energy
        HUDenergy = GameObject.Instantiate(this.HUDcomponent);
        HUDenergy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 1, 10));
        HUDenergy.GetComponent<TextMesh>().text = "Energy";
        // Population
        HUDenergy = GameObject.Instantiate(this.HUDcomponent);
        HUDenergy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.7f, 1, 10));
        HUDenergy.GetComponent<TextMesh>().text = "Population";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
