using UnityEngine;

[CreateAssetMenu(fileName = "-----", menuName = "Inventario/Item")]
public class Item : ScriptableObject
{
    public string nome;
    public Sprite icone;

    public int slots = 1;

    public bool group = false;
    public int maxGroup = 1;
}