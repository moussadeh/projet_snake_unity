using UnityEngine;

public class FlushAnimation : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Animator waterAnimator;
    [SerializeField] private AudioSource flushSound;
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Paramètres")]
    [SerializeField] private float animationDuration = 3f;
    [SerializeField] private float intervalBetweenFlush = 8f;

    private bool isPlaying = false;

    private void Start()
    {
        // Cacher l'objet au démarrage
        gameObject.SetActive(false);
        
        // Créer un GameObject temporaire pour gérer la coroutine
        GameObject controller = new GameObject("FlushController");
        controller.AddComponent<FlushController>().Initialize(this, intervalBetweenFlush, animationDuration);
        
        // Faire en sorte que le controller persiste
        DontDestroyOnLoad(controller);
    }

    public void PlayFlushAnimation()
    {
        if (isPlaying) return;

        Debug.Log("Animation flush DÉMARRE");
        isPlaying = true;
        gameObject.SetActive(true);

        // Démarrer l'animation
        if (waterAnimator != null)
            waterAnimator.SetTrigger("FlushTrigger");

        // Jouer les particules
        if (waterParticles != null)
        {
            waterParticles.Clear();
            waterParticles.Play();
        }

        // Jouer le son
        if (flushSound != null)
            flushSound.Play();

        // Programmer la fin de l'animation
        Invoke(nameof(StopFlushAnimation), animationDuration);
    }

    public void StopFlushAnimation()
    {
        Debug.Log("Animation flush FINIE");
        
        // Arrêter les particules
        if (waterParticles != null)
            waterParticles.Stop();
        
        if(waterAnimator != null)
        {
            waterAnimator.ResetTrigger("FlushTrigger");
            waterAnimator.SetTrigger("StopTrigger");
        }

        isPlaying = false;
        gameObject.SetActive(false);
    }
}

// Classe helper pour gérer la coroutine
public class FlushController : MonoBehaviour
{
    private FlushAnimation flushAnimation;
    private float interval;
    private float duration;

    public void Initialize(FlushAnimation animation, float intervalTime, float animDuration)
    {
        flushAnimation = animation;
        interval = intervalTime;
        duration = animDuration;
        
        StartCoroutine(FlushLoop());
    }

    private System.Collections.IEnumerator FlushLoop()
    {
        Debug.Log("FLUSH CONTROLLER: Attente de " + interval + " secondes avant première animation");
        
        // Attendre 10 secondes avant la première animation
        yield return new WaitForSeconds(interval);
        
        while (flushAnimation != null)
        {
            Debug.Log("FLUSH CONTROLLER: Lancement animation");
            flushAnimation.PlayFlushAnimation();
            
            // Attendre que l'animation finisse (5s)
            yield return new WaitForSeconds(duration);
            
            Debug.Log("FLUSH CONTROLLER: Attente de " + interval + " secondes avant prochaine animation");
            
            // Attendre 10 secondes avant la prochaine
            yield return new WaitForSeconds(interval);
        }
    }
}