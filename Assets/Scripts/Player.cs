using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get => instance; }
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    public RecipeScriptable holdingRecipe { get; private set; }
    public MaterialScriptable holdingMaterial { get; private set; }
    private UIController ui;
    private SpriteRenderer sr;

    [Header("Footsteps")]
    [SerializeField] private List<AudioClip> steps;
    [SerializeField] private float timeInbeetween;
    private float lastTimeStep;
    private bool step0 = true;

    public Transform pointsSpawn;

    public bool canDrop { get;set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        rb = GetComponent<Rigidbody2D>();
        sr= GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        canDrop= true;
        animator= GetComponent<Animator>();
        ui = UIController.Instance;
        lastTimeStep = Time.time;
    }
    void Update()
    {
        if (GameController.Instance.Ongoing == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if(movement == Vector2.zero)
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                if(Mathf.Abs(movement.x)>Mathf.Abs(movement.y))
                {
                    if(movement.x<0)
                    {
                        animator.SetTrigger("WalkLeft");
                    }
                    else
                    {
                        animator.SetTrigger("WalkRight");
                    }
                }
                else
                {
                    if (movement.y < 0)
                    {
                        animator.SetTrigger("WalkFront");
                    }
                    else
                    {
                        animator.SetTrigger("WalkBack");
                    }
                }
            }
            if ((movement.x!=0||movement.y!=0)&&(Time.time-lastTimeStep)>timeInbeetween)
            {
                if(step0)
                {
                    AudioEvents.Instance.PlaySound(steps[0]);
                    step0 = false;
                    lastTimeStep= Time.time;
                }
                else
                {
                    AudioEvents.Instance.PlaySound(steps[1]);
                    step0 = true;
                    lastTimeStep = Time.time;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.I)&&canDrop)
        {
            DropCurrent();
        }
        sr.sortingOrder =  -Mathf.RoundToInt(transform.position.y);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * speed * Time.deltaTime;
    }

    public void GrabMaterial(MaterialScriptable toHold)
    {
        if(holdingMaterial == null && holdingRecipe == null)
        {
            holdingMaterial = toHold;
            ui.ChangeCurrentObjectSprite(toHold.materialIcon);
        }        
    }

    public void GrabRecipe(RecipeScriptable toHold)
    {
        if (holdingMaterial == null && holdingRecipe == null)
        {
            holdingRecipe = toHold;
            ui.ChangeCurrentObjectSprite(toHold.recipeSprite);
        }
    }
    public void DropCurrent()
    {
        if(holdingRecipe!= null)
        {
            FindObjectOfType<DeliverCollider>().ActiveHand(false);
        }
        holdingMaterial = null;
        holdingRecipe = null;
        ui.ChangeCurrentObjectSprite(null);
    }


}
