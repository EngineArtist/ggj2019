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
    private TextMesh childTextMesh;
    // When the user presses a button, select the corresponding integer to represent the event choice
    public int userchoice = 0;
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
        this.setText(this.wholeText, 35);
        //this.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.8f, 8));
        this.childTextMesh = this.GetComponentInChildren<TextMesh>();
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
