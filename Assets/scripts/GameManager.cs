using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject UserPlayerPrefab;
    public GameObject AIPlayerPrefab;

    public int mapSize;

    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();
    public List<AIPlayer> ai = new List<AIPlayer>();
    public Player selectedPlayer = new Player();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        generateMap();
        generatePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedPlayer != null)
        {
           selectedPlayer.TurnUpdate();
        }
       // else nextTurn();
    }

    void OnGUI()
    {
       if (selectedPlayer != null) selectedPlayer.TurnOnGUI();
    }

    public void nextTurn()
    {
    }

    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance)
    {
        List<Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance);
        foreach (Tile t in highlightedTiles)
        {
            t.transform.GetComponent<Renderer>().material.color = highlightColor;
        }
    }

    public void removeTileHighlights()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                map[i][j].transform.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void moveCurrentPlayer(Tile destTile)
    {
        if (destTile.transform.GetComponent<Renderer>().material.color != Color.white)
        {
            int pmUtilisesX;
            int pmUtilisesY;
            int pmUtilises;
            pmUtilisesX = (int)destTile.gridPosition.x  - (int)selectedPlayer.gridPosition.x;
            
            if (pmUtilisesX < 0) {  pmUtilisesX = -pmUtilisesX;}
            pmUtilisesY = (int)destTile.gridPosition.y - (int)selectedPlayer.gridPosition.y;

            if (pmUtilisesY < 0) { pmUtilisesY = -pmUtilisesY; }
            pmUtilises = pmUtilisesX + pmUtilisesY;

            GameManager.instance.selectedPlayer.movementPoints -=  pmUtilises;
            selectedPlayer.gridPosition = destTile.gridPosition;
            selectedPlayer.moveDestination = destTile.transform.position + 1.5f * Vector3.up;
            removeTileHighlights();
        }
        else
        {
            Debug.Log("Déplacement invalide");
        }
    }

    public void attackWithCurrentPlayer(Tile destTile)
    {
        if (destTile.transform.GetComponent<Renderer>().material.color != Color.white)
        {
            Player target = null;
            foreach (Player p in players)
            {
                if (p.gridPosition == destTile.gridPosition)
                {
                    target = p;
                }
            }

            target.transform.GetComponent<Renderer>().material.color = Color.red;

            if (target != null)
            {
                if (selectedPlayer.gridPosition.x >= target.gridPosition.x - 1 && selectedPlayer.gridPosition.x <= target.gridPosition.x + 1 &&
                    selectedPlayer.gridPosition.y >= target.gridPosition.y - 1 && selectedPlayer.gridPosition.y <= target.gridPosition.y + 1)
                {
                    selectedPlayer.actionPoints--;

                    removeTileHighlights();
                    selectedPlayer.moving = false;

                    //attack logic
                    //roll to hit
                    bool hit = Random.Range(0.0f, 1.0f) <= selectedPlayer.attackChance;

                    if (hit)
                    {
                        //damage logic
                        int amountOfDamage = (int)Mathf.Floor(selectedPlayer.damageBase + Random.Range(0, selectedPlayer.damageRollSides));
                        target.HP -= amountOfDamage;
                        Debug.Log(selectedPlayer.playerName + " a touché " + target.playerName + " et lui a infligé " + amountOfDamage + " de dommages!");
                    }
                    else
                    {
                        Debug.Log(selectedPlayer.playerName + " a raté " + target.playerName + "!");
                    }
                }
                else
                {
                    Debug.Log("La cible est trop éloignée!");
                }
            }
        }
        else
        {
            Debug.Log("Déplacement invalide");
        }
    }

    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void generatePlayers()
    {
        UserPlayer player;

        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 1.5f, -0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(0, 0);
        player.playerName = "Bob";

        players.Add(player);

        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(2- Mathf.Floor(mapSize / 2),1.5f,-2 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(2,2);
        player.playerName = "Roger";

        players.Add(player);
    }
}
