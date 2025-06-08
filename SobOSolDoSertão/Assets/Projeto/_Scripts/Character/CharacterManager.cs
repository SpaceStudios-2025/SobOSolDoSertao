using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Elementos do Character")]
    private CharacterElements elements;
    private CharacterMove controls;

    [Header("Statics Variables")]
    public static bool dead;
    public static bool inventory;

    [Header("Elements Character")]
    [SerializeField] private float maxLife;
    [SerializeField] private float maxFome;
    [SerializeField] private float maxSede;

    public float Life { get; private set; }
    public float Fome { get; private set; }
    public float Sede { get; private set; }

    Color defaultC;

    [Header("Hit Character")]
    public float durantion;
    public float magnitude;

    void Start()
    {
        DefaultSet();
    }

    void LateUpdate()
    {
        if (!dead)
        {
            ElementsCharacter();
        }
    }

    void DefaultSet()
    {
        dead = false;
        inventory = false;

        Life = maxLife;
        Fome = maxFome;
        Sede = maxSede;

        elements = GetComponent<CharacterElements>();
        controls = GetComponent<CharacterMove>();

        defaultC = elements.characterlifeBar.color;
    }

    bool hit = false;
    public void CharacterHit(float damage, bool atk)
    {
        if (!hit && !dead)
        {
            elements.txtLife.SetActive(true);
            elements.txtLife.GetComponent<TextMeshProUGUI>().text = "-" + damage;

            if (atk)
                controls.BloodHit(elements.blood_fx);

            StartCoroutine(TransitionHit(damage));

            ColorLife();

            if (Life <= 0) dead = true;
            //Animacao de hit
            controls.HitAnim();
            FindFirstObjectByType<CameraFollow>().Shake(durantion, magnitude);

            elements.hitVinheta.SetActive(true);
        }
    }

    IEnumerator TransitionHit(float damage)
    {
        hit = true;

        var endLife = Life - damage;
        while (Life > endLife)
        {
            Life -= Time.deltaTime * 60;
            elements.characterlifeBar.fillAmount = Life / maxLife;
            yield return null;
        }

        hit = false;
    }

    void ColorLife()
    {
        if (Life <= (maxLife / 4))
            elements.characterlifeBar.color = Color.red;
        else
            elements.characterlifeBar.color = defaultC;
    }

    bool hitC;
    void ElementsCharacter()
    {
        if (Fome > 0)
        {
            Fome -= Time.deltaTime / 20;
            elements.characterfomeBar.fillAmount = Fome / maxFome;

            Fome = Mathf.Clamp(Fome, 0, maxFome);

            if (Fome <= (maxFome / 3))
            {
                EnableAlert(elements.characterfomeBar);
            }
        }
        else
        {
            if (!hitC)
            {
                GameManager.controller.Alert("Voce está com Fome");
                StartCoroutine(HitCharacter());
            }
        }

        if (Sede > 0)
        {
            Sede -= Time.deltaTime /20;
            elements.charactersedeBar.fillAmount = Sede / maxSede;

            Sede = Mathf.Clamp(Sede, 0, maxSede);

            if (Sede <= (maxSede / 3))
            {
                EnableAlert(elements.charactersedeBar);
            }
        }
        else
        {
            if (!hitC)
            {
                GameManager.controller.Alert("Voce está com Sede");
                StartCoroutine(HitCharacter());
            }
        }
    }

    void EnableAlert(Image element)
    {
        element.GetComponentInParent<Animator>().SetBool("end", true);
    }

    IEnumerator HitCharacter()
    {
        hitC = true;
        CharacterHit(2f, false);
        yield return new WaitForSeconds(6f);
        hitC = false;
    }


    public void DustFX()
    {
        elements.dust_fx.Play();
    }

    #region Inventario

    public void Interface_Inventario()
    {
        elements.inventario_interface.SetActive(!elements.inventario_interface.activeSelf);
        inventory = elements.inventario_interface.activeSelf;
        Time.timeScale = inventory ? .2f : 1f;

        InventoryManager.inventoryOpen = inventory;
    }


    #endregion

}
