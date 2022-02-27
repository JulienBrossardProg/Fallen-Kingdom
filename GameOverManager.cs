using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène");
            return;
        }

        instance = this;
    }

    public void OnPlayerDeath()
    {
        if (CurrentSceneManager.instance.isPlayerPresentByDefault)
        {
            DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
        }

        gameOverUI.SetActive(true);
    }

    public void RetryButton() 
    {
        // Reset les pièces
        Inventory.instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
        // Recommencer le niveau
        // Recharger la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Replacer le joueur au spawn
        PlayerHealth.instance.Respawn();
        // Réactiver les mouvements du joueur + qu'on lui rende sa vie
        gameOverUI.SetActive(false);
    }

    public void MainMenuButton()
    {
        DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
        // Retour au menu principal
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        // Fermer le jeu
        Application.Quit();
    }
}
