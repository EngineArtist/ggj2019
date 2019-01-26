using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundScroll: MonoBehaviour {

    public GameObject[] prefabs;
    public float speed;
    public float size;
    public int count;
    public float x0;
    public float x1;
    public float y0;
    public float y1;

    Transform[] objects;

    void Start() {
        objects = new Transform[count];
        for (int i = 0; i < count; ++i) {
            var gobj = GameObject.Instantiate(
                prefabs[Random.Range(0, prefabs.Length)],
                new Vector3(Random.value*(x1-x0) - (x1-x0)/2f, Random.value*(y1-y0) - (y1-y0)/2f, 0f),
                Quaternion.identity
            );
            objects[i] = gobj.transform;
            gobj.transform.SetParent(transform, false);
            gobj.transform.localScale = new Vector3(size, size, 1f);
        }
    }

    void Update() {
        for (int i = 0; i < count; ++i) {
            var t = objects[i];
            t.localPosition += Vector3.left*speed*Time.deltaTime;
            if (t.localPosition.x < x0) t.localPosition += Vector3.right*(x1-x0);
        }
    }
}
