using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;
    public GameObject audioObject;
    KetchupBeamScript ketchupBeamScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ketchupBeamScript = GameObject.Find("KetchupSpawnPoint").GetComponent<KetchupBeamScript>();
    }
    

    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            transform.localScale += ketchupBeamScript.ketchupSpread;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            transform.localScale -= new Vector3(ketchupBeamScript.ketchupSpread.x*3, 0f, ketchupBeamScript.ketchupSpread.z*2);
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
            //transform.localScale = ketchupBeamScript.ketchupPuddle;
        }

        if (other.tag == "Zombie")
        {
            other.gameObject.GetComponent<ZombieScript>().Hit();
        }
    }
}
