using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    [Header("Elements")]
    public Image icone;
    public TextMeshProUGUI txt_qtd;

    [HideInInspector] public Item item;
    [HideInInspector] public int qtd { get; set; }

    [HideInInspector] public int[] indices;
 
    public void Style(Item item)
    {
        this.item = item;
        icone.sprite = item.icone;

        qtd = 1;

        txt_qtd.text = qtd.ToString("00");
    }

    public void Style(Item item, int[] indices)
    {
        Style(item);
        this.indices = indices;
    }

    public void AddQtd(int qtd)
    {
        this.qtd += qtd;
        txt_qtd.text = this.qtd.ToString("00");
    }
}
