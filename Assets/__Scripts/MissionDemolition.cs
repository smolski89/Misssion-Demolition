using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}


public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Definiowane w panelu inspekcyjnym")]
        public Text uitLevel; //treść etykiety UIText_Level
        public Text uitShots; // treść etykiety UIText_Shots
        public Text uitButton; //Nazwa prezycisku UIButton_View
        public Vector3 castlePos; // Miejsce, w którym nazlezy umieścić zamek
        public GameObject[] castles; // tablica zamków
    [Header("Definiowane dynamicznie")]
        public int level; // Bieżacy poziom gry
        public int levelMax; // Liczba poziomów
        public int shotsTaken;
        public GameObject castle; //Bieżący zamek
        public GameMode mode = GameMode.idle;
        public String showing = "Show Slingshot"; //tryb FollowCam
    void Start()
    {
        S = this; //Zdefiniuj singleton
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        //usunięcie poprzedniego zamku jeśli istnieje
        if(castle != null)
        {
            Destroy(castle);
        }
        //usunięcie niepotrzebnych przycisków, jesli istnieją
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject ptemp in gos){
            Destroy(ptemp);
        }
        //Utworzenie nowego zamku
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        //Przypisanie kamierze parametrów domyślnych
        SwitchView("Show Both");
        ProjectileLine.S.CLear();
        //Przypisanie obiektowi celu parametrów domyslnych
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;

    }
    void UpdateGUI()
    {
        //Wyświetlanie informacji w elementach GUI
        uitLevel.text = "poziom: " + (level + 1) + " z " + levelMax;
        uitShots.text = "Strzały: " + shotsTaken;
    }
    void Update()
    {
        UpdateGUI();
        //Sprawdz czy koniec poziomu
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            //Zmień tryb, aby nie sprawdzać zakonczenia poziomu
            mode = GameMode.levelEnd;
            //pomniejszenie widoku
            SwitchView("show Both");
            //Za 2 sekundy rozpocznie sie kolejny poziom
            Invoke("NextLevel", 2f);
        }

    }
    void nextLevel()
    {
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public void SwitchView( string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Pokaż zamek";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "pokaż zamek";
                break;
            case "Show both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "pokaż proce";
                break;
        }
                
    }
    public static void ShotFired()
    {
        S.shotsTaken++;
    }

}
