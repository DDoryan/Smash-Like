using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI UIplayer1;
    public TextMeshProUGUI UIplayer2;
    public static UIManager instance;
    public void Start()
    {
        instance = this;
    }

    public void SetDamageTextPlayer1(string damageText)
    {
        UIplayer1.text = damageText;
    }

    public void SetDamageTextPlayer2(string damageText)
    {
        UIplayer2.text = damageText;
    }
}