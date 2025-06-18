using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float speed = 0f;
    private bool isFacingRight = false;
    private Rigidbody2D rb;
    private Animator anim;

    private float h;
    private float v;
    private Vector2 input;
    public Inventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        inventory.inventoryUI.RefreshToolbar();
    }

    void Update()
    {
        AnimationControls();
        FlipController();
        GetInput();

        if (Input.inputString.Length > 0)
        {
            char key = Input.inputString[0];
            if (key >= '1' && key <= '3')
            {
                int index = key - '1';
                inventory.inventoryUI.SelectToolbarSlot(index);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }

    private void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        input = new Vector2(h, v);
        input.Normalize();
    }

    /* Animation trigger controls */
    private void AnimationControls()
    {
        if ((input.magnitude > 0.1f) || (input.magnitude < -0.1f))
        {
            anim.SetBool("Moving", true);

        }
        else
        {
            anim.SetBool("Moving", false);
        }

    }
    /* Flip Character */
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FlipController()
    {
        if (rb.linearVelocity.x < 0 && isFacingRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x > 0 && !isFacingRight)
        {
            Flip();
        }

    }


    public void DropItem(Item item)
    {
        if (item == null || item.data == null)
        {
            Debug.LogWarning("Tentou dropar item nulo");
            return;
        }

        GameManager.instance.itemManager.RemoveCollectedItem(item.data.itemName);

        Vector3 spawnLocation = transform.position;
        Vector2 rawOffset = Random.insideUnitCircle.normalized * Random.Range(0.7f, 1.25f);

        Item droppedItem = Instantiate(item, spawnLocation + (Vector3)rawOffset, Quaternion.identity);
        droppedItem.data = item.data;

        Collectable collectable = droppedItem.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.SetCollected(false);  // vamos criar esse método abaixo
        }

        droppedItem.rb.AddForce(rawOffset * 2f, ForceMode2D.Impulse);
    }
}
