using UnityEngine;
using System;
using System.IO;

public class ScreenCaptureImages : MonoBehaviour {
    public string m_Path = "Screenshots/";
    private string m_FilePath;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            // ���丮 ����
            if (!Directory.Exists(m_Path)) {
                Directory.CreateDirectory(m_Path);
            }

            // ���� �ð��� �̿��Ͽ� ������ ���ϸ� ����
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            m_FilePath = m_Path + "Screenshot_" + timestamp + ".png";

            // ��ũ���� ĸó �� ����
            ScreenCapture.CaptureScreenshot(m_FilePath);
            Debug.Log("��ũ���� �����: " + m_FilePath);
        }
    }
}
