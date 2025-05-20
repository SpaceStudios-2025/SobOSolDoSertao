using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool full;

    public void Complete() => full = true;

    [Header("Barra Rapida")]
    public bool speed_bar;
    public bool center;
    public int bar; // 0 = centro, 1 = left, 2 right

    public GameObject icone;

    public GameObject select_item;
}