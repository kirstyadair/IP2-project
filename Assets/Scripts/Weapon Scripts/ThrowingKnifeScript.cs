using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
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
            Destroy(gameObject);
        }

        if (collision.tag == "Wall")
        {
            Debug.Log("collision");
            rb.constraints = RigidbodyConstraints.FreezePosition;
            Wiggle();
        }
    }

    void Wiggle()
    {
        // find how much time is left until the knife is destroyed
        float timeLeft = timeToDestroy;

        if (timeLeft > 0.5f)
        {
            StartCoroutine(WiggleUp());
        }
    }

    IEnumerator WiggleUp()
    {
        transform.Rotate(0, 10, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WiggleDown());
    }

    IEnumerator WiggleDown()
    {
        transform.Rotate(0, -10, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WiggleUp());
    }
}
