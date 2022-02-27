using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player; // Player
    public float timeOffset; // Temps après le déplacement du joueur pour le suivre
    public Vector3 posOffset; // Position de décallage avec le joueur

    private Vector3 velocity;  // Vitesse

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset); // Déplacement
    }
}
