using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Start : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private DialogueController Dialogue;
    private SpriteRenderer PlayerS;
    void Start()
    {
        PlayerS = Player.GetComponent<SpriteRenderer>();
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene() {
        yield return new WaitForFixedUpdate();
        Player.SwitchControllable(false);
        Letterbox.LetterboxOn(100, 1.5f);
        StartCoroutine(Player.CutSceneMove(-83f, 3f));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
    }
}
