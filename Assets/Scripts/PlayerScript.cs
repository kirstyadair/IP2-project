using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChefType
{
    FAT, CRAZY, THIN
}

public class PlayerScript : MonoBehaviour
{
    public GameObject reticle;

    [Tooltip("Hitpoints before player dies")]
    int numOfHits = 5;

    [Tooltip("Speed the player moves at")]
    public float moveSpeed;

    [Tooltip("Is this player AI-controlled?")]
    public bool isAIControlled;

    [Tooltip("Type of chef")]
    public ChefType chefType;

    // These will be moved to separate script soon
    public float fireCooldown;
    public GameObject knifePrefab;
    float fireTimeout = 0;
    /////////////////////////////////

    Rigidbody rb;
    GameData gameData;
    LineRenderer lineRenderer;

    [HideInInspector]
    public Vector3 reticlePos;


    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        gameData.OnStateChange += OnStateChange;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        fireTimeout -= Time.deltaTime;
        
        // human player reticle
        if (!isAIControlled)
        {
            // raycast to place reticle
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground"));
            reticlePos = hit.point;

            reticlePos.y = transform.position.y;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, reticlePos);
            reticle.transform.position = reticlePos;

            Vector3 direction = new Vector3(Input.GetAxis("HorizontalPlayer"), 0, Input.GetAxis("VerticalPlayer"));
            rb.AddForce(direction, ForceMode.Impulse);

            if (Input.GetMouseButton(0) == true && fireTimeout <= 0)
            {
                Vector3 bulletDirection = reticlePos - transform.position;
                bulletDirection.Normalize();

                Shoot(bulletDirection);

                fireTimeout = fireCooldown;
            }
        }
        else // if we are AI controlled
        {
            reticlePos = new Vector3(0, 0, 0);

            // If line is not set to the same position then it extends the size of the bounding rect and transform.position is all wack
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = false;
        }
    }

    // Colliding with a zombie, take damage!
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Zombie")
        {
            // Push the player away from the zombie
            Vector3 normal = collision.GetContact(0).normal;
            rb.AddForce(normal * 5, ForceMode.Impulse);

            numOfHits--;

            if (numOfHits <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    //////// COME BACK TO THIS ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Shoot(Vector3 direction)
    {
        if (fireTimeout > 0) return;
        if (gameData.state == GameState.PREP) return; // Don't shoot whilst we're preparing
        GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);
        
        knife.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        knife.transform.up = direction;
    }


    public void OnStateChange(GameState oldState, GameState newState)
    {

    }

}
