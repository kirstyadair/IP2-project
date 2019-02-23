using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    bool trapDeactivated = false;
    bool exploding = true;
    float timeToExplode;
    float scale;
    public float largeScaleLimit = 50;
    public float smallScaleLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        timeToExplode = 5.0f;
        scale = transform.localScale.x;
        scale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploding == true)
        {
            timeToExplode -= Time.deltaTime;
            Pulse();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!trapDeactivated)
                {
                    timeToExplode = 5.0f;
                    Pulse();
                }
            }
        }
    }

    void Pulse()
    {
        for (int i = 0; i < largeScaleLimit; i++)
        {
            scale += i;
        }

        for (int i = 0; i < smallScaleLimit; i++)
        {
            scale -= i;
        }
    }
}
