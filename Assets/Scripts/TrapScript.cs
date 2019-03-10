using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    // Cooker trap variables
    bool trapDeactivated = false;
    float timeToExplode;
    float timeToReactivate;
    public float trapRadius;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        timeToExplode = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToExplode <= 0)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, trapRadius);
            foreach (Collider zombie in hitEnemies)
            {
                if (zombie.tag == "Zombie")
                {
                    zombie.gameObject.GetComponent<ZombieScript>().Hit();
                }
            }
            trapDeactivated = true;
            timeToExplode = 3;
            ps = GetComponent<ParticleSystem>();
            if (!ps.isPlaying) ps.Play();
            StartCoroutine(Reactivate());
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Collided");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed, trap deactivated " + trapDeactivated);
                if (!trapDeactivated)
                {
                    StartCoroutine(Grow());
                }
            }
        }
    }

    IEnumerator Grow()
    {
        do
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            timeToExplode -= Time.deltaTime;
            if (timeToExplode <= 0) break;
        }
        while (transform.localScale.x <= 8.5f);

        if (timeToExplode > 0)
        {
            Debug.Log("Shrinking");
            StartCoroutine(Shrink());
        }
    }

    IEnumerator Shrink()
    {
        do
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            timeToExplode -= Time.deltaTime;
            if (timeToExplode <= 0) break;
        }
        while (transform.localScale.x >= 8f);

        if (timeToExplode > 0)
        {
            Debug.Log("Growing");
            StartCoroutine(Grow());
        }
    }

    IEnumerator Reactivate()
    {
        timeToReactivate = 60.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToReactivate -= Time.deltaTime;
        }
        while (timeToReactivate >= 0);

        if (timeToReactivate <= 0)
        {
            ps.Stop();
            trapDeactivated = false;
        }
    }
}
