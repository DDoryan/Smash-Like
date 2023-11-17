using TMPro;
using UnityEngine;

public class MapLimite : MonoBehaviour
{
    public PlayersManager _playersManager;
    public GameObject gameOver;
    public TextMeshProUGUI winnerText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_playersManager.playerCount < 2)
        {
            if(collision != null && collision.transform.gameObject.layer == 3) 
            {
                collision.transform.position = _playersManager.spawns[0].transform.position;
            }
        }
        else
        {
            collision.GetComponent<PlayerController>().isDead = true;
            winnerText.text = PlayersManager.instance.Winner();
            MenusManager.Instance.EnableGameOver();
        }
    }
}
