using UnityEngine;
using TMPro;

[DefaultExecutionOrder(100)]
public class MovementButMath : Player
{
    [Header("Settings")] 
    [SerializeField] private float propelForce = 10f;
    [SerializeField] private float downforce = 5f;

    [Header("Ammo Settings")] 
    public int maxAmmo = 5;

    [Header("Pivot")] 
    public GameObject pivot;

    [Header("Ground Check")]
    public GameObject groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundCheckLayer;
    public string safeGroundTag = "SafeGround";
    public static bool isOnSafeGround = false;
    
    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    private Vector2 velocity;
    private Camera mainCam;
    private Vector2 propelDir;
    private bool isGrounded = false;
    private int currentAmmo;
    private bool shouldPropel = false;

    void Awake()
    {
        mainCam = Camera.main;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        HandleControl();
        HandleGroundCheck();
        HandleUI();
        ApplyDownforce();
        HandleMovement();
    }

    void HandleControl()
    {
        if (pivot == null) return;

        Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pivotPos = pivot.transform.position;
        Vector2 dir = (mouseWorld - pivotPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Vector2 toMouse = mouseWorld - (Vector2)transform.position;
            propelDir = -toMouse.normalized;
            shouldPropel = true;
        }
    }

    void HandleGroundCheck()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundCheckLayer);

        isGrounded = hit != null;

        isOnSafeGround = hit != null && hit.CompareTag(safeGroundTag);

        if (isGrounded)
        {
            currentAmmo = maxAmmo;
            if (velocity.y < 0) velocity.y = 0; // stop downward velocity
        }
    }

    void HandleUI()
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }

    void ApplyDownforce()
    {
        // Only apply downforce if falling
        if (!isGrounded)
        {
            velocity += Vector2.down * downforce * Time.deltaTime;
        }
    }

    void HandleMovement()
    {
        if (shouldPropel)
        {
            velocity = propelDir * propelForce;
            currentAmmo--;
            shouldPropel = false;
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
    }
}
