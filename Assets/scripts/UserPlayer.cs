using UnityEngine;
using System.Collections;

public class UserPlayer : Player
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.selectedPlayer == this)
        {
            transform.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }

        if (HP <= 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public override void TurnUpdate()
    {
        if (Vector3.Distance(moveDestination, transform.position) >= 0.1f)
        {
            transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
            if (Vector3.Distance(moveDestination, transform.position) <= 0.1f)
            {
                transform.position = moveDestination;
                actionPoints--;
                GameManager.instance.highlightTilesAt(gridPosition, Color.blue, this.movementPoints);
            }
        }
    }

    public override void TurnOnGUI()
    {
        float buttonHeight = 50;
        float buttonWidth = 150;

        Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);

        //Bouton pour attaquer
        if (GUI.Button(buttonRect, "Attaque"))
        {
            if (!attacking)
            {
                GameManager.instance.removeTileHighlights();
                moving = false;
                attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
            }
            else
            {
                moving = false;
                attacking = false;
                GameManager.instance.removeTileHighlights();
            }
        }

        //Bouton pour lancer un sort
        buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "Sort"))
        {
            if (!attacking)
            {
                GameManager.instance.removeTileHighlights();
                moving = false;
                attacking = true;
                GameManager.instance.highlightTilesAt(gridPosition, Color.cyan, attackRange);
            }
            else
            {
                moving = false;
                attacking = false;
                GameManager.instance.removeTileHighlights();
            }
        }

        //Bouton fin de tour
        buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "Fin de tour"))
        {
            GameManager.instance.removeTileHighlights();
            actionPoints = 2;
            moving = false;
            attacking = false;
            movementPoints = 6;
            GameManager.instance.selectedPlayer = null;
            GameManager.instance.nextTurn();
            
        }

        base.TurnOnGUI();
    }

    void OnMouseExit()
    {
        if(GameManager.instance.selectedPlayer  == null)
        {
            GameManager.instance.removeTileHighlights();
        }
    }

    void OnMouseDown()
    {
        if (GameManager.instance.selectedPlayer != null)
        {
            if (!GameManager.instance.selectedPlayer.attacking)
            {
                if (GameManager.instance.selectedPlayer == this)
                {
                    GameManager.instance.selectedPlayer = null;
                }
            }
        }
        else
        {
            GameManager.instance.selectedPlayer = this;
        }
        
    }
}
