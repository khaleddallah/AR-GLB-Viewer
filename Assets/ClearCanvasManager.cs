using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearCanvasManager : MonoBehaviour
{
    public GameObject item;
    public Button clearButton;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        clearButton.onClick.AddListener(() => {
            Destroy(item);
            Destroy(gameObject);
        });
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = cam.transform.forward;
    }
}
