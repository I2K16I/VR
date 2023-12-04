using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapmanager : MonoBehaviour
{
    [SerializeField]
    private GameObject Ecke;
    [SerializeField]
    private GameObject Grade;
    [SerializeField]
    private GameObject TKreuzung;

     [SerializeField]
    private GameObject EckeMitTruhe;
    [SerializeField]
    private GameObject GradeMitTruhe;
    [SerializeField]
    private GameObject TKreuzungMitTruhe;
    [SerializeField]
    private int XFields=7;
    [SerializeField]
    private int YFields=7;
    [SerializeField]
    private GameObject MovingPlatformSpawner;
    [SerializeField]
    private int nrofchests=3;
    private int AnzahlEcken=15;            //19-4 vordeinierte;
    private int AnzahlGraden=13;           //13 weil keine vordiniert;
    private int AnzahlTs=6;                //18-12 vordinierte;
    private int[,] mapsave;    //Ecke=1i, Grade=2i, Kreuzung=3i, i=>rotation um i=>90 Grad
    private int lastpiece;
    private GameObject lastPieceObject;

    public GameObject LastPieceObject
    {
        get
        {
            return lastPieceObject;
        }
        set
        {
            lastPieceObject = value;
        }
    }
    private MovingPlatformSpawner movingspawner;
    private float scaleoflabyrinth;
    private int [] ChestsX;
    private int [] ChestsY;
    private int [] chestpieceatinterval;
    

    private void Awake()
    {
        scaleoflabyrinth=Ecke.transform.localScale.x;
        mapsave=new int[XFields+2, YFields+2];
        ChestsX=new int [nrofchests+1];
        ChestsY=new int [nrofchests+1];
        chestpieceatinterval=new int [nrofchests+1];
        fillmap();
        checkmap();
        chestPiece();
        spawnmap();
        setlastpiece();
        movingspawner=MovingPlatformSpawner.GetComponent<MovingPlatformSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {   

        //movingspawner.setPlatform(lastpiece);
    }
    private GameObject spawn(GameObject GO, int positionx, int positiony, int rotationy){
        return Instantiate(GO, new Vector3(positionx*scaleoflabyrinth, 0, positiony*scaleoflabyrinth), Quaternion.Euler(0, (rotationy-2)*90, 0));
    }
    private void checkmap(){
        for (int i=0; i<nrofchests; i++){
                        chestpieceatinterval[i]=mapsave[ChestsX[i], ChestsY[i]];
                        mapsave[ChestsX[i], ChestsY[i]]=0;
        }
    }
        
    

    private void setlastpiece(){
        if(AnzahlEcken>0){
            lastPieceObject=Ecke;
        }
        else if(AnzahlGraden>0){
            lastPieceObject=Grade;
        }
        else if(AnzahlTs>0){
            lastPieceObject=TKreuzung;
        }
        lastPieceObject = spawnPlatform();
    }
    
    private GameObject spawnPlatform(){
        if(lastpiece==1){
            return spawn(Ecke, -2, 2, 0);
        }
        else if(lastpiece==2){
            return spawn(Grade, -2, 2, 0);
        }
        
        return spawn(TKreuzung, -2, 2, 0);
    }

    private void chestPiece(){
        for (int i=0; i<=nrofchests; i++){
            ChestsX[i]=(Random.Range(0, XFields));
            ChestsY[i]=(Random.Range(0, YFields));
            if(i>=1){
                for(int j=0; j<i; j++){
                    if(ChestsX[i]==(ChestsX[j]+1)||ChestsX[i]==ChestsX[j]||ChestsX[i]==(ChestsX[j]+1)){
                        if(ChestsY[i]==(ChestsY[j]+1)||ChestsY[i]==ChestsY[j]||ChestsY[i]==(ChestsY[j]+1)){
                            
                            i-=1; 
                        } 
                    }
                    if(ChestsY[i]==(ChestsY[j]+1)||ChestsY[i]==ChestsY[j]||ChestsY[i]==(ChestsY[j]+1)){
                        if(ChestsX[i]==(ChestsX[j]+1)||ChestsX[i]==ChestsX[j]||ChestsX[i]==(ChestsX[j]+1)){
                            i-=1; 
                        } 
                    }
                }
            
            }
            
        }
    }

    private void fillmap(){
        //Ich fuelle das Array, damit ich spaeter die nicht vordefinierten Felder besser raussuchen kann
        for (int i=1; i<=XFields; i++){
            for(int j=1; j<=YFields; j++){
                mapsave[i, j]=0;
            }
         } 
        
        //Hier fuelle ich die Standartfaecher, kein Plan, wie ich das geschickter loese, vielleicht testen wir das auch komplett ohne vordefinierte Felder?
            //Ecktiles
         mapsave[1, 1]=10;
         mapsave[1, 7]=11;
         mapsave[7, 1]=13;
         mapsave[7, 7]=12;

         //Seitentiles
         mapsave[1, 3]=31;
         mapsave[1, 5]=30;
         mapsave[7, 3]=32;
         mapsave[7, 5]=31;
         mapsave[5, 7]=32;
         mapsave[3, 7]=32;
         mapsave[3, 1]=33;
         mapsave[5, 1]=33;

         //Mitteltiles
         mapsave[3, 3]=33;
         mapsave[3, 5]=30;
         mapsave[5, 5]=32;
         mapsave[5, 3]=31;

         //Hier fuelle ich das 2D-Array mit den Zahlen

         for (int i=1; i<=XFields; i++){
            for(int j=1; j<=YFields; j++){
                if(mapsave[i, j]==0){
                    int zusammenfassung;
                    do {        
                    int zwischenspeicher=Random.Range(1, 4);
                     zusammenfassung=checkifpossible(zwischenspeicher);     //guckt ob die Nummer noch vorhanden ist, erstellt Feldzahl
                    //Debug.Log("An dieser Stelle "+i+ " I "+j+ "ist Zusammenfassung" + zusammenfassung +" und zwischenspeicher :" + zwischenspeicher);
                    mapsave[i, j]=zusammenfassung;
                    
                    }
                    while(zusammenfassung==5);
                }
            }
         }
        /*for (int i=1; i<=XFields; i++){
            for(int j=1; j<=YFields; j++){
                Debug.Log("In Feld "+ i +"I"+ j + " ist die Zahl" + mapsave [i, j] + "!");
            }
        }*/

    }
    private void spawnmap(){
        for(int i=1; i<=XFields+1; i++){
            for(int j=1; j<=YFields+1; j++){
                if(mapsave[i, j]!=0){
                    if(mapsave[i, j]<20){
                    
                        spawn(Ecke, i, j, (mapsave[i, j]-10));
                    }
                     else if(mapsave[i, j]<30){
                        spawn(Grade, i, j, (mapsave[i, j]-20));
                    }
                    else if(mapsave[i, j]<40){
                        spawn(TKreuzung, i, j, (mapsave[i, j]-30));
                    }
                }
            }
        }
        for (int i=0; i<=nrofchests; i++){
            if(chestpieceatinterval[i]<20){
                    
                        spawn(EckeMitTruhe, ChestsX[i], ChestsY[i], (chestpieceatinterval[i]-10));
                    }
                     else if(chestpieceatinterval[i]<30){
                        spawn(GradeMitTruhe, ChestsX[i], ChestsY[i], (chestpieceatinterval[i]-20));
                    }
                    else if(chestpieceatinterval[i]<40){
                        spawn(TKreuzungMitTruhe, ChestsX[i], ChestsY[i], (chestpieceatinterval[i]-30));
                    }


        }

    }
    private int checkifpossible(int zwischenspeicher){
                    if(zwischenspeicher==1){
                        if(AnzahlEcken<=0){
                            return 5;
                        }
                        else{
                            AnzahlEcken-=1;
                            zwischenspeicher*=10;
                            zwischenspeicher+=Random.Range(0, 4);
                            return zwischenspeicher;
                        }
                    }
                    else if(zwischenspeicher==2){
                        if(AnzahlGraden<=0){
                            return 5;
                        }
                        else{
                            AnzahlGraden-=1;
                            zwischenspeicher*=10;
                            zwischenspeicher+=Random.Range(0, 4);
                            return zwischenspeicher;
                        }
                    }
                    else if(zwischenspeicher==3){
                        if(AnzahlTs<=0){
                            return 5;
                        }
                        else{
                            AnzahlTs-=1;
                            zwischenspeicher*=10;
                            zwischenspeicher+=Random.Range(0, 4);
                            return zwischenspeicher;
                        }
                    }
                    else{
                        return 5;
                    }
    }
    // Update is called once per frame
    
    void Update()
    {
        
    }
}
