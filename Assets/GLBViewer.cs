using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Siccity.GLTFUtility;
using UnityEngine.Networking;


public class GLBViewer : MonoBehaviour
{

    public Transform objectTransform;
    public GameObject displayedObject;

    public void DisplayGLBModel(string filePath){
        displayedObject = Importer.LoadFromFile(filePath);
        
        foreach(Transform tr in displayedObject.GetComponentsInChildren<Transform>()){
            tr.gameObject.AddComponent<BoxCollider>();
            tr.tag = "item";
        }

        displayedObject.transform.position = objectTransform.position;
        displayedObject.transform.rotation = objectTransform.rotation;
        displayedObject.transform.localScale = objectTransform.localScale;
    }


}
