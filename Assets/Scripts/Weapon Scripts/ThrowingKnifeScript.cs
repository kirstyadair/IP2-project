using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnifeScript : MonoBehaviour
{
    public AudioClip knifeLandSound;
    public AudioClip knifeThrowSound;
    float timeToDestroy = 3f;
    float knifeLength = 0.25f;
    Animator animator;
    AudioSource audioSource;
    GameData gameData;
    public ParticleSystem particleSystem;
    Vector3 origin;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
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
        if (collision.gameObject == null) return;

        if (collision.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombieScript>().Hit(gameData.throwingKnifeDamage);
            //Destroy(gameObject);
        }

        if (collision.tag == "Wall")
        {
            if (rb == null) return;
            rb.velocity = Vector3.zero;
            animator.Play("wiggle");
            particleSystem.Emit(50);
            audioSource.PlayOneShot(knifeLandSound);
            //Destroy(gameObject);
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
