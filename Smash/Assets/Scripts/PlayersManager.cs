using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public int playerCount = 0;
    public GameObject[] spawns;
    public int[] mask;
    public GameObject[] connectionTexts;
    public GameObject splitter;
    public List<GameObject> players = new();
    public static PlayersManager instance;
    private List<Color> colorList = new List<Color>();

    public void Start()
    {
        colorList.Add(Color.blue); 
        colorList.Add(Color.red);
    }
    public void OnPlayerJoined(PlayerInput PlayerInput)
    {
        instance = this;
        players.Add(PlayerInput.transform.gameObject);
        playerCount++;
        PlayerInput.transform.gameObject.GetComponent<SpriteRenderer>().color = colorList[playerCount-1];
        PlayerInput.transform.position = spawns[(playerCount-1)%spawns.Length].transform.position;
        PlayerInput.transform.gameObject.layer = mask[playerCount - 1];
        connectionTexts[playerCount-1].SetActive(false);
        if (playerCount == 2) 
        {
            splitter.SetActive(false);
        }
    }

    public string Winner()
    {
        for (int i = 0; i < players.Count; i++) 
        {
            if (!players[i].GetComponent<PlayerController>().isDead)
            {
                int winner = i + 1;
                return("Le gagnant est : Joueur " + winner);
            }
        }
        return ("Égalité");
    }
}
