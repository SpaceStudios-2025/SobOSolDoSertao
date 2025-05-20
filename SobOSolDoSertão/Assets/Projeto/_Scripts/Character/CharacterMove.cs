using System.Collections;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [Header("Movimentacao")]
    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float RunSpeed = 6f;

    private float currentSpeed;
    private Vector3 moveDirection;

    private bool isFacing;
    private bool move = true;

    [Header("Animations")]
    [SerializeField] private Animator CharacterAnim;
    [SerializeField] private Animator ShadowAnim;

    private int transitions;

    private CharacterManager manager;

    void Start()
    {
        currentSpeed = WalkSpeed;
        move = true;
        manager = GetComponent<CharacterManager>();
    }

    void LateUpdate() => LateFuncoes();
    void Update() => Funcoes();


    public void LateFuncoes()
    {
        if (!Dead() && !CharacterManager.inventory)
        {
            //Todas as funcoes e mecanicas do personagem
            if(move) Movimentacao();
        }
        else AnimIdle();
    }

    public void Funcoes()
    {
        if (!Dead())
        {
            if (Input.GetButtonDown("Hit"))
            {
                manager.CharacterHit(15,true);
            }

            if (Input.GetButtonDown("Inventory"))
            {
                manager.Interface_Inventario();
            }
        }
    }

    void Movimentacao()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, y).normalized;

        //Velocidade
        currentSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Run") ? RunSpeed : WalkSpeed;

        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        //Direcao
        if (moveDirection.x < 0 && !isFacing) Flip();
        else if (moveDirection.x > 0 && isFacing) Flip();

        // Animations
        if (moveDirection != Vector3.zero) transitions = currentSpeed == RunSpeed ? 2 : 1;
        else transitions = 0;

        CharacterAnim.SetInteger("transition", transitions);
        ShadowAnim.SetInteger("transition", transitions);

    }
    bool Dead()
    {
        if (CharacterManager.dead)
        {
            CharacterAnim.SetTrigger("dead");
            ShadowAnim.SetTrigger("dead");
        }


        return CharacterManager.dead;
    }
    void Flip()
    {
        isFacing = !isFacing;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        var scaleShadow = transform.Find("Shadow").localScale;
        scaleShadow.x *= -1;
        transform.Find("Shadow").GetComponent<SpriteRenderer>().flipX = !transform.Find("Shadow").GetComponent<SpriteRenderer>().flipX;
        transform.Find("Shadow").localScale = scaleShadow;

        manager.DustFX();
    }

    public void HitAnim()
    {
        CharacterAnim.SetTrigger("hit");
        ShadowAnim.SetTrigger("hit");
        StartCoroutine(StopMove());
    }

    IEnumerator StopMove()
    {
        move = false;
        yield return new WaitForSeconds(.3f);
        move = true;
    }

    public void AnimIdle()
    {
        CharacterAnim.SetInteger("transition", 0);
        ShadowAnim.SetInteger("transition", 0);
    }

    public void BloodHit(GameObject blood)
    {
        var pos = new Vector3(transform.position.x, transform.position.y + 1f);
        var bloodFx = Instantiate(blood, pos, Quaternion.identity);
        if (isFacing)
        {
            var scale = bloodFx.transform.Find("fx").localScale;
            scale.x = -1;
            bloodFx.transform.Find("fx").localScale = scale;
        }
    }
}
