using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    private Transform player;
    public Item item;
    private bool isNear = false;

    void Start()
    {
        player = FindFirstObjectByType<CharacterManager>().transform;
    }

    void LateUpdate()
    {
        Collect();
    }

    public void Collect()
    {
       float distance = Vector2.Distance(transform.position, player.position);
        
        // Chegou perto
        if (distance < 1.5f && !isNear)
        {
            isNear = true;
            GameManager.controller.collectInterface.SetActive(true);
            GameManager.controller.currentColetavel = this;
        }
        // Saiu de perto
        else if (distance >= 1.5f && isNear)
        {
            isNear = false;
            if (GameManager.controller.currentColetavel == this)
            {
                GameManager.controller.collectInterface.SetActive(false);
                GameManager.controller.currentColetavel = null;
            }
        }

        if (isNear && Input.GetButtonDown("Collect"))
        {
            if (InventoryManager.instance.CreateItem(item))
            {
                GameManager.controller.Alert("+1 " + item.name);
                Instantiate(InventoryManager.instance.collect_fx, transform.position, Quaternion.identity);
                
                // 🔻 DESATIVA a interface antes de destruir
                GameManager.controller.collectInterface.SetActive(false);
                GameManager.controller.currentColetavel = null;
                
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 1.5f));
    }
}
