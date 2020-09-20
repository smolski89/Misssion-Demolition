using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    //pola definiowane w panelu inspecyjnym Unity
    [Header("Definiowane w panelu inspekcyjnym")]
    public GameObject prefarbProjectile;
    public float velocityMult = 8f;

    [Header("Definowane dynamicznie")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public Rigidbody projectileRigidbody;
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        //print("Slingshot:onMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
    void OnMouseDown()
    {
        //Gracz nacisnął klawisz myszy podczas przesuwania jej wskaznika nad procą
        aimingMode = true;
        //Tworzenie pocisku
        projectile = Instantiate(prefarbProjectile) as GameObject;
        //Umieść go w miejscu oddawania strzału (launchPoint)
        projectile.transform.position = launchPos;
        //Użyj kinematyki
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;

    }
    void Update()
    {
        //jeśli proca nie działa w trybie aimingmode, nie uruchamiaj tego kodu
        if (!aimingMode) return;
        //pobierz bieżące położenie myszy w dwuwymiarowych wspołrzednych ekranu
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //Oblicz wektor różnicy między launchPos i mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Ogranicz wartość mouseDelta do promienia zderzacza kulistego dla obiektu Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //Przenieśćpocisk do nowego położenia
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            //Przycisk myszy został zwolniony
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }

    }
}
