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
    float timeToDeactivate = 5.0f;
    float timeToActivate = 5.0f;
    bool activated = true;

    public TrapType trapType;

    public ParticleSystem ps;
    public BoxCollider bCollider;

    // Start is called before the first frame update
    void Start()
    {
        timeToExplode = 3.0f;
        if (trapType == TrapType.EXTINGUISHER)
        {
            ps = GetComponent<ParticleSystem>();
            timeToDeactivate = 5.0f;
            StartCoroutine(PSActive());
            bCollider = GetComponent<BoxCollider>();
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
            // if active, enable the collider
            if (activated) bCollider.enabled = true;
            else bCollider.enabled = false;
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

    IEnumerator PSActive()
    {
        activated = true;
        if (!ps.isPlaying)
        {
            ps.Play();
        }

        timeToDeactivate = 5.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDeactivate -= Time.deltaTime;

        } while (timeToDeactivate >= 0);
        
        if (timeToDeactivate <= 0)
        {
            Debug.Log("Deactivating");
            StartCoroutine(PSInactive());
        }
    }

    IEnumerator PSInactive()
    {
        activated = false;
        if (ps.isPlaying)
        {
            ps.Stop();
        }

        timeToActivate = 5.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToActivate -= Time.deltaTime;

        } while (timeToActivate >= 0);

        if (timeToActivate <= 0)
        {
            Debug.Log("Activating");
            StartCoroutine(PSActive());
        }
    }
}
