using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMover : MonoBehaviour
{

    [SerializeField] private InputAction move;

    [SerializeField] private InputAction moveBack;

    [SerializeField] private InputAction enter ;
    
    [SerializeField] private GameObject[] _triggers;

    [SerializeField] private int tileSize;

    [SerializeField]
    private Renderer _indicator;

    private GameObject activeTile;

    private int _currentTriggerIndex;

    private GameObject _activeTigger;
    
    private List<Collider> row = new List<Collider>();

    private Vector3 directionVector = new Vector3(0, 0, 0);

    private List<Tiles> tileList = new List<Tiles>();

    private bool isMoving;

    [SerializeField] private float tileMoveDuration = 2f;

    public struct Tiles {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public Transform Tile;
        public Tiles(Vector3 start, Vector3 end, GameObject tile){
            StartPosition = start;
            EndPosition = end;
            this.Tile = tile.transform;
        }
    } 

    [SerializeField]
    private GameObject mapGenerator;

    void Start()
    {
        activeTile = mapGenerator.GetComponent<MapGenerator>().LastTile;
        this.transform.position = _triggers[0].transform.position;
        activeTile.GetComponent<BoxCollider>().enabled = false;
    }
    
    private void toggleIsMoving()
    {
        Debug.Log("Toggle from: " + isMoving);
        isMoving =! isMoving;
        if (isMoving)
        {
            _indicator.material.SetColor("_ArrowColor", Color.red);
        }
        else
        {
            _indicator.material.SetColor("_ArrowColor", Color.green);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            return;
        }
        if (enter.triggered)
        {
            toggleIsMoving();
            tileList = new List<Tiles>();
            //row.Remove(activeTile.GetComponent<BoxCollider>());
            Vector3 activeTileStartPos = activeTile.transform.position;
            float activeX = activeTileStartPos.x + directionVector.x * (tileSize * 1.95f);
            float activeY = activeTileStartPos.y + directionVector.y * (tileSize * 1.95f);
            float activeZ = activeTileStartPos.z + directionVector.z * (tileSize * 1.95f);
            tileList.Add(new Tiles(activeTileStartPos, new Vector3(activeX, activeY, activeZ), activeTile));
            foreach (var collidingObject in row)
            {
                //collider.gameObject.transform.position += directionVector*tileSize;
                GameObject temp = collidingObject.gameObject;
                Vector3 startPosition = temp.transform.position;
                float x = startPosition.x + directionVector.x*tileSize;
                float y = startPosition.y + directionVector.y*tileSize;
                float z = startPosition.z + directionVector.z*tileSize;
                Vector3 endPosition = new Vector3(x,y,z);
                Tiles tile = new Tiles(startPosition, endPosition, temp); 
                tileList.Add(tile);
            }
            StartCoroutine(Lerp());
        }
        if (move.triggered)
        {
            _currentTriggerIndex++;
            _currentTriggerIndex %= 12;
            changePos();
            changeRot();
        }

        if (moveBack.triggered)
        {
            _currentTriggerIndex--;
            if (_currentTriggerIndex < 0)
            {
                _currentTriggerIndex = 11;
            }
            changePos();
            changeRot();
        }
        
    }
    IEnumerator Lerp(){
        float timeMoving = 0f; 
        while (timeMoving < tileMoveDuration){
        foreach (var tileElement in tileList){
            tileElement.Tile.position = Vector3.Lerp(tileElement.StartPosition, tileElement.EndPosition, timeMoving);
            timeMoving += Time.deltaTime;
            yield return null;
        }}
        foreach (var tileElement in tileList){
            tileElement.Tile.position = tileElement.EndPosition;
        }
        toggleIsMoving();
        changeActiveTile(_activeTigger.GetComponent<TriggerAlarm>().getNewActiveTile());
        reactToTrigger(_activeTigger);
        StopCoroutine("Lerp");
    } 

    private void changeRot()
    {
        float temp = (float)_currentTriggerIndex / 4;
        switch (temp)
        {
            case  < 0.75f:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                directionVector = new Vector3(1, 0, 0);
                break;
            case < 1.5f :
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
                directionVector = new Vector3(0, 0, -1);
                break;
            case < 2.25f :
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                directionVector = new Vector3(-1, 0, 0);
                break;
            case < 3 :
                this.transform.rotation = Quaternion.Euler(0, 270, 0);
                directionVector = new Vector3(0, 0, 1);
                break;
        }
    }

    private void changePos()
    {
        Debug.Log(activeTile.name);
        Vector3 currentTriggerPosition = _triggers[_currentTriggerIndex].transform.position;
        this.transform.position = currentTriggerPosition;
        activeTile.transform.position = currentTriggerPosition;
    }

    public void setCurrentTrigger(GameObject alarmedTrigger)
    {
        _activeTigger = alarmedTrigger;
    }
    public void reactToTrigger(GameObject alarmedTrigger)
    {
        _currentTriggerIndex = findIndex(alarmedTrigger);
        changePos();
        changeRot();
    }

    public void changeActiveTile(GameObject newActiveTile)
    {
        //activeTile.transform.position += directionVector * (tileSize * 1.5f);
        GameObject activeBuffer = activeTile;
        //activeTile.GetComponent<BoxCollider>().enabled = true;
        activeTile = newActiveTile;
        activeTile.GetComponent<BoxCollider>().enabled = false;
        activeBuffer.GetComponent<BoxCollider>().enabled = true;
    }

    public GameObject getActiveTile()
    {
        return activeTile;
    }

    private int findIndex(GameObject go)
    {
        for (int i = 0; i < _triggers.Length; i++)
        {
            if (_triggers[i].name.Equals(go.name))
            {
                return i;
            }
        }

        return -1;
    }

    private void OnTriggerEnter(Collider other)
    {
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
        move.Enable();
        enter.Enable();
        moveBack.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        enter.Disable();
        moveBack.Disable();
    }

}
