using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Vitesse
    public float climbSpeed; // Vitesse de monter
    public float jumpForce; // Force de saut

    private bool isJumping; // Si saute ou non
    private bool isGrounded; // Si touche le sol ou non
    [HideInInspector]
    public bool isClimbing;

    public Transform groundCheck; // Si joueur touche le sol 
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb; // Corps du joueur
    public Animator animator; // Animator
    public SpriteRenderer spriteRenderer; // Sprite du joueur (pour flip)
    public CapsuleCollider2D playerCollider;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers); // Si touche le sol


        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime; // Mouvement horizontal
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing) // Si touche le sol et boutton Space appuyé
        {
            isJumping = true; // IsJumping devient vrai
        }

        MovePlayer(horizontalMovement, verticalMovement); // Déplacement du joueur

        Flip(rb.velocity.x); // Flip le joueur

        float characterVelocity = Mathf.Abs(rb.velocity.x); // Met la vitesse du joueur en positif
        animator.SetFloat("Speed", characterVelocity); // Refresh la vitesse pour l'animator (pour si Iddle ou Run)
        animator.SetBool("isClimbing", isClimbing);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement) // Déplacement du joueur
    {
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y); // Vecteur pour déplacer le joueur
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f); // Déplacement du joueur

            if (isJumping) // Si IsJumping vrai
            {
                rb.AddForce(new Vector2(0f, jumpForce)); // Saute
                isJumping = false; // IsJumping devient Faux
            }
        }
        else
        {
            // déplacement vertical
            Vector3 targetVelocity = new Vector2(0, _verticalMovement); // Vecteur pour déplacer le joueur
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f); // Déplacement du joueur

        }
    }

    void Flip(float _velocity) // Flip l'image
    {
        if(_velocity > 0.1f) // Si va à droite
        {
            spriteRenderer.flipX = false; // Flip l'image pour droite
        }else if(_velocity < -0.1f) // Si va à gauche
        {
            spriteRenderer.flipX = true; // Flip l'image pur gauche
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
