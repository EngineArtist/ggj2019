using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public Color32 startColor = Color.white;
    public Color32 endColor = Color.white;
    public float progress = 0.0f;
    public float progressSpeed = 0.5f;
    public TextMesh textMeshComponent;

    void Start()
    {
        // Get the Text Mesh for which we are creating a time-dependent gradiant effect
        this.textMeshComponent = this.GetComponent<TextMesh>();
        // If we couldn't find Text Mesh, get rid of this script
        if(this.textMeshComponent == null)
        {
            GameObject.Destroy(this);
        }
    }   

    // Format the colour start and end points
    public void setColors(Color32 startColor, Color32 endColor)
    {
        this.startColor = startColor;
        this.endColor = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        this.progress += progressSpeed * Time.deltaTime;
        this.textMeshComponent.color = Color32.Lerp(this.startColor, this.endColor, this.progress);
        // Lerping the color is complete, thus get rid of this component
        if (this.progress >= 1.0f)
        {
            GameObject.Destroy(this);
        }
    }
}
