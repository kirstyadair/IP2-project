using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject reticle;
    public GameObject bulletPrefab;
    public float moveSpeed;
    public float fireCooldown;
    public float fireTimeout = 0;
    float movementCooldown = 0;
    public Text firingCooldownText;
    public Text dragText;
    public Text moveSpeedText;
    ParticleSystem blood;
    Rigidbody2D rb;
    GameData gameData;
    LineRenderer lineRenderer;
    bool isAbleToFire = false;
    bool tempAI;
    int numOfHits = 5;

    // Start is called before the first frame update
    void Start()
    {
        blood = GetComponent<ParticleSystem>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        gameData.OnReadyForZombies += ZombiesHaveStarted;

        if (gameObject.tag == "TempAI")
        {
            tempAI = true;
        }
        else
        {
            tempAI = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        fireTimeout -= Time.deltaTime;


        Vector3 reticlePos;

        // human player reticle
        if (!tempAI)
        {
            reticlePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            reticlePos.z = 0;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, reticlePos);
            rb.simulated = true;
        }
        else // ai reticle
        {
            reticlePos = new Vector3(0, 0, 0);

            // If line is not set to the same position then it extends the size of the bounding rect and transform.position is all wack
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = false;
            rb.simulated = false;
        }


        reticle.transform.position = reticlePos;

        movementCooldown -= Time.deltaTime;

        //if (movementCooldown > 0) return;

        if (!tempAI)
        {
            Vector2 direction = new Vector2(Input.GetAxis("HorizontalPlayer"), Input.GetAxis("VerticalPlayer"));
            rb.AddForce(direction, ForceMode2D.Impulse);
        }

        //rb.velocity = direction * moveSpeed;

        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), transform.position.z);

        if (!tempAI)
        {
            if (Input.GetMouseButton(0) == true && fireTimeout <= 0)
            {
                reticle.GetComponent<Animator>().SetTrigger("pulse");

                Vector2 bulletDirection = reticlePos - transform.position;
                bulletDirection.Normalize();

                Shoot(bulletDirection);

                fireTimeout = fireCooldown;
            }
        }
        
        firingCooldownText.text = "Firing cooldown (" + fireCooldown + ")";
        dragText.text = "Drag (" + rb.drag + ")";
        moveSpeedText.text = "Move speed (" + moveSpeed + ")"; 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Zombie")
        {
            Vector2 normal = collision.GetContact(0).normal;
            rb.AddForce(normal * 5, ForceMode2D.Impulse);
            blood.Emit(20);
            movementCooldown = 0.5f;

            numOfHits--;

            if (numOfHits <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Shoot(Vector2 direction)
    {
        if (fireTimeout > 0) return;
        if (gameData.state == GameState.PREP) return; // Don't shoot whilst we're preparing
        if (!isAbleToFire) return; // Don't fire if firing is disabled
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.up = direction;
        bullet.GetComponent<BulletScript>().direction = direction;
    }


    public void ZombiesHaveStarted()
    {
        isAbleToFire = true;
    }

    public void IncreaseMoveSpeed()
    {
        moveSpeed++;
    }

    public void DecreaseMoveSpeed()
    {
        moveSpeed--;
    }

    public void IncreaseDrag()
    {
        rb.drag += 0.5f;
    }

    public void DecreaseDrag()
    {
        rb.drag -= 0.5f;
    }

    public void IncreaseFiringCooldown()
    {
        fireCooldown += 0.1f;
    }

    public void DecreaseFiringCooldown()
    {
        fireCooldown -= 0.1f;
    }
}
