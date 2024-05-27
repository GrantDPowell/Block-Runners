using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float baseSpeed = 10.0f;
    private float jumpForce = 100.0f;
    public float gravityScale = 2.0f;
    public LayerMask groundLayer;
    private Rigidbody rb;
    private int score;
    public Text scoreText;
    public Text speedText;
    public Text velocityText;
    public AnimationCurve jumpCurve;

    private float currentSpeed;
    private float topSpeed;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTime;
    private int comboCount;
    private int maxCombo;

    public Text comboText;
    private HealthSystem healthSystem;

    public int Score { get { return score; } }
    public float TopSpeed { get { return topSpeed; } }
    public int MaxCombo { get { return maxCombo; } }


    // Sound effects
    public AudioClip itemPointClip;
    public AudioClip obstacleClip;
    public AudioClip jumpClip;
    public AudioClip gameOverClip;

    private AudioSource itemPointAudioSource;
    private AudioSource obstacleAudioSource;
    private AudioSource jumpAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        score = 0;
        currentSpeed = baseSpeed;
        topSpeed = baseSpeed;
        comboCount = 0;
        maxCombo = 0;

        healthSystem = GetComponent<HealthSystem>();

        // Initialize AudioSource components
        itemPointAudioSource = gameObject.AddComponent<AudioSource>();
        obstacleAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource = gameObject.AddComponent<AudioSource>();

        // Assign audio clips
        itemPointAudioSource.clip = itemPointClip;
        obstacleAudioSource.clip = obstacleClip;
        jumpAudioSource.clip = jumpClip;

        // Optional: Set volume for each AudioSource
        itemPointAudioSource.volume = 1f;
        obstacleAudioSource.volume = 1f;
        jumpAudioSource.volume = 0.07f;

        SetScoreText();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = new Vector3(movement.x * currentSpeed / 2, rb.velocity.y, 1 * currentSpeed); // Set the horizontal velocity directly

        if (currentSpeed > topSpeed)
        {
            topSpeed = currentSpeed; // Update top speed if the current speed exceeds it
        }

        if (isJumping)
        {
            jumpTime += Time.fixedDeltaTime;
            float curveValue = jumpCurve.Evaluate(jumpTime);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * curveValue, rb.velocity.z);
        }

        ApplyGravity();
        SetScoreText(); // Update the score and velocity text each frame
    }

    void Update()
    {
        CheckGrounded(); // Check if the player is on the ground

        if (isGrounded && !isJumping && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.J)))
        {
            isJumping = true;
            jumpTime = 0;
            PlaySound(jumpAudioSource); // Play jump sound
        }

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!isJumping)
        {
            rb.velocity += Vector3.down * gravityScale * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("itempoint"))
        {
            comboCount++;
            if (comboCount > maxCombo)
            {
                maxCombo = comboCount; // Update max combo if the current combo count exceeds it
            }
            other.gameObject.SetActive(false);
            score += Mathf.RoundToInt(currentSpeed) * comboCount;
            currentSpeed += baseSpeed * 0.08f; // Increase speed by 10% of the base speed
            SetScoreText();
            PlaySound(itemPointAudioSource); // Play item point sound
        }
        else if (other.gameObject.CompareTag("obstacle"))
        {
            comboCount = 0;
            other.gameObject.SetActive(false);
            healthSystem.TakeDamage(1);
            score -= Mathf.RoundToInt(currentSpeed) / 2;
            currentSpeed -= baseSpeed * 0.2f; // Decrease speed by 20% of the base speed
            SetScoreText();
            PlaySound(obstacleAudioSource); // Play obstacle sound
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("boundary"))
        {
            Vector3 reflectDir = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = reflectDir; // Reflect the player's velocity upon collision with the boundary
        }
    }

    void SetScoreText()
    {
        Vector3 velocity = rb.velocity;
        velocityText.text = "Velocity: " + "X: " + Mathf.RoundToInt(velocity.x) + " Y: " + Mathf.RoundToInt(velocity.y) + " Z: " + Mathf.RoundToInt(velocity.z);
        comboText.text = "Combo: " + comboCount.ToString();
        speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed).ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
