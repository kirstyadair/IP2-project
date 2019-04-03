using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeCrosshairScript : MonoBehaviour
{

    Vector3 lastPosition;

    Vector3 innerPosition;
    Vector3 innerMostPosition;

    Vector3 velocity = Vector3.zero;
    public GameObject innerRing;
    public GameObject innerMostRing;

    public float wobbleFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = this.transform.position;
        innerPosition = innerRing.transform.localPosition;
        innerMostPosition = innerMostRing.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        velocity += (lastPosition - transform.position);

        innerRing.transform.localPosition = innerPosition - velocity / 2;
        innerMostRing.transform.localPosition = innerMostPosition - velocity / 3;
        lastPosition = transform.position;

        velocity *= wobbleFactor;
    }
}
