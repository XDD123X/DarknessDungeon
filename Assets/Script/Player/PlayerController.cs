using Assets.Scripts.Examples.Interface;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : EntityInterface
{
    private TrailRenderer myTrailRenderer;

    private Weapon weapon;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private float currSpeed;

    public bool FacingLeft = false;

    private Knockback knockback;
    private Flash flash;

    private AudioSource myAudioSource;
    private SoundSettings soundSettings;

    [SerializeField] private float knockBackThrust = 15f;
    //private Knockback knockback;
    private void Awake()
    {

        currSpeed = MoveSpeed;
        CanAttack = true;
        CanDash = true;
        CanMove = true;
        IsAttacking = false;
        IsAttacked = false;
        IsDashing = false;
        IsDead = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        weapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
        weapon.ChangeWeapon(null);
        myTrailRenderer = GetComponent<TrailRenderer>();
        myTrailRenderer.emitting = false;
        myAudioSource = GetComponent<AudioSource>();
        soundSettings = GameObject.FindGameObjectWithTag("Setting").GetComponent<SoundSettings>();
    }

    public void Test()
    {
        BuffSettings bf = GameObject.FindGameObjectWithTag("Setting").GetComponent<BuffSettings>();

        Buff.ApplyBuff(bf.buff_ATK, this.gameObject);
        Buff.ApplyBuff(bf.buff_HP, this.gameObject);
        Buff.ApplyBuff(bf.buff_CRIT, this.gameObject);
        Buff.ApplyBuff(bf.buff_DEF, this.gameObject);
    }

    public void Test2()
    {
        BuffSettings bf = GameObject.FindGameObjectWithTag("Setting").GetComponent<BuffSettings>();
        BuffHolder bh = gameObject.GetComponent<BuffHolder>();
        Debug.Log(bh.GetRemainingDuration(bf.buff_ATK));
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead && CurrentHP <= 0)
        {
            myAnimator.SetTrigger("isDead");
            IsDead = true;

        }
        if (IsDead)
        {
            return;
        }
        if (CanMove)
        {
            Move();
        }
        if (CanAttack)
        {
            Attack();
        }

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsAttacking = true;
            weapon.Attack();
            CanAttack = false;
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(this.AttackSpeed);

        IsAttacking = false;
        CanAttack = true;
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(horizontalInput * MoveSpeed, verticalInput * MoveSpeed);
        if (horizontalInput < 0)
        {
            FacingLeft = true;
            myAnimator.SetBool("isLeft", FacingLeft);
        }
        else if (horizontalInput > 0)
        {
            FacingLeft = false;
            myAnimator.SetBool("isLeft", FacingLeft);
        }
        if ((horizontalInput != 0) || (verticalInput != 0))
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
        myAnimator.SetBool("isMoving", IsMoving);

    }
    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        PlaySound("Player");
    }
    public void TakeDamage(Transform damageSource, float damage)
    {
        CurrentHP -= damage;
        PlaySound("Player");
        knockback.GetKnockedBack(damageSource, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());

    }
    public void Dead()
    {
        InventoryManagement inventoryManagement = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryManagement>();
        inventoryManagement.YouDead();
        CanMove = false;
        CanAttack = false;
        CanDash = false;
    }
    public void PlaySound(String sound)
    {
        switch (sound)
        {
            case "Sword":
                {
                    myAudioSource.PlayOneShot(soundSettings.SwordSlash, 0.6f);
                    break;
                }
            case "Staff":
                {
                    myAudioSource.PlayOneShot(soundSettings.StaffAttack, 2f);
                    break;
                }
            case "Bow":
                {
                    myAudioSource.PlayOneShot(soundSettings.BowAttack);
                    break;
                }
            case "Player":
                {
                    myAudioSource.PlayOneShot(soundSettings.PlayerBeingAttacked, 0.15f);
                    break;
                }
            case "Enemy":
                {
                    myAudioSource.PlayOneShot(soundSettings.EnemyBeingAttacked, 0.15f);
                    break;
                }
        }
    }
}
