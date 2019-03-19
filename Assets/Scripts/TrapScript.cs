using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    // Cooker trap variables
    bool trapDeactivated = false;
    float timeToReactivate;
    public float trapRadius;
    ParticleSystem smokePS;
    public ParticleSystem firePS;
    Vector3 trapCentre;

    // Start is called before the first frame update
    void Start()
    {
        trapCentre = GetComponent<BoxCollider>().bounds.center;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerScript>().IsActivatingTrap())
            {
                if (!trapDeactivated)
                {
                    StartCoroutine(Detonate());
                }
            }
        }
    }

    IEnumerator Detonate()
    {
        float timeToDetonate = 3.0f;
        do
        {
            Debug.Log(timeToDetonate);
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDetonate -= Time.deltaTime;
        }
        while (timeToDetonate >= 0);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, trapRadius);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.tag == "Zombie")
            {
                hit.gameObject.GetComponent<ZombieScript>().Hit();
            }
        }
        Debug.Log("Emitting");
        firePS.Emit(20);
        firePS.emissionRate = 0;
        trapDeactivated = true;
        smokePS = GetComponent<ParticleSystem>();
        if (!smokePS.isPlaying) smokePS.Play();
        StartCoroutine(Reactivate());
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
            smokePS.Stop();
            trapDeactivated = false;
        }
    }
}
