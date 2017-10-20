using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public Vector2 gridPosition = Vector2.zero;

    public Vector3 moveDestination;
    public float moveSpeed = 5.0f;

    public int movementPerActionPoint = 5;
    public int attackRange = 1;

    public bool moving = false;
    public bool attacking = false;

    public string playerName = "George";
    public int HP = 25;

    public float attackChance = 0.75f;
    public float defenseReduction = 0.15f;
    public int damageBase = 5;
    public float damageRollSides = 6; //d6

    public int actionPoints = 2;
    public int movementPoints = 4;

    void Awake()
    {
        moveDestination = transform.position;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        if(GameManager.instance.selectedPlayer == null )
        {
            GameManager.instance.removeTileHighlights();
            GameManager.instance.highlightTilesAt(gridPosition, Color.blue, this.movementPoints);
        }
    }

    void OnMouseExit()
    {
       // GameManager.instance.removeTileHighlights();
    }

    public virtual void TurnUpdate()
    {
        if (actionPoints <= 0)
        {
            actionPoints = 2;
            movementPoints = 4;
            moving = false;
            attacking = false;
            GameManager.instance.nextTurn();
        }
    }

    public virtual void TurnOnGUI()
    {

    }
}
