using UnityEngine;
using UnityEngine.UI;

public class CharacterElements : MonoBehaviour
{
    [Header("Interface")]
    public Image characterlifeBar;
    public Image characterfomeBar;
    public Image charactersedeBar;

    public GameObject txtLife;

    [Header("FX")]
    public GameObject hitVinheta;
    public GameObject blood_fx;

    [HideInInspector] public ParticleSystem dust_fx;

    [Header("Inventario")]
    public GameObject inventario_interface;

    void Start()
    {
        dust_fx = transform.Find("Dust").GetComponent<ParticleSystem>();
    }

}
