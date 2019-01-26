using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public int buttonIndex;
    private GameObject parent;
    
    // Start is called before the first frame update
    void Start()
    {
        this.parent = this.transform.parent.gameObject;
    }
    // Set the index of the button, i.e. which choice it represents
    public void setIndex(int index)
    {
        this.buttonIndex = index;
    }
    // User pressed the button; sending pad information on which choice was chosen
    void OnMouseDown()
    {
        parent.GetComponent<EventPad>().ButtonDown(this.buttonIndex);
    }
}
