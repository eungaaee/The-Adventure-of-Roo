using System.Collections;
using UnityEngine;
using System;
using System.IO;

public class ScreenCaptureImages : MonoBehaviour {
    public string m_Path = "Screenshots/";
    private string m_FilePath;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            // 디렉토리 생성
            if (!Directory.Exists(m_Path)) {
                Directory.CreateDirectory(m_Path);
            }

            // 현재 시간을 이용하여 고유한 파일명 생성
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            m_FilePath = m_Path + "Screenshot_" + timestamp + ".png";

            // 스크린샷 캡처 및 저장
            ScreenCapture.CaptureScreenshot(m_FilePath);
            Debug.Log("스크린샷 저장됨: " + m_FilePath);
        }
    }
}
