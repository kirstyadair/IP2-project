using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        /*
        transform.Rotate(90, 0, 0); 
        transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX & RigidbodyConstraints.FreezePositionY;
        */
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision");
        if (collision.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombieScript>().Hit();
            Destroy(gameObject);
        }
    }
}
