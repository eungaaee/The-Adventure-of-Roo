using UnityEngine;
using DG.Tweening;


public class endcutsceneCameraMove : MonoBehaviour
{
    [SerializeField] private Transform CameraMain;
    
    private Sequence CameraMove;

    // Update is called once per frame
    void Start()
    {
        new WaitForSeconds(2);
        CameraMove = DOTween.Sequence()
        .Append(CameraMain.DOMoveX(15.84f,28).SetEase(Ease.Linear))
        .Append(CameraMain.DOMoveY(-1.1f,2.5f).SetEase(Ease.Linear))
        .Append(CameraMain.DOMoveX(-26.42f,8).SetEase(Ease.Linear));
    }
}

