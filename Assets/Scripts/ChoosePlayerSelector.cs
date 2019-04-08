using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerSelector : MonoBehaviour
{
    public bool isHovered = false;
    public bool isSelected = false;
    public int selectedPlayer;

    public Animator animator;
    public Image highlighter;
    public Text playerText;

    PlayerSelectionData playerSelectionData;

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

    public void ToggleSelect()
    {
        if (!isSelected)
        {
            isHovered = false;
            selectedPlayer = Random.Range(0, 4);

            isSelected = true;
            highlighter.color = playerSelectionData.playerColors[selectedPlayer];
            playerText.text = "P" + (selectedPlayer + 1);
            animator.Play("select");
        } else
        {
            isSelected = false;
            animator.Play("unselect");
        }
    }
}
