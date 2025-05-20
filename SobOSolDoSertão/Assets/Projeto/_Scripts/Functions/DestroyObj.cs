using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public bool open = false;

    public void Desabilite()
    {
        gameObject.SetActive(false);
        open = false;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void Open()
    {
        open = true;
    }
}
