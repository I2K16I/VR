using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int _amountOfCorners = 18;
    [SerializeField]
    private int _amountOfStraights = 12;
    [SerializeField]
    private int _amountOfTs = 17;
    [SerializeField]
    private GameObject _corner;
    [SerializeField]
    private GameObject _straight;
    [SerializeField]
    private GameObject _tCrossing;
    [SerializeField]
    private int _amountOfCornersChests = 1;
    [SerializeField]
    private int _amountOfStraightChests = 1;
    [SerializeField]
    private int _amountOfTChests = 1;
    [SerializeField]
    private GameObject _cornerChest;
    [SerializeField]
    private GameObject _straightChest;
    [SerializeField]
    private GameObject _tCrossingChest;

    private List<GameObject> tileCollection = new List<GameObject>();

    [SerializeField] private int _dimension = 7;

    private float _scale;


    private int _counterCorner;
    private int _counterStraight;
    private int _counterTCrossing;
    private int _counterCornerChest;
    private int _counterStraightChest;
    private int _counterTCrossingChest;
    
    [SerializeField]
    private GameObject _firstTrigger;

    private GameObject _lastTile;

    public GameObject LastTile
    {
        get
        {
            return _lastTile;
        }
    }

    private void Awake()
    {
        tileCollection.Add(_corner);
        tileCollection.Add(_cornerChest);
        tileCollection.Add(_straight);
        tileCollection.Add(_straightChest);
        tileCollection.Add(_tCrossing);
        tileCollection.Add(_tCrossingChest);
        //Debug.Log("scale 1: " + _corner.transform.localScale.x);
        _scale = _corner.transform.localScale.x;
        //Debug.Log("scale 2: " + _corner.transform.localScale.x);
        for (int i = 0; i < _dimension; i++)
        {
            for (int j = 0; j < _dimension; j++)
            {
                // Debug.Log("j und i " + j + ", " + i);
                
                GameObject gameObjectTemp = chooseGameObject();
                //Debug.Log("chooseGameObject " + gameObjectIndex);
                if (checkAvailablity(gameObjectTemp))
                {
                    createObject(gameObjectTemp, i*_scale + (_scale/2), j*_scale + (_scale/2), Random.Range(0,4)*90);
                }
                else
                {
                    j--;
                }
            }
        }
        _lastTile = chooseGameObject();
        _lastTile = createObject(_lastTile, _firstTrigger.transform.position.x, _firstTrigger.transform.position.z, Random.Range(0,4)*90);
    }

    void Start()
    {
 
    }
    
    private GameObject createObject(GameObject obj, float positionx, float positionz, int rotationy){
        //Debug.Log("Objekt: " + obj.name + "Position: " + positionx + ", " + positiony);
        return Instantiate(obj, new Vector3(positionx, 0, positionz), Quaternion.Euler(0, rotationy, 0));
    }

    private bool checkAvailablity(GameObject obj)
    {
        bool isAvailable = false;
        switch (obj.name)
        {
            case "Ecke" :
            case "Ecke Texturiert":
                if (_counterCorner < _amountOfCorners)
                {
                    _counterCorner++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_corner);
                }
                break;
            
            case "Gerade" :
            case "Gerade Texturiert":
                if (_counterStraight < _amountOfStraights)
                {
                    _counterStraight++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_straight);
                }
                break;
            
            case "T-Kreuzung" :
            case "T-Kreuzung Texturiert":
                if (_counterTCrossing < _amountOfTs)
                {
                    _counterTCrossing++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_tCrossing);
                }
                break;
            case "Ecke Texturiert Chest" :
                if (_counterCornerChest < _amountOfCornersChests)
                {
                    _counterCornerChest++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_cornerChest);
                }
                break;
            case "Gerade Texturiert Chest" :
                if (_counterStraightChest < _amountOfStraightChests)
                {
                    _counterStraightChest++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_straightChest);
                }
                break;
            case "T-Kreuzung Texturiert Chest" :
                if (_counterTCrossingChest < _amountOfTChests)
                {
                    _counterTCrossingChest++;
                    isAvailable = true;
                }
                else
                {
                    tileCollection.Remove(_tCrossingChest);
                }
                break;
        }
        //Debug.Log(isAvailable);
        return isAvailable;
    }

    private GameObject chooseGameObject()
    {
        Debug.Log(tileCollection.Count);
        if (tileCollection.Count != 0)
        {
            return tileCollection[Random.Range(0, tileCollection.Count)];
        }
        return tileCollection[0];
    }
    
}
