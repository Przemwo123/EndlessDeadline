using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject black;
    public GameObject pressCText;
    public GameObject player;

    public float transitionTime = 1;

    private bool iSDoorText = false;

    void OnEnable()
    {
        PlayerInterfere.DoorEnter += InfoDoorEnter;
        PlayerInterfere.DoorExit += DoorExit;
    }


    void OnDisable()
    {
        PlayerInterfere.DoorEnter -= InfoDoorEnter;
        PlayerInterfere.DoorExit -= DoorExit;
    }

    void InfoDoorEnter()
    {
        if (!iSDoorText)
        {
            iSDoorText = true;
            pressCText.GetComponent<Animator>().SetTrigger("Start");
        }
    }

    void DoorExit()
    {
        if (iSDoorText)
        {
            iSDoorText = false;
            pressCText.GetComponent<Animator>().SetTrigger("End");
        }
    }

    void Update()
    {
        if (iSDoorText && Input.GetKeyDown(KeyCode.C))
        {
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<PlayerCombatController>().enabled = false;
            player.GetComponent<PlayerInterfere>().enabled = false;
            PlayerInterfere.DoorExit -= DoorExit;
            DoorExit();
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        black.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}