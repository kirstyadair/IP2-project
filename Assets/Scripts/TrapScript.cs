using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    // Cooker trap variables
    bool trapDeactivated = false;
    bool activate = false;
    bool pulse = false;
    float timeToReactivate;
    public float trapRadius;
    ParticleSystem smokePS;
    public ParticleSystem firePS;
    Vector3 trapCentre;
    public Animator promptAnim;
    public Animator explosionAnim;

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
            if (!trapDeactivated)
            {
                promptAnim.SetBool("activate", true);
                if (collision.GetComponent<PlayerScript>().IsActivatingTrap())
                {

                    promptAnim.SetBool("activate", false);
                    explosionAnim.SetBool("pulse", true);
                    StartCoroutine(Detonate());

                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            promptAnim.SetBool("activate", false);
        }
    }

    IEnumerator Detonate()
    {
        float timeToDetonate = 3.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDetonate -= Time.deltaTime;
        }
        while (timeToDetonate >= 0);
        explosionAnim.SetBool("pulse", false);

        
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, trapRadius);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.tag == "Zombie")
            {
                hit.gameObject.GetComponent<ZombieScript>().Hit();
            }
        }
        firePS.Emit(20);
        firePS.emissionRate = 0;
        Debug.Log(firePS.emissionRate);
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
