using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator anim;
    int curHP;
    int maxHP = 4;
    bool isHit = false;
    public Main main;
    public bool KeyYellow = false;
    bool canTP = true;
    public bool inWater = false;
    public bool isClimbing = false;
    int coin = 0;
    public bool canHit = true;
    public GameObject blueGem, greenGem;
    int GemCount = 0;
    public Sprite HasKeySprite;
    public SoundEffector soundEffector;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (Input.GetAxis("Horizontal") != 0)
                Flip();
        }
        else
        {

            CheckGround();


            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
                anim.SetInteger("State", 1);

            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
        }
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
        if (KeyYellow)
        GetComponent<SpriteRenderer>().sprite = HasKeySprite;

        
    }

    void FixedUpdate()
    {

    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);

    }

    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0 && canHit)
        {
            soundEffector.PlayHitSound();
            curHP = curHP + deltaHP;
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;

            StartCoroutine(OnHit());
        }
        else if (deltaHP > 0)
        {
            curHP = curHP + deltaHP;
        }


        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        //print(curHP);
        if (curHP <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()
    {

        /*if (isHit)
             GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
                GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        
        if (GetComponent<SpriteRenderer>().color.g == 1f)
        {
            print ("Стоп" +canHit);
            StopCoroutine(OnHit());
            
        }

        if (GetComponent<SpriteRenderer>().color.g <= 0)
            isHit = false;
            
        yield return new WaitForSeconds(0.2f);
        if (GetComponent<SpriteRenderer>().color.g != 1f)
        {
                StartCoroutine(OnHit());
                canHit = true;
        }*/


        if (isHit)
        {
            canHit = false;
            for (int hitcounts = 4; hitcounts > 0; hitcounts--)
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 1f);
                yield return new WaitForSeconds(0.2f);
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                yield return new WaitForSeconds(0.2f);
            }
            isHit = false;
            canHit = true;
        }


        if (GetComponent<SpriteRenderer>().color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.8f);
        //canHit = true;
        StartCoroutine(OnHit());

    }

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeadLine")
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
        
        if (collision.gameObject.tag == "KeyYellow")
        {
            Destroy(collision.gameObject);
            KeyYellow = true;
            
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coin++;
            soundEffector.PlayCoinSound();
            
        }

        if (collision.gameObject.tag == "Heart")
        {
            Destroy(collision.gameObject);
            RecountHP(1);
            //if (curHP < maxHP)
            //curHP ++;
        }

        if (collision.gameObject.tag == "Mushroom")
        {
            Destroy(collision.gameObject);
            RecountHP(-1);
        }

        if (collision.gameObject.tag == "Bluegem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(noHit());
        }

        if (collision.gameObject.tag == "Greengem")
        {
            Destroy(collision.gameObject);
            StartCoroutine(speedbonus());
        }

        if (collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }
            else if (KeyYellow)
            {
                collision.gameObject.GetComponent<Door>().Unlock();
            }
        }
    }

    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(2f);
        canTP = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
            rb.bodyType = RigidbodyType2D.Dynamic;
        isClimbing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tramp")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));

        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }


        {
            if (collision.transform.tag == "platform") //передаем персонажу скорость движущихся платформ
                transform.parent = collision.transform;
        }


    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform") //убираем у персонажа скорость платформы
            transform.parent = null;

        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);

    }

    IEnumerator noHit()
    {
        GemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);

        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        print("Вы неуязвимы");
        yield return new WaitForSeconds(8f);
        StartCoroutine(invisGem(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;
        print("Вы больше не неуязвимы");

        GemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }

    IEnumerator speedbonus()
    {
        GemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);

        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        print("Быстр как ветер");
        yield return new WaitForSeconds(4f);
        StartCoroutine(invisGem(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;
        print("Скорость обычная");

        GemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
    }

    void CheckGems(GameObject obj)
    {
        if (GemCount == 1)
            obj.transform.localPosition = new Vector3(0, 0.6f, 0);
        else if (GemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.6f, 0);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.6f, 0);
        }
    }

    IEnumerator invisGem(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(invisGem(spr, time));
    }

    public int GetCoins()
    {
        return coin;
    }

    public int GetHP()
    {
        return curHP;
    }

    public bool GetKey()
    {
        return KeyYellow;
    }


    //////////////////////////////////////самоделки

}
