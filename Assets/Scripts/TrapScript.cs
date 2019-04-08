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
    public GameObject pointLight;

    // stats for the player that most recently activated this trap
    PlayerStats stats;

    Vector3 trapCentre;
    public Animator promptAnim;
    public Animator explosionAnim;
    GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        trapCentre = GetComponent<BoxCollider>().bounds.center;
        pointLight.SetActive(false);
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
                pointLight.SetActive(true);
                promptAnim.SetBool("activate", true);
                if (collision.GetComponent<PlayerScript>().IsActivatingTrap())
                {
                    stats = collision.GetComponent<PlayerScript>().stats;
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
            pointLight.SetActive(false);
        }
    }

    IEnumerator Detonate()
    {
        // Wait one second after activation before detonating
        float timeToDetonate = 1.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDetonate -= Time.deltaTime;
        }
        while (timeToDetonate >= 0);
        // Stop the pulsing animation 
        explosionAnim.SetBool("pulse", false);

        // Create an array of sushi colliders to apply damage to
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, trapRadius);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.tag == "Zombie")
            {
                if (hit.gameObject.GetComponent<ZombieScript>().Hit(gameData.cookerDamage) && stats != null) stats.kills++;
            }
        }

        // Play the fire particle effect
        firePS.Emit(1);
        firePS.emissionRate = 0;
        trapDeactivated = true;

        // Play the smoke particle effect and the explosion sound
        smokePS = GetComponent<ParticleSystem>();
        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
        if (!smokePS.isPlaying) smokePS.Play();
        // Start the countdown to re-enable the oven trap
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
