using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool isCrouching;
    private bool isClimbing;
    private bool hasHorizontalCollision;
    private bool wasGrounded;

    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip carrotSound;
    public float deathYThreshold = -10f;
    public int carrotCount = 0;
    public GameObject winCanvas;
    public GameObject HUDCanvas;
    public Text livesText;
    public Text carrotsText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        wasGrounded = isGrounded;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (!isCrouching)
        {
           rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isCrouching = true;
            GetComponent<BoxCollider2D>().size = new Vector2(0.483156f, 0.4361324f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0.08361232f, 0.2180662f);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
            GetComponent<BoxCollider2D>().size = new Vector2(0.4285704f, 0.6794069f);
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.02870198f, 0.3401934f);
        }

        animator.SetBool("isCrouching", isCrouching);

        if (horizontalInput != 0 && !isCrouching)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            audioSource.PlayOneShot(jumpSound);
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        if (isClimbing)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
            animator.SetBool("isClimbing", true);

            if (verticalInput == 0)
            {
                rb.gravityScale = 1;
            }
            else
            {
                rb.gravityScale = 0;
            }
        }
        else
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = 1;
        }

        if (hasHorizontalCollision && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (transform.position.y < deathYThreshold)
        {
            PlayerLifeSystem playerLifeSystem = GetComponent<PlayerLifeSystem>();
            if (playerLifeSystem != null)
            {
                playerLifeSystem.LoseLife();
            }
        }

        if (horizontalInput != 0 && !isCrouching && isGrounded)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.loop = false;
        }

        if (!wasGrounded && isGrounded)
        {
            audioSource.PlayOneShot(landSound);
        }

        wasGrounded = isGrounded;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        hasHorizontalCollision = CheckHorizontalCollision();

        Collider2D finishTrigger = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Finish"));
        if (finishTrigger != null)
        {
            Debug.Log("Triggered: " + finishTrigger.gameObject.tag);
            if (finishTrigger.gameObject.tag == "Finish")
            {
                StartCoroutine(ShowWinCanvas());
            }
            else if (finishTrigger.gameObject.tag == "Finish2")
            {
                StartCoroutine(ShowWinCanvasLevel2());
            }
        }

        Collider2D spikeTrigger = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Spike"));
        if (spikeTrigger != null && spikeTrigger.gameObject.tag == "Spike")
        {
            PlayerLifeSystem playerLifeSystem = GetComponent<PlayerLifeSystem>();
            if (playerLifeSystem != null)
            {
                playerLifeSystem.LoseLife();
            }
        }
    }

    private bool CheckHorizontalCollision()
    {
        float direction = transform.localScale.x;
        float distance = playerCollider.bounds.extents.x + 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(playerCollider.bounds.center, new Vector2(direction, 0), distance, groundLayer);

        return hit.collider != null;
    }

    public void AddCarrot()
    {
        carrotCount++;
        HUDController.instance.UpdateCarrotCount(carrotCount);

        carrotsText.text = carrotCount.ToString();

        if (carrotSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(carrotSound);
        }
    }

    private void LoseGame()
    {
        SceneManager.LoadScene("GameOver");
    }

    /*public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered: " + other.gameObject.tag);
        if (other.gameObject.tag == "Finish")
        {
            ShowWinCanvas();
        }
    }*/

    public IEnumerator ShowWinCanvas()
    {
        HUDCanvas.SetActive(false);

        winCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        winCanvas.SetActive(false);

        SceneManager.LoadScene("Level2");
    }

    public IEnumerator ShowWinCanvasLevel2()
    {
        HUDCanvas.SetActive(false);

        winCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        winCanvas.SetActive(false);

        SceneManager.LoadScene("Credits");
    }
}