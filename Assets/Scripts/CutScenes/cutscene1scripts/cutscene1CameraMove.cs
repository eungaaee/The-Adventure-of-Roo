using System.Collections;
using UnityEngine;
using DG.Tweening;


public class cutscene1CameraMove : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(CameraMove());
    }

    private IEnumerator CameraMove(){
        yield return new WaitForSeconds(9);
        yield return MainCamera.DOMoveX(61.63f,47).SetEase(Ease.Linear).OnComplete(() => MainCamera.DOMoveY(-3.4f,2).SetEase(Ease.Linear));
    }
}
