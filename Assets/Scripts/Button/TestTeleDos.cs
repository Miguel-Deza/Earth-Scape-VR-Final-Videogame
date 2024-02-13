using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestTeleDos : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;

    // Referencia al objeto del jugador que tiene el script de movimiento
    public GameObject player;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;

        // Asegúrate de asignar la referencia apropiada desde el Inspector de Unity
        if (player == null)
        {
            Debug.LogError("No se ha asignado la referencia al objeto del jugador en el Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();

            // Teletransportar al jugador a la posición específica
            Debug.Log("Posición antes de la teletransportación: " + player.transform.position);
            player.transform.position = new Vector3(-5.7f, 10.7f, 4f);
            Debug.Log("Posición después de la teletransportación: " + player.transform.position);

            isPressed = true;
        }
        StartCoroutine(WaitAndTrigger());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            StartCoroutine(WaitToAvoidDoubleClicks());
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public IEnumerator WaitAndTrigger()
    {
        yield return new WaitForSeconds(5);
        button.transform.localPosition = new Vector3(0, 0.015f, 0);
        isPressed = false;
    }

    public IEnumerator WaitToAvoidDoubleClicks()
    {
        yield return new WaitForSeconds(2);
    }
}
