using UnityEngine;

public class Coins : MonoBehaviour
{
    public int coins;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.controller.AddCoins(coins);
            GameManager.controller.Alert("+ " + coins + " moedas");
            Instantiate(InventoryManager.instance.collect_fx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
