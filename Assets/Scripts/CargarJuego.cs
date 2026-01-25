using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarJuego : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BotonReload()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("SceneManager.GetActiveScene().name");
    }
}
