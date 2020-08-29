using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float length, startpos;
    public GameObject cam;
    public float parallax;
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1-parallax);
        float dist = cam.transform.position.x * parallax;
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        startpos += length;
        else if (temp < startpos - length)
        startpos -= length;
    }
}
