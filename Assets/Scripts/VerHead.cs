using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ver : MonoBehaviour
{
    Vector2 dir;
    List<Transform> segments = new List<Transform>();
    [SerializeField] Transform segmentPrefab;
    
    public int Score = 0;
    public TextMeshProUGUI TextScore;

    public GameObject spriteRenderer;  // Assure-toi que c'est un GameObject
    public Button btnRejouer;          // Assure-toi que c'est un Button
    public AudioSource audioSourceEat;
    public AudioSource audioSourceAmbiance;
    public AudioSource audioSourceLoose;

    void Start()
    {
        Time.timeScale = 0.18f;
        dir = Vector2.right;
        segments.Add(transform);

        TextScore = GameObject.Find("TextScore").GetComponent<TextMeshProUGUI>();
        
        // Vérifie si spriteRenderer est assigné avant de l'utiliser
        if (spriteRenderer != null)
        {
            spriteRenderer.SetActive(false);
        }
        else
        {
            Debug.LogError("spriteRenderer n'est pas assigné dans l'Inspector !");
        }

        // Vérifie si btnRejouer est assigné avant de l'utiliser
        if (btnRejouer != null)
        {
            btnRejouer.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("btnRejouer n'est pas assigné dans l'Inspector !");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && dir.x != 0)
        {
            dir = Vector2.up;
        } 
        else if (Input.GetKeyDown(KeyCode.DownArrow) && dir.x != 0)
        {
            dir = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir.y != 0)
        {
            dir = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir.y != 0)
        {
            dir = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Déplace la tête du serpent
        float x = Mathf.Round(transform.position.x) + dir.x;
        float y = Mathf.Round(transform.position.y) + dir.y;

        transform.position = new Vector3(x, y);
    }

    void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Molecule")
        {
            audioSourceEat.Play();
            Grow();
            Score += 1;
            TextScore.text = "Score : " + Score;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si spriteRenderer est assigné avant de l'utiliser
        if (spriteRenderer != null)
        {
            spriteRenderer.SetActive(true);  // Afficher le sprite lorsque la collision se produit
        }
        else
        {
            Debug.LogError("spriteRenderer n'est pas assigné dans l'Inspector !");
        }

        // Vérifie si btnRejouer est assigné avant de l'utiliser
        if (btnRejouer != null)
        {
            btnRejouer.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("btnRejouer n'est pas assigné dans l'Inspector !");
        }

        audioSourceAmbiance.Stop();
        audioSourceLoose.Play();
        Destroy(GetComponent<Ver>());
    }
}
