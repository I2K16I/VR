using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Ecke;
    [SerializeField]
    private GameObject Grade;
    [SerializeField]
    private GameObject TKreuzung;
    private int Platform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    public void setPlatform(int plat){
        Platform=plat;
        spawnPlatform();

    }

    private void spawnPlatform(){
        if(Platform==1){
            spawn(Ecke, 2, -1, 0);
        }
        else if(Platform==2){
            spawn(Grade, 2, -1, 0);
        }
        else if(Platform==3){
            spawn(TKreuzung, 2, -1, 0);
        }
    }

    private void spawn(GameObject GO, int positionx, int positiony, int rotationy){
    
        Instantiate(GO, new Vector3(positionx*3, 0, positiony*3), Quaternion.Euler(0, (rotationy-2)*90, 0));
    }

    void Update()
    {
        
    }
}
