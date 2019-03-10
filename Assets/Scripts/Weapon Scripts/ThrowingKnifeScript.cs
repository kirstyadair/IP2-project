using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    float timeToDestroy = 10.0f;
    float knifeLength = 0.25f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

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
        
        if (collision.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombieScript>().Hit();
            //Destroy(gameObject);
        }

        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
            //Rigidbody rb = GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            //Wiggle();
        }
    }

    /*void Wiggle()
    {
        // find entry direction
        Vector3 knifeDirection = transform.position - origin;
        knifeDirection.y = 0.25f;
        knifeDirection.Normalize();

        

        transform.Translate((-knifeDirection.x * (knifeLength/2)), 0, 0);
        Vector3 newpos = transform.position;
        newpos.y = 0.25f;

        // find how much time is left until the knife is destroyed
        float timeLeft = timeToDestroy;

        if (timeLeft > 0.5f)
        {
            StartCoroutine(WiggleUp());
        }
    }*/

    /*IEnumerator WiggleUp()
    {
        transform.Rotate(0, 10, 0);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(WiggleDown());
    }

    IEnumerator WiggleDown()
    {
        transform.Rotate(0, -10, 0);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(WiggleUp());
    }*/
}
