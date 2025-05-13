    using UnityEngine;
    using System.Collections;

    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float propelForce = 10f;
        [SerializeField] private float downforce = 5f;

        [Header("Ammo Settings")] 
        public int maxAmmo = 5;

        [Header("Pivot")] 
        public GameObject pivot;

        [Header("Ground Detection")] 
        public LayerMask groundLayer;
        public float groundRayDistance = 0.2f;

        Rigidbody2D rb;
        Camera mainCam;
        Vector2 propelDir;
        bool isGrounded = false;
        int currentAmmo;
        bool isReloading;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCam = Camera.main;
            currentAmmo = maxAmmo;
        }

        void Update()
        {
            RotatePivotTowardMouse();
            HandlePropelInput();
        }

        void FixedUpdate()
        {
            ApplyDownforce();
            HandleGroundCheck();
        }

        void RotatePivotTowardMouse()
        {
            if (pivot == null) return;

            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pivotPos = pivot.transform.position;
            Vector2 dir = (mouseWorld - pivotPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        void HandlePropelInput()
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
            {
                Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;
                Vector2 toMouse = mouseWorld - transform.position;
                propelDir = -toMouse.normalized;

                Propel();
            }
        }

        void HandleGroundCheck()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayDistance, groundLayer);
            isGrounded = hit.collider != null;

            if (isGrounded)
            {
                currentAmmo = maxAmmo;
            }
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


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * groundRayDistance);
        }
    }
