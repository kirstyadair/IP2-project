using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour
{
    GameData gameData;
    HordeScript horde;
    Vector3 cameraInitialPosition;
    Vector3 centerScreen;
    Camera cam;
    public int cameraStillness;
    public float zoomDuration;
    public float cameraShock = 0;
    public PostProcessingBehaviour postProcessing;
    public Animator animator;

    void Awake()
    {
        cam = GetComponent<Camera>();
        animator = GetComponent<Animator>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += OnStateChange;
        horde = GameObject.Find("Horde").GetComponent<HordeScript>();
        cameraInitialPosition = this.transform.position;
        centerScreen = gameData.currentMap.centerPoint.position;//cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 10));

        PlayerScript.OnPlayerDeath += OnPlayerDeath;
    }
    
    void OnPlayerDeath(PlayerScript player)
    {
        StartCoroutine(DelayedPlayerDeath());
    }

    IEnumerator DelayedPlayerDeath()
    {
        yield return new WaitForSeconds(0.5f);
        animator.Play("camera shock");
    }

    public void OnStateChange(GameState oldState, GameState newState)
    {
        /*
        float prepCameraZoom = gameData.currentMap.cameraZoomPrepMode;
        float playCameraZoom = gameData.currentMap.cameraZoomPlayMode;

        if (newState == GameState.PREP) StartCoroutine(LerpCameraSize(prepCameraZoom));
        if (newState == GameState.PLAY) StartCoroutine(LerpCameraSize(playCameraZoom));
        */
    }

    IEnumerator LerpCameraSize(float to)
    {
        float t = 0;
        float startingSize = cam.orthographicSize;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;

            cam.orthographicSize = Mathf.Lerp(LerpSmoothStart(startingSize, to, t / zoomDuration), LerpSmoothEnd(startingSize, to, t / zoomDuration), t/zoomDuration);

            if (t >= zoomDuration) cam.orthographicSize = to;
            yield return null;
        }
    }

    float LerpSmoothStart(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t * t);
    }

    float LerpSmoothEnd(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 1 - (1 - t) * (1 - t));
    }

    void Update()
    {
        Vector3 distanceFromCenterOfScreen = horde.crosshair.transform.position - centerScreen;
        distanceFromCenterOfScreen.z = 0;
        this.transform.position = cameraInitialPosition + distanceFromCenterOfScreen/cameraStillness;

        ChromaticAberrationModel.Settings chromaticAbberation = postProcessing.profile.chromaticAberration.settings;
        chromaticAbberation.intensity = cameraShock;
        postProcessing.profile.chromaticAberration.settings = chromaticAbberation;
    }
}
