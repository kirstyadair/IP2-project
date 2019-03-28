using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    public AudioClip knifeLandSound;
    public AudioClip knifeThrowSound;
    public PlayerStats stats;
    float timeToDestroy = 3f;
    float knifeLength = 0.25f;
    Animator animator;
    AudioSource audioSource;
    GameData gameData;
    public ParticleSystem particleSystem;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(knifeThrowSound);
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
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
        // idk why i need to do this
        if (gameData == null) gameData = GameObject.Find("GameData").GetComponent<GameData>();

        if (collision.tag == "Zombie")
        {
            if (collision.gameObject.GetComponent<ZombieScript>().Hit(gameData.throwingKnifeDamage)) stats.kills++;
            Destroy(gameObject);
        }

        if (collision.tag == "Wall")
        {
            if (rb == null) return;
            rb.velocity = Vector3.zero;
            animator.Play("wiggle");
            particleSystem.Emit(50);
            audioSource.PlayOneShot(knifeLandSound);
        }
    }
}
