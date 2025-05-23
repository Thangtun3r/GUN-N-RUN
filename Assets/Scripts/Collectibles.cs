using System;
                    using System.Collections;
                    using UnityEngine;
                    
                    public class Collectibles : MonoBehaviour
                    {
                        private Transform playerPosition;
                        private bool isCollecting = false;
                        private Vector3 initialPosition;
                        private Vector3 velocity = Vector3.zero;
                    
                        [Header("Follow Settings")]
                        public Vector3 followOffset = new Vector3(0.5f, 1f, 0f);
                        public float followSpeed = 5f;
                    
                        private Coroutine collectTimerCoroutine;
                    
                        [Header("Spin Reference")]
                        public CollectibleSpin spinComponent; // Assign in Inspector
                    
                        [Header("Visuals & Effects")]
                        public GameObject spriteObject; // Assign the Sprite GameObject in Inspector
                        public ParticleSystem playerCollectEffect; // Assign the particle effect in Inspector
                    
                        private void Awake()
                        {
                            initialPosition = transform.position;
                        }
                    
                        private void OnEnable()
                        {
                            LosingEvent.onPlayerDeath += ResetCollectible;
                        }
                    
                        private void OnDisable()
                        {
                            LosingEvent.onPlayerDeath -= ResetCollectible;
                        }
                    
                        private void OnTriggerEnter2D(Collider2D other)
                        {
                            if (other.CompareTag("Player") && !isCollecting)
                            {
                                Debug.Log("Collectible collected!");
                                playerPosition = other.transform;
                                isCollecting = true;
                    
                                if (spinComponent != null)
                                    spinComponent.enabled = false;
                    
                                if (collectTimerCoroutine == null)
                                    collectTimerCoroutine = StartCoroutine(StartTime());
                            }
                        }
                    
                        private void Update()
                        {
                            if (!isCollecting || playerPosition == null) return;
                            FollowPlayer();
                        }
                    
                        private void FollowPlayer()
                        {
                            Vector3 targetPos = playerPosition.position + followOffset;
                            transform.position = Vector3.SmoothDamp(
                                transform.position,
                                targetPos,
                                ref velocity,
                                1f / followSpeed
                            );
                        }
                    
                        private void ConfirmCollect()
                        {
                            StartCoroutine(CollectAndDestroy());
                        }
                    
                        private IEnumerator CollectAndDestroy()
                        {
                            if (spriteObject != null)
                                spriteObject.SetActive(false);
                    
                            if (playerCollectEffect != null)
                                playerCollectEffect.Play();
                    
                            yield return new WaitForSeconds(0.5f);
                    
                            Destroy(gameObject);
                        }
                    
                        private void ResetCollectible()
                        {
                            if (!isCollecting) return;
                    
                            if (collectTimerCoroutine != null)
                            {
                                StopCoroutine(collectTimerCoroutine);
                                collectTimerCoroutine = null;
                            }
                    
                            isCollecting = false;
                            playerPosition = null;
                            transform.position = initialPosition;
                    
                            if (spinComponent != null)
                                spinComponent.enabled = true;
                    
                            if (spriteObject != null)
                                spriteObject.SetActive(true);
                        }
                    
                        IEnumerator StartTime()
                        {
                            float timer = 0f;
                            float requiredTime = 0.5f;
                    
                            while (true)
                            {
                                if (Movement.isOnSafeGround)
                                {
                                    timer += Time.deltaTime;
                                    if (timer >= requiredTime)
                                    {
                                        ConfirmCollect();
                                        yield break;
                                    }
                                }
                                else
                                {
                                    timer = 0f;
                                }
                    
                                yield return null;
                            }
                        }
                    }