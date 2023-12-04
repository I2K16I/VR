using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartDetection : MonoBehaviour
{
    private int _counter = 2;
    private int _counterParts;
    public InputAction changePos;
    public InputAction enter;
    private List<Collider> row = new List<Collider>();
    private Vector3 directionVector = new Vector3(0, 0, 0);
    private int tileSize = 6;
    private GameObject activePiece;

    [SerializeField]
    private GameObject[] trigger;

    [SerializeField] private GameObject mapManager;
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("TEST");
        this.transform.parent = mapManager.transform;
        this.transform.position = new Vector3(0,0,(_counter*tileSize));
        activePiece = mapManager.GetComponent<Mapmanager>().LastPieceObject;
        activePiece.transform.parent = this.transform;
        Debug.Log(activePiece);
    }

    // Update is called once per frame
    void Update()
    {
        if (changePos.triggered)
        {
            //_counterParts = 0;
            foreach (var var in row)
            {
                Debug.Log(var.name);
            }
            
            _counter+=2;
            if (_counter <= tileSize)
            {
                directionVector = new Vector3(1, 0, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (_counter == 2)
                {
                    transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 2*tileSize);

                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+2*tileSize);

                }
                
            }
            if (_counter > tileSize && _counter <= 12)
            {
                directionVector = new Vector3(0, 0, -1);
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
                if (_counter == 8)
                {
                    
                    transform.position = new Vector3(2*tileSize, this.transform.position.y, this.transform.position.z);
   
                }
                else
                {
                    
                    this.transform.position = new Vector3(this.transform.position.x+2*tileSize,this.transform.position.y,this.transform.position.z);

                }
            }
            if (_counter > 12 && _counter <= 18)
            {                
                directionVector = new Vector3(-1, 0, 0);
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                if (_counter == 14)
                {
                    transform.position = new Vector3(transform.position.x, this.transform.position.y, 2*tileSize*3);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z-2*tileSize);

                }
                
            }
            if (_counter > 18 && _counter <= 24)
            {                
                directionVector = new Vector3(0, 0, 1);
                this.transform.rotation = Quaternion.Euler(0, 270, 0);
                if (_counter == 20)
                {
                    transform.position = new Vector3(2*tileSize*3, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x-2*tileSize,this.transform.position.y,this.transform.position.z);

                }
                
            }
           
            _counter = _counter % 24;
        }
        if (enter.triggered)
        {
            foreach (var collider in row)
            {
                
                collider.gameObject.transform.position += directionVector*tileSize;
               // mapManager.GetComponent<Mapmanager>().LastPieceObject.transform.position += directionVector*tileSize;
                Debug.Log("lastPiece: " + mapManager.GetComponent<Mapmanager>().LastPieceObject.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        
        if (other.GameObject().CompareTag("PartDetection"))
        {
            row.Add(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GameObject().CompareTag("PartDetection"))
        {
            row.Remove(other);
        }
    }

    void OnEnable()
    {
        changePos.Enable();
        enter.Enable();
    }

    void OnDisable()
    {
        changePos.Disable();
        enter.Disable();
    }
}
