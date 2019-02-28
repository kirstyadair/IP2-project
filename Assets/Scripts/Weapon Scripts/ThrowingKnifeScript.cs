using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(90, 0, 0);
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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.collider.tag == "Zombie")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
