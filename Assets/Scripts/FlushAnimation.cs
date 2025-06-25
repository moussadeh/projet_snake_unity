using UnityEngine;

public class FlushAnimation : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private Animator waterAnimator;
    [SerializeField] private AudioSource flushSound;

    [Header("Paramètres")]
    [SerializeField] private float animationDuration = 5f;

    private bool isPlaying = false;

    private void Start()
    {
        // Désactiver l'objet au démarrage
        gameObject.SetActive(false);
        StartCoroutine(FlushLoop());
        // gameObject.SetActive(false); // Cacher au début
    }

    public void PlayFlushAnimation()
    {
        if (isPlaying) return;

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

    private void StopFlushAnimation()
    {
        // Arrêter l'animation et les effets
        if (waterParticles != null)
            waterParticles.Stop();

        isPlaying = false;
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator FlushLoop()
    {
        yield return new WaitForSeconds(30f); // Attendre 30s au début

        while (true)
        {
            PlayFlushAnimation();
            yield return new WaitForSeconds(animationDuration); // Attendre la fin de l'animation
            // L'objet est caché dans StopFlushAnimation
            yield return new WaitForSeconds(30f); // Attendre 30s avant de recommencer
        }
    }
}