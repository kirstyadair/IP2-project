using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravestoneScript : MonoBehaviour
{
    GameData gameData;
    Rigidbody rb;
    Animator animator;

    // How much to launch by
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        // Launch into air
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        gameData.OnStateChange += OnStateChange;
    }

    void OnStateChange(GameState oldState, GameState newState)
    {
        // kill the gravestone if we're back in prep stage
        if (newState == GameState.PREP)
        {
            animator.Play("dissapear");
            Destroy(gameObject, 5f);
        }
    }

    private void OnDestroy()
    {
        gameData.OnStateChange -= OnStateChange;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
