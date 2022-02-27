using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;


public class GLBViewer : MonoBehaviour
{
    [HideInInspector] public bool buttonPressed;
    public Transform objectTransform;
    public GameObject clearButtonCanvas;


    Camera cam;


    void Start(){
        buttonPressed = false;
        cam = Camera.main;
    }

    void Update(){
            if(Input.touchCount > 0){
                DisplayClearButtonCanvas(Input.GetTouch(0).position);
            }

            if(Input.GetMouseButtonDown(0)){
                DisplayClearButtonCanvas(Input.mousePosition);
            }

    }


    void DisplayClearButtonCanvas(Vector3 screenPoint){
        Ray ray = cam.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if( hit.transform.tag == "item" &&
                !hit.transform.gameObject.GetComponentInChildren<ClearCanvasManager>())
            {
                Debug.Log("hit item");
                Vector3 pos = hit.transform.root.position + new Vector3(0, -0.5f, 0);
                GameObject x = GameObject.Instantiate(clearButtonCanvas, pos, Quaternion.identity);
                x.GetComponent<ClearCanvasManager>().item = hit.transform.root.gameObject;
                x.transform.SetParent(hit.transform.root);
            }
        }
    }


    public void DisplayGLBModel(string filePath){
        GameObject displayedObject = Importer.LoadFromFile(filePath);
        
        foreach(Transform tr in displayedObject.GetComponentsInChildren<Transform>()){
            tr.gameObject.AddComponent<BoxCollider>();
            tr.tag = "item";
        }

        displayedObject.transform.position = cam.transform.position + cam.transform.forward * 10;
    }












}
