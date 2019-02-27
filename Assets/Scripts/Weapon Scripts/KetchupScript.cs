using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        transform.localScale += new Vector3(0.2f, 0f, 0.1f);

        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ground")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY & RigidbodyConstraints.FreezePositionZ & RigidbodyConstraints.FreezePositionY;
            transform.localScale = new Vector3(20, 0.1f, 10);
        }
    }
}
