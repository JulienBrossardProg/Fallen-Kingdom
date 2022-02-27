using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed; // Vitesse
    public Transform[] waypoints; // Points de destination

    public int damageOnCollision = 20;
    public SpriteRenderer graphics; // Ennemi
    private Transform target; // Cible de destination
    private int destPoint = 0; // Initialise la destination à 0

    void Start()
    {
        target = waypoints[0]; // Initialise la première destination
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position; // Direction
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); // Déplacement

        // Si l'ennemi est quasiment arrivé à sa destination
        if(Vector3.Distance(transform.position, target.position) < 0.3f) 
        {
            destPoint = (destPoint + 1) % waypoints.Length; // Passe à la seconde destination
            target = waypoints[destPoint]; // Idem
            graphics.flipX = !graphics.flipX; // Flip l'image de l'ennemi
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageOnCollision);
        }
    }
}
