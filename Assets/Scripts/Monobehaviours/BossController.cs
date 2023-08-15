using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//break up this class by enemy type later on
public class BossController : MonoBehaviour
{
    private Animator animator;
    private Camera cam;
    private CircleCollider2D col;
    private Rigidbody2D rb;
    private Hitbox hitbox;
    private BossStatistics stats;
    private AudioSource sound;
    public AudioClip[] clips;
    private bool dead;
    bool idle;
    bool moving;
    public Transform firePath;
    [HideInInspector] public Vector3 tempV;
    private Vector2 lookVector;
    private float timeToCount;
    public float shootingWait;
    ObjectPooler objectPooler;

    void Awake()
    {
        stats = GetComponent<BossStatistics>();
        animator = GetComponent<Animator>();
        hitbox = GetComponent<Hitbox>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();
        sound = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        //EventManager.onPlayerJoined += this.OnPlayerJoined;
        timeToCount = shootingWait;
        objectPooler = ObjectPooler.Instance;
    }
    
    void Update()
    {
        
    }
    
    // This will get called by animation events to grab a prefab from a pool and set it active and give it velocity
    IEnumerator ShootProjectile()
    {
        animator.SetBool("Fire", false);
        animator.SetTrigger("Shoot");
        sound.clip = clips[1];
        sound.Play();

        while (timeToCount > 0)
        {
            timeToCount -= Time.deltaTime;

            yield return null;
        }

        animator.SetBool("Fire", true);
        //firePath.right = (((Vector3)lookVector + firePath.position) - transform.position) + transform.position;
        firePath.right = ((Vector3)lookVector - transform.position) + transform.position;
        
        GameObject ammoInstance;
        ammoInstance = objectPooler.SpawnFromPool("EG2 Bullet", firePath.position, firePath.right, firePath.rotation);
        //ammoInstance = objectPooler.GetPooledObject();

        //specialReady = false; //delete this later, it does nothing

        float tempTime = 1f;

        while (tempTime > 0)
        {
            tempTime -= Time.deltaTime;

            yield return null;
        }
        
        timeToCount = shootingWait;
    }
}
