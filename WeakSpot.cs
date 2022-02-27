using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    public GameObject objectToDestroy;

    private void OnTriggerEnter2D(Collider2D collision) // Détecte une collision
    {
        if (collision.CompareTag("Player")) // Si collision avec le joueur
        {
            //Destroy(transform.parent.gameObject);
            Destroy(objectToDestroy); // Détruit l'ennemi et ces composants
        }
    }
}
