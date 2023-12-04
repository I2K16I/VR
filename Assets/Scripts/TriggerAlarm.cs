using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerAlarm : MonoBehaviour
{
    private GameObject _partDetector;
    private GameObject _newActiveTile;
    private int _counter = 0;
    private void Start()
    {
        _partDetector = GameObject.Find("PartDetection");
    }

    public GameObject getNewActiveTile()
    {
        return _newActiveTile;
    }
    private void OnTriggerStay(Collider other)
    {
        _partDetector.GetComponent<TileMover>().setCurrentTrigger(this.GameObject());
        if (other.GameObject().CompareTag("PartDetection"))
        {
            _newActiveTile = other.GameObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Test dies ist nur ein Test");
        /*if (other.GameObject().CompareTag("PartDetection"))
        {
            Debug.Log(other.transform.position);
            //other.transform.position = transform.position
            //_partDetector.GetComponent<TileMover>().reactToTrigger(gameObject);
            //_partDetector.GetComponent<TileMover>().changeActiveTile(_newActiveTile);
            float distance = Vector3.Distance(other.transform.position,
                _partDetector.GetComponent<TileMover>().getActiveTile().transform.position);
            if (distance > 0.5f || _counter == 0) 
            {
                Debug.Log("Neues Aktives Tile");
                _counter++;
                 _newActiveTile = other.gameObject;
                 _partDetector.GetComponent<TileMover>().reactToTrigger(gameObject);
                 _partDetector.GetComponent<TileMover>().changeActiveTile(_newActiveTile);
            }
        }*/
    }
}
