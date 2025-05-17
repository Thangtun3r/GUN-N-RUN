using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : Player
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
    private bool wasGrounded = false;
    public string safeGroundTag = "SafeGround";
    public static bool isOnSafeGround = false;
    
    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 propelDir;
    private bool isGrounded = false;
    private int currentAmmo;
    private bool isReloading;
    private bool shouldPropel = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        HandleControl();
        HandleGroundcheck();
        HandleUI();
    }

    void FixedUpdate()
    {
        if (shouldPropel)
        {
            Propel();
            shouldPropel = false;
        }

        ApplyDownforce();
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

    void HandleGroundcheck()
    {
        bool isNowGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundCheckLayer);

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundCheckLayer);

        if (hit != null && hit.CompareTag(safeGroundTag))
        {
            isOnSafeGround = true;
        }
        else
        {
            isOnSafeGround = false;
        }

        if (isNowGrounded)
        {
            currentAmmo = maxAmmo;
        }
    }

    void HandleUI()
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }

    void Propel()
    {
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = propelDir * propelForce;
        currentAmmo--;
    }

    void ApplyDownforce()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector2.down * downforce);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
    }
}
