using InControl;
using System;
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
    public delegate void ShootingEvent();
    public event ShootingEvent OnFire;

    public delegate void HealthChangedEvent(float health);
    public event HealthChangedEvent OnHealthChanged;

    public delegate void PlayerDiedEvent(PlayerScript player);
    public static event PlayerDiedEvent OnPlayerDeath;

    public GameObject reticle;

    [Tooltip("Player health")]
    public float maxHealth;
    public float health;

    [Tooltip("Speed the player moves at")]
    public float moveSpeed;

    [Tooltip("Is this player AI-controlled?")]
    public bool isAIControlled;

    [Tooltip("Type of chef")]
    public PlayerType chefType;

    // p1, p2, p3 etc
    public int playerNumber;

    public int showIndicatorFor;

    public SpriteRenderer halo;

    public bool dead = false;

    // to tint the UI
    public Color playerColor;

    public Animator animator;

    Rigidbody rb;
    GameData gameData;
    LineRenderer lineRenderer;

    [HideInInspector]
    public Vector3 reticlePos;

    public PlayerIndicatorScript playerIndicator;
        
    public bool up, down, left, right, firingUp, firingDown, firingLeft, firingRight;
    public bool firing = false;

    // InControl InputDevice
    public InputDevice controller;

    // If you want to apply a force to the player, do it with this vector
    public Vector3 pushForce;

    private void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += OnStateChange;
    }
    // Start is called before the first frame update
    void Start()
    {
      
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        halo.color = playerColor;
        firing = false;

        if (dead) return;

        // the following should never happen if the player is dead
        // human player reticle
        if (!isAIControlled)
        {
            // Keyboard and mouse input places the reticle via raycast
            if (controller == null)
            {
                // raycast to place reticle
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground"));
                reticlePos = hit.point;
                reticlePos.y = transform.position.y;

                if (Input.GetMouseButton(0))
                {
                    firing = true;
                    OnFire();
                }
            } else
            {
                // check if the right stick is being pushed enough to be past the deadzone
                if (controller.RightStick.Vector.magnitude > 0.5)
                {
                    // otherwise the reticle is placed with the right analog stick
                    Vector3 rightStick = new Vector3(controller.RightStick.Vector.x, 0, controller.RightStick.Vector.y);

                    reticlePos = transform.position + rightStick;


                    // trigger firing events
                    OnFire();

                    firing = true;
                } else
                {
                    reticlePos = transform.position;
                }
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, reticlePos);
            reticle.transform.position = reticlePos;

            Vector3 direction = new Vector3(0, 0, 0);

            if (controller == null) // If we have no controller, default to keyboard input
            {
                direction = new Vector3(Input.GetAxis("HorizontalPlayer"), 0, Input.GetAxis("VerticalPlayer"));
            } else
            {
                direction = new Vector3(controller.LeftStick.Vector.x, 0, controller.LeftStick.Vector.y);
            }


            rb.velocity = (direction * moveSpeed) + pushForce;

            pushForce /= 2;

            //if (pushForce.magnitude < 0.2f) pushForce = new Vector3(0, 0, 0);
        }
        else // if we are AI controlled
        {
            reticlePos = new Vector3(0, 0, 0);

            // If line is not set to the same position then it extends the size of the bounding rect and transform.position is all wack
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = false;
        }


        // this section is for working out the animation
        up = false;
        down = false;
        left = false;
        right = false;

        firingLeft = false;
        firingRight = false;
        firingUp = false;
        firingDown = false;

        Vector3 lineToReticle = transform.position - reticlePos;
        lineToReticle.Normalize();

        if (firing)
        {
            if (lineToReticle.x > 0.5f) firingLeft = true;
            if (lineToReticle.x < -0.5f) firingRight = true;
            if (lineToReticle.z > 0.1f) firingDown = true;
            if (lineToReticle.z < -0.1f) firingUp = true;
        }

        if (rb.velocity.z > 0.1f) up = true; 
        if (rb.velocity.z < -0.1f) down = true;  
        if (rb.velocity.x > 0.1f) right = true; 
        if (rb.velocity.x < -0.1f) left = true;


        if (animator != null)
        {
            animator.enabled = true;

            if (!firing)
            {
                if (right || left)
                {
                    if (right) animator.Play("walk right");
                    if (left) animator.Play("walk left");
                }
                else if (up || down)
                {
                    if (up) animator.Play("walk up");
                    if (down) animator.Play("walk down");
                }
                else
                {
                    animator.enabled = false;
                }
            } else
            {
                if (firingRight || firingLeft)
                {
                    if (firingRight) animator.Play("fire right");
                    if (firingLeft) animator.Play("fire left");
                }
                else if (firingUp || firingDown)
                {
                    if (firingUp) animator.Play("fire up");
                    if (firingDown) animator.Play("fire down");
                }
                else
                {
                    animator.enabled = false;
                }
            }
        }
    }

    public void ShowIndicator()
    {
        playerIndicator.Show("P" + playerNumber, playerColor, showIndicatorFor);
    }

    public bool IsActivatingTrap()
    {
        if (controller != null) return controller.Action1.IsPressed;
        else return Input.GetKey(KeyCode.E);
    }

    // Colliding with a zombie, take damage!
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Zombie")
        {
            // Push the player away from the zombie
            Vector3 normal = collision.GetContact(0).normal;
            rb.AddForce(normal * 5, ForceMode.Impulse);

            health -= 1 * GameObject.Find("Horde").GetComponent<HordeScript>().offensiveStat;

            if (health <= 0)
            {
                health = 0;
                Die();
            }

            if (OnHealthChanged != null) OnHealthChanged(health);
        }
    }

    public void Die()
    {
        dead = true;
        animator.enabled = true;
        animator.Play("die");
        OnPlayerDeath(this);
    }

    public void Respawn()
    {
        dead = false;
    }
    

    public void OnStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.PREP)
        {
            if (dead) Respawn();
            ShowIndicator();
        }
    }

}
