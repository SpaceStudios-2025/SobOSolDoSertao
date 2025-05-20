using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Fx")]
    public ParticleSystem collect_fx;

    [Header("Elements")]
    [SerializeField] private SlotManager slotManager;

    void Awake() => instance = !instance ? this : instance;

    [Header("Barra Rapida")]
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private GameObject rodaContainer;
    [SerializeField] private GameObject roda;

    public List<Slot> barraRapida = new();

    private readonly float[] angles = { 0f, 106f, -134f };
    private bool isRotating = false;

    [Header("Hand")]
    [SerializeField] private Image iconeHand;

    public bool CreateItem(Item item)
    {
        return slotManager.Create(item);
    }

    void LateUpdate()
    {
        BarraRapida();
    }

    void BarraRapida()
    {
        if (Input.GetButton("Roda"))
        {
            rodaContainer.SetActive(true);

            if (!isRotating && rodaContainer.GetComponent<DestroyObj>().open)
            {
                if (Input.GetButtonDown("Left")) RotateRoda(-1);
                else if (Input.GetButtonDown("Right")) RotateRoda(1);
            }
        }
        else if (Input.GetButtonUp("Roda"))
        {
            if (rodaContainer.GetComponent<DestroyObj>().open)
                rodaContainer.GetComponent<Animator>().SetTrigger("close");
            else rodaContainer.GetComponent<DestroyObj>().Desabilite();
        }
    }

    void RotateRoda(int direction)
    {
        for (int i = 0; i < barraRapida.Count; i++)
        {
            if (barraRapida[i].center)
            {
                barraRapida[i].center = false;
                barraRapida[i].select_item.SetActive(false);

                int nextIndex = (i + direction + barraRapida.Count) % barraRapida.Count;
                barraRapida[nextIndex].center = true;
                iconeHand.sprite = barraRapida[nextIndex].icone.GetComponent<Image>().sprite;

                StartCoroutine(RotateToAngle(Quaternion.Euler(0, 0, angles[nextIndex]), nextIndex));

                break;
            }
        }
    }

    private IEnumerator RotateToAngle(Quaternion targetRotation, int index)
    {
        isRotating = true;
        Quaternion startRotation = roda.transform.localRotation;
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / rotationDuration;
            roda.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        foreach (var bar in barraRapida)
        {
            bar.icone.transform.localRotation = Quaternion.Euler(0, 0, -angles[index]);
        }

        // Garante que termina exatamente no alvo
        roda.transform.localRotation = targetRotation;
        barraRapida[index].select_item.SetActive(true);

        isRotating = false;
    }
}
