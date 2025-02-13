using System.Runtime.ExceptionServices;
using System.Transactions;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.ComponentModel.Design;
using System.Net;
using System.Diagnostics;
using UnityEngine;
using RDG;

public class MaterialMenager : MonoBehaviour
{
    private hub Hub;
    private Movingcleaninganim movingcleaninganim;
    private Movingcleaninganim movingpainting;
    public GameObject movewhenanimoff;
    float holdTime = 2f;
    float heldDuration = 0f;
    //
    float paintholdTime = 2f;
    float paintheldDuration = 0f;
    GameObject tenmalujeteraz;
    //
    public Material Material1;
    private scriptanimationgombka Scriptanimationgombka;
    private scriptanimationszczota Scriptanimationszczota;

    void Start()
    {
        GameObject obj = GameObject.Find("Menager");
        Hub = obj.GetComponent<hub>();
        GameObject movecleaninganim = GameObject.Find("Szczotagombkaparent");
        movingcleaninganim = movecleaninganim.GetComponent<Movingcleaninganim>();
        GameObject movepaintinganim = GameObject.Find("Farba i wiadro");
        movingpainting = movepaintinganim.GetComponent<Movingcleaninganim>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)&&hit.collider.gameObject.tag == "dirtywall"&& hit.collider.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                if (hit.collider.gameObject.tag == "dirtywall")//this script after raycasting checks if wall hited is dirty if it is it transports animation to wall and plays animation
                {
                    bool czyszczotka = Hub.szczotkajestwrece;
                    
                        if(heldDuration==0&&czyszczotka)
                    {
                        Vibration.Vibrate(100,250);
                        movingcleaninganim.SetTargetObject(hit.collider.gameObject);
                        Hub.animszczotka = true;
                        Hub.animgombka = true;

                    }
                    
                    heldDuration += Time.deltaTime;
                    
                    if (heldDuration >= holdTime&&czyszczotka)//after some time it cleans wall by turning its material
                    {
                        hit.collider.gameObject.tag = "wall";
                        Renderer renderer = hit.collider.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            Vibration.Vibrate(100,150);
                            renderer.material = Material1;
                        }
                    }
                }
                else
                {
                    heldDuration = 0f;
                        Hub.animszczotka = false;
                        Hub.animgombka = false;
                        movingcleaninganim.SetTargetObject(movewhenanimoff);
                }
            }//paitning of a wall
            else if (Physics.Raycast(ray, out hit)&&(hit.collider.gameObject.tag == "wall"||hit.collider.gameObject.tag == "floor")&& hit.collider.gameObject.GetComponent<MeshRenderer>().enabled&&Hub.malowac)
            {
                if (hit.collider.gameObject.tag == "wall")//same here checks if wall hited is wall and colours itselft after some time and playing animation
                {
                    if(paintheldDuration==0&&Hub.czykolorzostalwybrany&&!Hub.animszczotka&&!Hub.animgombka)
                    {
                        tenmalujeteraz = hit.transform.gameObject;
                        Hub.animmalowanie = true; //
                        movingpainting.SetTargetObject(hit.collider.gameObject);
                    }
                    else if (paintheldDuration >= paintholdTime&&Hub.czykolorzostalwybrany&&hit.transform.gameObject==tenmalujeteraz)
                    {
                        Renderer renderer = hit.collider.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = Hub.paintcolor;
                            Vibration.Vibrate(100,200);
                            paintheldDuration = 0f;
                            Hub.animmalowanie = false; 
                            movingpainting.SetTargetObject(movewhenanimoff);
                            
                        }
                    }
                    else if(!(hit.transform.gameObject==tenmalujeteraz))//this was for fixing a bug that after some time you could paint all walls at once
                    {
                    paintheldDuration = 0f;
                    Hub.animmalowanie = false; 
                    movingpainting.SetTargetObject(movewhenanimoff);
                    }
                    if(paintheldDuration>=0&&Hub.czykolorzostalwybrany&&hit.transform.gameObject==tenmalujeteraz)
                    {
                    paintheldDuration += Time.deltaTime;
                    Vibration.Vibrate(20,100);
                    }
                }
                else if(hit.collider.gameObject.tag == "floor")//there is paiting of a floor it doesnt have animation yet
                {
                    if(paintheldDuration==0&&Hub.czykolorzostalwybrany&&!Hub.animszczotka&&!Hub.animgombka)
                    {
                        tenmalujeteraz = hit.transform.gameObject;
                        
                        
                    }
                    else if (paintheldDuration >= paintholdTime&&Hub.czykolorzostalwybrany&&hit.transform.gameObject==tenmalujeteraz)
                    {
                        Renderer renderer = hit.collider.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = Hub.paintcolor;
                            Vibration.Vibrate(100,200);
                            paintheldDuration = 0f;
                            movingpainting.SetTargetObject(movewhenanimoff);
                            
                        }
                    }
                    else if(!(hit.transform.gameObject==tenmalujeteraz))
                    {
                    paintheldDuration = 0f;
                    movingpainting.SetTargetObject(movewhenanimoff);
                    }
                    if(paintheldDuration>=0&&Hub.czykolorzostalwybrany&&hit.transform.gameObject==tenmalujeteraz)
                    {
                    paintheldDuration += Time.deltaTime;
                    Vibration.Vibrate(20,100);
                    }
                }
                else
                {
                    paintheldDuration = 0f;
                    Hub.animmalowanie = false; 
                    movingpainting.SetTargetObject(movewhenanimoff);
                }
            }
            else
            {
            movingpainting.SetTargetObject(movewhenanimoff);
            movingcleaninganim.SetTargetObject(movewhenanimoff);
            Hub.animmalowanie = false; 
            Hub.animszczotka = false;
            Hub.animgombka = false;
            heldDuration = 0f;
            paintheldDuration=0f;
            }
        }
        else
        {
            movingpainting.SetTargetObject(movewhenanimoff);
            movingcleaninganim.SetTargetObject(movewhenanimoff);
            Hub.animmalowanie = false; 
            Hub.animszczotka = false;
            Hub.animgombka = false;
            heldDuration = 0f;
            paintheldDuration=0f;
        }
    }
}

