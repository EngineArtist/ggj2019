using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverSine: MonoBehaviour {

    public float frequency;
    public float amplitude;

    Vector3 initPos;

    void Start() {
        initPos = transform.localPosition;
    }

    void Update() {
        transform.localPosition = initPos + Vector3.up*Mathf.Sin(Time.time*frequency)*amplitude;
    }
}
