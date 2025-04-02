using UnityEngine;
using UnityEngine.SceneManagement;  // Nécessaire pour recharger la scène

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        // Récupère le nom de la scène actuelle
        SceneManager.LoadScene("SampleScene");

    }
}
