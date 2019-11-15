using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private Vector3 lastHitNormal;

    public GameObject highlight;

    private GameObject quadH;

    void Start() {
        quadH = (GameObject) Instantiate(highlight, Vector3.zero, Quaternion.identity);
        quadH.SetActive(false);
        quadH.transform.parent = this.transform;
    }
    
    void Update() {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            HighlightSelectedFace(hit);
        } else {
            RemoveHighlight();
        }
    }

    private void HighlightSelectedFace(RaycastHit hit) {
        if(lastHitNormal != Vector3.zero) {
            RemoveHighlight();
        }

        Vector3 quadPos = hit.point + hit.normal/2.0f;
                
        quadPos.x = (float) Math.Round(quadPos.x, MidpointRounding.AwayFromZero);
        quadPos.y = (float) Math.Round(quadPos.y, MidpointRounding.AwayFromZero);
        quadPos.z = (float) Math.Round(quadPos.z, MidpointRounding.AwayFromZero);

        quadPos = quadPos - (hit.normal/2.25f);

        quadH.transform.position = quadPos;
        quadH.transform.rotation = Quaternion.LookRotation(-hit.normal);
        quadH.SetActive(true);
        lastHitNormal = hit.normal;
    }

    private void RemoveHighlight() {

        quadH.SetActive(false);

        lastHitNormal = Vector3.zero;
    }
}
