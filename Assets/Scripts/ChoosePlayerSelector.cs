using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerSelector : MonoBehaviour
{
    public bool isHovered = false;
    public bool isSelected = false;
    public int selectedPlayer = -1;

    public Animator animator;
    public Image highlighter;
    public Text playerText;

    PlayerSelectionData playerSelectionData;

    List<CursorScript> cursorsHovering = new List<CursorScript>();

    public PlayerType playerType;

    public void Start()
    {
        playerSelectionData = GameObject.Find("PlayerSelectionData").GetComponent<PlayerSelectionData>();
    }

    public void Hover()
    {
        if (isSelected) return;

        if (!isHovered)
        {
            isHovered = true;
            animator.Play("hover");
        }
    }

    public void Unhover()
    {
        if (isSelected) return;

        if (isHovered)
        {
            isHovered = false;
            animator.Play("unhover");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hover();
        CursorScript cursor = collision.GetComponent<CursorScript>();

        cursorsHovering.Add(cursor);

        // if the cursor is still touching another selector, unhover from that one
        if (cursor.currentlyHoveredSelector != null)
        {
            cursor.currentlyHoveredSelector.cursorsHovering.Remove(cursor);
            if (cursor.currentlyHoveredSelector.cursorsHovering.Count == 0) cursor.currentlyHoveredSelector.Unhover();
        }

        collision.GetComponent<CursorScript>().currentlyHoveredSelector = this;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        CursorScript cursor = collision.GetComponent<CursorScript>();
        cursorsHovering.Remove(cursor);

        // If the cursor has left this selector and not entered another at the same time, set it to null
        if (cursor.currentlyHoveredSelector == this) cursor.currentlyHoveredSelector = null;

        // only unhover if there are no cursors sitting on this boi
        if (cursorsHovering.Count == 0) Unhover();
    }

    public void Unselect()
    {
        if (isSelected)
        {
            isSelected = false;
            animator.Play("unselect");
        }
    }

    public void Select(int playerNumber)
    {
        selectedPlayer = playerNumber;

        if (!isSelected)
        {
            isHovered = false;
            isSelected = true;
            highlighter.color = playerSelectionData.playerColors[selectedPlayer];
            playerText.text = "P" + (selectedPlayer + 1);
            animator.Play("select");
        } 
    }
}
