using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;
    //public KetchupBeamScript ketchupBeamScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //ketchupBeamScript = GetComponent<KetchupBeamScript>();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            transform.localScale += new Vector3(0.2f, 0f, 0.1f);
        }
        if (Input.GetButtonUp("Fire1"))
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
            transform.localScale = new Vector3(20, 0.1f, 10);
        }
    }
}
