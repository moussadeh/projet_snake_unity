// using UnityEngine;
// using System.Collections;

// public class FlushSystem : MonoBehaviour
// {
//     [Header("Params de la chasse d'eau")]
//     [SerializeField] private float minTimeBetweenFlush = 20f;
//     [SerializeField] private float maxTimeBetweenFlush = 40f;
//     [SerializeField] private float warningDuration = 3f;
//     [SerializeField] private float flushDuration = 5f;
    
//     [Header("Refs")]
//     [SerializeField] private GameObject warningVisual;
//     [SerializeField] private GameObject flushVisual;
//     [SerializeField] private AudioSource warningSound;
//     [SerializeField] private AudioSource flushSound;
//     [SerializeField] private Transform[] safeZones;
//     [SerializeField] private float safeZoneRadius = 1.0f; //TODO
//     [SerializeField] private FlushAnimation flushAnimation;
    
//     [Header("Snake Refs")]
//     [SerializeField] private SnakeController snakeController;
    
//     private bool isFlushActive = false;
//     private float nextFlushTime;
    
//     private void Start()
//     {
//         // Initialiser le temps avant la premiere chasse d'eau
//         SetNextFlushTime();
        
//         // desactive les visuels au start
//         warningVisual.SetActive(false);
//         flushVisual.SetActive(false);
//     }
    
//     private void Update()
//     {
//         if (!isFlushActive && Time.time >= nextFlushTime)
//         {
//             StartCoroutine(ActivateFlush());
//         }
//     }
    
//     private void SetNextFlushTime()
//     {
//         nextFlushTime = Time.time + Random.Range(minTimeBetweenFlush, maxTimeBetweenFlush);
//     }
    
//     private IEnumerator ActivateFlush()
//     {
//         isFlushActive = true;
        
//         // Phase d'avertissement
//         warningVisual.SetActive(true);
//         if (warningSound != null)
//             warningSound.Play();
        
//         yield return new WaitForSeconds(warningDuration);
        
//         // Phase de chasse d'eau
//         warningVisual.SetActive(false);
//         // flushVisual.SetActive(true);
//         flushAnimation.PlayFlushAnimation();
//         if (flushSound != null)
//             flushSound.Play();
        
        
//         if (!IsSnakeInSafeZone())
//         {
            
//             snakeController.Die();
//         }
        
//         yield return new WaitForSeconds(flushDuration);
        
//        //reset all
//         flushVisual.SetActive(false);
//         isFlushActive = false;
//         SetNextFlushTime();
//     }
    
//     // private bool IsSnakeInSafeZone()
//     // {
//     //     Transform snakeHead = snakeController.GetSnakeHead();
        
//     //     foreach (Transform safeZone in safeZones)
//     //     {
//     //         //check snake head in the zone
//     //         // Utiliser des colliders serait plus efficace
//     //         Collider2D safeZoneCollider = safeZone.GetComponent<Collider2D>();
//     //         if (safeZoneCollider != null && safeZoneCollider.bounds.Contains(snakeHead.position))
//     //         {
//     //             return true;
//     //         }
//     //     }
        
//     //     return false;
//     // }

   

// private bool IsSnakeInSafeZone()
// {
//     Transform snakeHead = snakeController.GetSnakeHead();
//     if (snakeHead == null) return false;

//     foreach (Transform safeZone in safeZones)
//     {
//         if (Vector3.Distance(snakeHead.position, safeZone.position) <= safeZoneRadius)
//         {
//             return true;
//         }
//     }
//     return false;
// }
    
//     // Pour l'interface visuelle, we can add
//     public float GetTimeUntilNextFlush()
//     {
//         return nextFlushTime - Time.time;
//     }
// }

// public class SnakeController
// {
//     private Ver verInstance; // Reference to the Ver script

//     public SnakeController(Ver ver)
//     {
//         verInstance = ver;
//     }

//     public Transform GetSnakeHead()
//     {
//         // Returns the head of the snake (first segment)
//         return verInstance != null && verInstance.segments.Count > 0 ? verInstance.segments[0] : null;
//     }

//     public void Die()
//     {
//         Debug.Log("Le ver a été emporté par la chasse d'eau!");
//         // Game over logic
//     }
// }


using UnityEngine;
using System.Collections;

public class FlushSystem : MonoBehaviour
{
    [Header("Paramètres de la chasse d'eau")]
    [SerializeField] private float minTimeBetweenFlush = 20f;
    [SerializeField] private float maxTimeBetweenFlush = 40f;
    [SerializeField] private float warningDuration = 3f;
    [SerializeField] private float flushDuration = 5f;
    
    [Header("Références")]
    [SerializeField] private GameObject warningVisual;
    [SerializeField] private FlushAnimation flushAnimation;
    [SerializeField] private AudioSource warningSound;
    [SerializeField] private Transform[] safeZones;
    
    [Header("Snake References")]
    [SerializeField] private Transform snakeHead;
    
    private bool isFlushActive = false;
    private float nextFlushTime;
    
    private void Start()
    {
        // Initialiser le temps avant la première chasse d'eau
        SetNextFlushTime();
        
        // Désactiver les visuels au démarrage
        if (warningVisual != null)
            warningVisual.SetActive(false);
    }
    
    private void Update()
    {
        if (!isFlushActive && Time.time >= nextFlushTime)
        {
            StartCoroutine(ActivateFlush());
        }
    }
    
    private void SetNextFlushTime()
    {
        nextFlushTime = Time.time + Random.Range(minTimeBetweenFlush, maxTimeBetweenFlush);
    }
    
    private IEnumerator ActivateFlush()
    {
        isFlushActive = true;
        
        // Phase d'avertissement
        if (warningVisual != null)
            warningVisual.SetActive(true);
            
        if (warningSound != null)
            warningSound.Play();
        
        yield return new WaitForSeconds(warningDuration);
        
        // Phase de chasse d'eau
        if (warningVisual != null)
            warningVisual.SetActive(false);
            
        if (flushAnimation != null)
            flushAnimation.PlayFlushAnimation();
        
        // Vérifier si le serpent est dans une zone de sécurité
        if (!IsSnakeInSafeZone())
        {
            // Game over si le serpent n'est pas dans une zone sûre
            GameOver();
        }
        
        yield return new WaitForSeconds(flushDuration);
        
        // Remettre tout à zéro
        isFlushActive = false;
        SetNextFlushTime();
    }
    
    private bool IsSnakeInSafeZone()
    {
        // Vérifier si la tête du serpent n'existe pas
        if (snakeHead == null)
            return false;
            
        foreach (Transform safeZone in safeZones)
        {
            // Vérifier si la zone sécurisée existe
            if (safeZone == null)
                continue;
                
            // Vérifier si la tête du serpent est dans une zone de sécurité
            Collider2D safeZoneCollider = safeZone.GetComponent<Collider2D>();
            if (safeZoneCollider != null && safeZoneCollider.bounds.Contains(snakeHead.position))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private void GameOver()
    {
        Debug.Log("Le ver a été emporté par la chasse d'eau!");
        // Implémentez ici votre logique de game over
        // Par exemple, vous pourriez appeler une méthode sur votre GameManager
        
        // Si vous avez un GameManager avec une méthode GameOver()
        // GameManager.Instance.GameOver();
    }
    
    // Pour l'interface visuelle, vous pouvez ajouter cette méthode
    public float GetTimeUntilNextFlush()
    {
        return nextFlushTime - Time.time;
    }
}