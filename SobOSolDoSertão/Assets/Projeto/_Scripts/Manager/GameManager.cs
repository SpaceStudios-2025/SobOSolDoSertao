using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager controller;

    [Header("Interface")]
    [SerializeField] private TextMeshProUGUI alert_txt;
    public GameObject collectInterface;

    [HideInInspector] public ItemPrefab currentColetavel = null;

    void Awake() => controller = controller == null ? this : controller;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI coins_txt;

    public static int CoinsAll = 0;

    public void Alert(string text)
    {
        alert_txt.gameObject.SetActive(true);
        alert_txt.text = text;
    }

    public void AddCoins(int coins)
    {
        CoinsAll += coins;
        coins_txt.text = CoinsAll.ToString("00");
    }
}
