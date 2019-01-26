using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPad : MonoBehaviour
{
    // To be added a wave function dampened entrance to the screen
    //public float sinepos = 0.5f;
    public string wholeText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam rhoncus eros ut pulvinar lobortis. Nulla eleifend maximus nisl. Suspendisse finibus dignissim justo eget facilisis. In ornare, dolor quis tristique auctor, magna dolor pretium nisi, sit amet convallis eros quam in neque. Aenean ut sagittis purus. Nullam massa tortor, convallis in congue eu, malesuada et erat.";
    public string typingText = "";
    public float typeSpeed = 24.0f;
    public float typedLength = 0.0f;
    public int lineWidth = 33;
    // Button GameObject; corresponding amount of responses will be created to the pad as there are alternatives
    public GameObject padButton;
    private TextMesh childTextMesh;
    // List of buttons created on the run
    private List<GameObject> buttons;
    // When the user presses a button, select the corresponding integer to represent the event choice
    public int index = 0;
    // When event choice is done, next click will get rid of event pad
    private bool destroyClick = false;
    // The GameEvent corresponding to this event pad
    private GameEvent ge;

    public void setText(string text, int lineLength)
    {
        // From: https://answers.unity.com/questions/190800/wrapping-a-textmesh-text.html
        // Split string by char " "         
        string[] words = text.Split(" "[0]);
        // Prepare result
        string result = "";
        // Temp line string
        string line = "";
        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;
            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {
                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }
        // Append last line into result        
        result += line;
        // Remove first " " char
        this.wholeText = result.Substring(1, result.Length - 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start cutting text into lines
        //this.setText(this.wholeText, 35);
        //this.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.8f, 8));
        this.childTextMesh = this.GetComponentInChildren<TextMesh>();
        // Format lists
        this.buttons = new List<GameObject>();
    }

    // Set the particular Game Event
    public void setGE(GameEvent ge)
    {
        // Save the game event to the pad
        this.ge = ge;
        // Take the event text from the particular event
        this.setText(ge.mainMessage, lineWidth);

        // Go trough button creation process
        GameObject tmp;
        // Create the required amount of response buttons to an event on the run
        for (int i = 0; i < ge.options.Count; i++)
        {
            Debug.Log("Creating button: " + i);
            // Run-time creation and positioning of the event decision buttons
            tmp = GameObject.Instantiate(this.padButton);
            tmp.transform.parent = this.gameObject.transform;
            tmp.GetComponent<ButtonScript>().setIndex(i);
            tmp.transform.position = new Vector3(0, -1+i*(-1.0f), 1);
            tmp.transform.localScale = new Vector3(7, 0.5f, 1);
            tmp.GetComponentInChildren<TextMesh>().text = ge.options[i].initialText;
            //this.buttons.Add(tmp);
        }
    }

    public void ButtonDown(int index)
    {
        if (!destroyClick) {
            this.index = index;
            Debug.Log("Decision click");
            this.setText(ge.options[index].resultText, lineWidth);
            this.typedLength = 0;
            // Get rid of buttons
            foreach (GameObject b in buttons)
            {
                Debug.Log("Destroying button");
                GameObject.Destroy(b.gameObject);
            }
            destroyClick = true;
        }
        else {
            GameObject.Find("Homebound").GetComponent<GameMaster>().ResolveEvent(ge.options[index].resultDict);
            Debug.Log("Destroy click");
            GameObject.Destroy(this.gameObject);
        }
        //Debug.Log("Button " + index + " pressed");
    }

    // Update is called once per frame
    void Update()
    {
        // Create a typewriter-like effect
        if (this.typedLength < this.wholeText.Length)
        {
            this.typedLength += this.typeSpeed * Time.deltaTime;
            this.childTextMesh.text = wholeText.Substring(0, Mathf.Min(wholeText.Length, (int) this.typedLength));
        }
    }

}
