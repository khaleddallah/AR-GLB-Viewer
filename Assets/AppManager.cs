using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;


[Serializable]
public class glbModel{
    public Button button;
    public string url;
    public string localPath="";
}


public class AppManager : MonoBehaviour
{
    public glbModel[] items;

    public Slider progressBar;
    public TextMeshProUGUI noteText;
    public Color successDownloadColor = Color.green;
    
    GLBDownloader glbDownloader;
    GLBViewer glbViewer;

    Camera cam;



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        glbDownloader = GetComponent<GLBDownloader>();
        glbViewer = GetComponent<GLBViewer>();
        foreach(glbModel item in items){
            item.button.onClick.AddListener(() => {
                glbViewer.buttonPressed = true;
                StartCoroutine(HandleModelClick(item.url));
            });
        }

        progressBar.maxValue = 1+glbDownloader.progressBarOffset;

    }



    public IEnumerator HandleModelClick(string url){
        glbModel currentGlbModel = Array.Find(items, item => item.url == url);
        string localPathTmp = "";

        if(!IsFileExist(currentGlbModel)){
            string fileName = Path.GetFileName(url);
            Debug.Log("filename is " + fileName);

            localPathTmp= Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log("localPath is " + localPathTmp);

            Debug.Log("Downloading start");
            StartCoroutine(glbDownloader.DownloadGLBRoutine(url, localPathTmp));

            Debug.Log("Monitoring start");
            // progressBar.gameObject.SetActive(true);
            foreach(glbModel item in items){
                item.button.interactable = false;
            }
            yield return StartCoroutine(glbDownloader.MonitorProgressRoutine());

            Debug.Log("reset UI");
            progressBar.value = 0f;
            // progressBar.gameObject.SetActive(false);
            foreach(glbModel item in items){
                item.button.interactable = true;
            }

            if(glbDownloader.errorMsg.Length==0){
                currentGlbModel.localPath = localPathTmp;
                currentGlbModel.button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = successDownloadColor;
            } else {
                Debug.Log("Error");
                StartCoroutine(DisplayMsgError(glbDownloader.errorMsg));
                yield break;
            }
        }


        Debug.Log("Display Model");
        glbViewer.DisplayGLBModel(currentGlbModel.localPath);

    }

    IEnumerator DisplayMsgError(string msg){
        noteText.text = "Error: " + msg;
        yield return new WaitForSecondsRealtime(3);
        noteText.text = "";
    }

    bool IsFileExist(glbModel item){
        return item.localPath.Length>0;
    }


    void Update(){
        if(progressBar.IsActive()){
            progressBar.value = Mathf.Lerp(progressBar.value, glbDownloader.progress, 0.1f);
        }
    }



}
