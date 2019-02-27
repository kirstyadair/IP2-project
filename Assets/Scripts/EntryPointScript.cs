using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPointScript : MonoBehaviour
{
    public Animator arrowAnimator;
    public Animator windowAnimator;
    public GameObject portalRing;
    public bool isCurrentEntryPoint = false;
    public bool isYeetingZombies = false;
    public bool isActivating = false;
    public BoxCollider2D windowCollider;

    public Transform yeetEnding;
    public Transform yeetStarting;

    public float timeoutBetweenYeetings = 1000f;
    float yeetingsDebounce = 0;
    public float yeetSpeed = 10;



    // How many zombies currently crowding around this
    int zombiesAtPoint = 0;
    GameData gameData;

    // Start is called before the first frame update
    void OnEnable()
    {
         
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += OnStateChange;
    }

    public void OnStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.PREP)
        {
            // Enable the arrows when in preparation stage
            arrowAnimator.gameObject.SetActive(true);
            arrowAnimator.SetBool("selected", false);
        } else if (newState == GameState.PLAY)
        {
            // Disable the arrows in play state
            arrowAnimator.gameObject.SetActive(false);
        }
    }

    public void ConfirmEntryPoint()
    {
        BreakGlass();
        isCurrentEntryPoint = true;
        gameData.ChangeState(GameState.PLAY);
        gameData.SetEntryPoint(this);

        //gameData.OnReadyForZombies += StartYeetingZombies;
    }

    public void StartYeetingZombies()
    {
        isYeetingZombies = true;
        //windowCollider.enabled = false;
    }

    public void BreakGlass()
    {
        windowAnimator.SetBool("broken", true);
        portalRing.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // possible bug - if zombies are killed during prep phase then they wo't go towards this counter
        if (collision.tag == "Zombie")
        {
            if (!isCurrentEntryPoint)
            {
                zombiesAtPoint++;
                isActivating = true;
                arrowAnimator.SetBool("selected", true);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        // possible bug - if zombies are killed during prep phase then they wo't go towards this counter
        if (collision.tag == "Zombie")
        {

            // Start yeeting zombies through the hole
            if (isYeetingZombies)
            {
                if (yeetingsDebounce > 0) return;
                if (!collision.gameObject.GetComponent<ZombieScript>().isBeingYeeted && !collision.gameObject.GetComponent<ZombieScript>().hasEnteredBuilding)
                {
                    yeetingsDebounce = timeoutBetweenYeetings;
                    GetComponent<AudioSource>().Play();
                    portalRing.GetComponent<Animator>().SetTrigger("pop");
                    StartCoroutine(YeetZombie(collision.gameObject.GetComponent<ZombieScript>()));
                }
            }
        }
    }

    IEnumerator YeetZombie(ZombieScript zombie)
    {
        zombie.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        zombie.isBeingYeeted = true;
        Transform zomb = zombie.gameObject.transform;

        // First yeet the zombie to the starting point
        Vector3 yeetDirection = Vector3.zero;
        float yeets = 0;
        while ((zomb.position - yeetStarting.position).magnitude > 0.5f && yeets < 50) {
            yeets++;
            yeetDirection = yeetStarting.position - zomb.position;
            yeetDirection.z = zomb.position.z;
            zomb.Translate(yeetDirection / yeetSpeed);
            yield return null;
        }

        yeets = 0;

        // Now yeet to the ending point

        while ((zomb.position - yeetEnding.position).magnitude > 0.2f && yeets < 50)
        {
            yeets++;
            yeetDirection = yeetEnding.position - zomb.position;
            yeetDirection.z = zomb.position.z;
            zomb.Translate(yeetDirection / yeetSpeed);
            yield return null;
        }

        zombie.hasEnteredBuilding = true;
        zombie.isBeingYeeted = false;
        zombie.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        zombie.gameObject.GetComponent<Rigidbody2D>().AddForce(yeetDirection);
        yield return null;
    }

    

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Zombie")
        {
            if (!isCurrentEntryPoint)
            {
                zombiesAtPoint--;
                if (zombiesAtPoint == 0)
                {
                    isActivating = false;
                    arrowAnimator.SetBool("selected", false);
                }
            } else
            {

            }
        }
    }
    void Update()
    {
        yeetingsDebounce -= Time.deltaTime;

        if (zombiesAtPoint > 0 && gameData.state == GameState.PREP)
        {
            // When the arrow filling up animation has finished playing, we have confirmed this entry point
            if (arrowAnimator.GetCurrentAnimatorStateInfo(1).IsName("arrowAnimation") && arrowAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1)
            {
                ConfirmEntryPoint();
            }
        }
    }
}
