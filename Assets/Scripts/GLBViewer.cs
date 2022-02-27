using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;


public class GLBViewer : MonoBehaviour
{
    [HideInInspector] public bool buttonPressed;
    public GameObject clearButtonCanvas;
    public float canvasPositionOffset = 0.55f;
    public float objectPositionOffset = 1.55f;
    public float scaleObjectOffset = 0.1f;
    public float scaleCanvasOffset = 0.4f;

    private Camera cam;


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
                Vector3 localPos = Vector3.zero;
                foreach(BoxCollider c in hit.transform.gameObject.GetComponents<BoxCollider>()){
                    if(c.center.y-c.size.y/2 < localPos.y){
                        localPos.y = c.center.y-c.size.y/2;
                    }
                }
                localPos.y = localPos.y - canvasPositionOffset;

                GameObject x = GameObject.Instantiate(clearButtonCanvas);
                x.transform.SetParent(hit.transform.root);
                x.transform.localPosition = localPos;
                x.transform.localScale = Vector3.one*scaleCanvasOffset;

                x.GetComponent<ClearCanvasManager>().item = hit.transform.root.gameObject;
                x.transform.SetParent(hit.transform.root);
            }
        }
    }


    public void DisplayGLBModel(string filePath){
        GameObject displayedObject = Importer.LoadFromFile(filePath);


        foreach(MeshRenderer r in displayedObject.GetComponentsInChildren<MeshRenderer>()){
            BoxCollider c = displayedObject.AddComponent<BoxCollider>();
            c.size = r.bounds.size;
            c.center = r.bounds.center;
        }
        foreach(SkinnedMeshRenderer r in displayedObject.GetComponentsInChildren<SkinnedMeshRenderer>()){
            BoxCollider c = displayedObject.AddComponent<BoxCollider>();
            c.size = r.bounds.size;
            c.center = r.bounds.center;
        }
        displayedObject.transform.tag = "item";


        displayedObject.transform.position = cam.transform.position + cam.transform.forward * objectPositionOffset;
        displayedObject.transform.forward = cam.transform.forward;
        displayedObject.transform.localScale = Vector3.one * scaleObjectOffset;
    }


}
