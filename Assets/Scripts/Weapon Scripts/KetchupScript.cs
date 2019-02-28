using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;
    public GameObject audioObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }
    

    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.localScale += new Vector3(0.15f, 0f, 0.1f);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.localScale -= new Vector3(0.4f, 0f, 0.2f);
        }


        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ground")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            transform.localScale = new Vector3(15, 0.1f, 10);
        }
    }
}
