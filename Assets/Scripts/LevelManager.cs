using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject black;
    public GameObject pressCText;
    public GameObject player;

    public float transitionTime = 1;

    private bool iSDoorText = false;

    private GameObject checkpoint;
    private Vector2 startPlayerPosition;

    private void Awake()
    {
        startPlayerPosition = player.transform.position;
    }

    void OnEnable()
    {
        PlayerInterfere.DoorEnter += InfoDoorEnter;
        PlayerInterfere.DoorExit += DoorExit;
        Checkpoint.NewCeckpoint += NewCeckpoint;
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

    void NewCeckpoint(GameObject checkpoint)
    {
        if (this.checkpoint != null) this.checkpoint.SendMessage("ResetCheckpoint");
        this.checkpoint = checkpoint;
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

    public void MoveToCheckpoint()
    {
        if (this.checkpoint != null) player.transform.position = this.checkpoint.transform.position;
        else player.transform.position = startPlayerPosition;
    }

    IEnumerator LoadLevel()
    {
        black.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}