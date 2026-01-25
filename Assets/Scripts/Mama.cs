using UnityEngine;
using UnityEngine.SceneManagement;

public class Mama : MonoBehaviour
{

    [SerializeField] private Material materialPared;
    [SerializeField] private Material materialTecho;
    [SerializeField] private AudioClip cancion2;
    [SerializeField] private AudioSource reproductorMusica;
    
    
    private int contadorMuertes = 0;
    private GameObject[] paredes;
    private GameObject[] techo;
    
    
    private void OnEnable()
    {
        RespawnAction.OnRespawn += Muertes;
    }
    
    private void OnDisable()
    {
        RespawnAction.OnRespawn -= Muertes;
    }
    
    private void Muertes()
    {
        contadorMuertes++;

        if (contadorMuertes >= 3)
        {
           CambiarEscenario();
        }

        if (contadorMuertes >= 5)
        {
            Derrota();
        }
        
    }

    private void Derrota()
    {
        SceneManager.LoadScene("Derrota");
        
    }

    private void CambiarEscenario()
    {
        
        reproductorMusica.Stop();             
        reproductorMusica.clip = cancion2;    
        reproductorMusica.Play();
        
        paredes = GameObject.FindGameObjectsWithTag("Pared");
        techo = GameObject.FindGameObjectsWithTag("Techo");
        
        foreach (GameObject pared in paredes)
        {
           Renderer rend = pared.GetComponent<Renderer>();
           rend.material = materialPared;
        } 
        foreach (GameObject techo in techo)
        {
           Renderer rend = techo.GetComponent<Renderer>();
           rend.material = materialTecho;
        }
    }


    
}
