using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    COOKER, EXTINGUISHER, SINK
}

public class TrapScript : MonoBehaviour
{
    // Cooker trap variables
    bool trapDeactivated = false;
    float timeToExplode;
    public float trapRadius;

    // Extinguisher trap variables
    bool activated = true;
    float timeToDeactivate = 10.0f;
    float maxTimeToDeactivate;
    float timeToActivate = 20.0f;
    float maxTimeToActivate;

    public TrapType trapType;

    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        timeToExplode = 3.0f;
        if (trapType == TrapType.EXTINGUISHER)
        {
            ps = GetComponent<ParticleSystem>();
            timeToDeactivate = maxTimeToDeactivate;
            StartCoroutine(Deactivate());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (trapType == TrapType.COOKER)
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
                gameObject.SetActive(false);
                trapDeactivated = true;
                timeToExplode = 3;
            }
        }
        
        if (trapType == TrapType.EXTINGUISHER)
        {
            
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (trapType == TrapType.COOKER)
            {
                Debug.Log("Collided, press E to detonate");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E pressed");
                    if (!trapDeactivated)
                    {
                        StartCoroutine(Grow());
                    }
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
        }
        while (transform.localScale.x >= 8f);

        if (timeToExplode > 0)
        {
            Debug.Log("Growing");
            StartCoroutine(Grow());
        }
    }

    IEnumerator Deactivate()
    {
        Debug.Log("Coroutine called");

        if (!ps.isPlaying)
        {
            ps.Play();
            Debug.Log("Particle system playing");
        }

        do
        {
            Debug.Log("Loop reached");
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDeactivate -= Time.deltaTime;

        } while (timeToDeactivate >= 0);
        

        if (timeToDeactivate <= 0)
        {
            Debug.Log("Loop finished");
            StartCoroutine(Activate());
            timeToDeactivate = maxTimeToDeactivate;
        }
    }

    IEnumerator Activate()
    {
        if (ps.isPlaying) ps.Pause();

        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToActivate -= Time.deltaTime;

        } while (timeToActivate >= 0);

        if (timeToActivate <= 0)
        {
            StartCoroutine(Deactivate());
            timeToActivate = maxTimeToActivate;
        }
    }
}
