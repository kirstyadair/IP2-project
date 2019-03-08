using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    UNDECIDED, FAT, CRAZY, THIN, HORDE
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
    public PlayerType chefType;
    

    Rigidbody rb;
    GameData gameData;
    LineRenderer lineRenderer;

    [HideInInspector]
    public Vector3 reticlePos;

    public Animator animator;

    public bool up, down, left, right;

    // InControl InputDevice
    InputDevice input;

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

            Vector3 direction = new Vector3(0, 0, 0);

            if (input == null) // If we have no controller, default to keyboard input
            {
                direction = new Vector3(Input.GetAxis("HorizontalPlayer"), 0, Input.GetAxis("VerticalPlayer"));
            } else
            {
                direction = new Vector3(input.LeftStick.Vector.x, 0, input.LeftStick.Vector.y);
            }

            rb.velocity = direction * moveSpeed;
        }
        else // if we are AI controlled
        {
            reticlePos = new Vector3(0, 0, 0);

            // If line is not set to the same position then it extends the size of the bounding rect and transform.position is all wack
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = false;
        }


        up = false;
        down = false;
        left = false;
        right = false;

        if (rb.velocity.z > 0.1f) up = true; 
        if (rb.velocity.z < -0.1f) down = true;  
        if (rb.velocity.x > 0.1f) right = true; 
        if (rb.velocity.x < -0.1f) left = true;


        if (animator != null)
        {
            animator.enabled = true;
            if (right || left)
            {
                if (right) animator.Play("walk right");
                if (left) animator.Play("walk left");
            }
            else if (up || down)
            {
                if (up) animator.Play("walk up");
                if (down) animator.Play("walk down");
            } else
            {
                animator.enabled = false;
            }
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
    

    public void OnStateChange(GameState oldState, GameState newState)
    {

    }

}
