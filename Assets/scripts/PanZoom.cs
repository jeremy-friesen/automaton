using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PanZoom : MonoBehaviour{
  Vector3 touchStart;
  public float zoomOutMin = 100;
  public float zoomOutMax = 300;
  public float moveMax = 50;

  private bool panTouch;
  private Vector3 startPos;

  private float startZoom;

  public GameObject codeWindowTop;
  private float codeWindowHeight;

  private Camera mainCamera;

  private bool twoTouches;

  private bool settingsUp;
  public bool SettingsUp{
    get{
      return settingsUp;
    }
    set{
      settingsUp = value;
    }
  }

  private bool panEnabled;
  public bool Enabled{
    get{
      return panEnabled;
    }
    set{
      panEnabled = value;
      if(!panEnabled){
        if(mainCamera == null)
          Start();
        mainCamera.transform.position = startPos;
        mainCamera.orthographicSize = startZoom;
      }
    }
  }

  [SerializeField]
  public bool startSettingsUp = false;

  void Start(){
    twoTouches = false;
    mainCamera = gameObject.GetComponent<Camera>();
    panTouch = false;
    startPos = Camera.main.transform.position;
    startZoom = Camera.main.orthographicSize;
    codeWindowHeight = mainCamera.WorldToScreenPoint(codeWindowTop.transform.position)[1];
    panEnabled = SaveSystem.Load().TouchPanZoom;
    settingsUp = startSettingsUp;
  }

  // Update is called once per frame
  void Update(){
    if(!settingsUp && panEnabled){
      /*if(Input.touchCount == 2 && !twoTouches){

      } else {

      }*/
      if (Input.GetMouseButtonDown(0) && Input.mousePosition[1] > codeWindowHeight){
        touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        panTouch = true;
      }
      if (Input.touchCount == 2 && panTouch){
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        zoom(difference * 0.25f);
        if(Camera.main.orthographicSize > zoomOutMin && Camera.main.orthographicSize < zoomOutMax)
          gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.WorldToScreenPoint(Vector3.Lerp(touchZero.position,touchOne.position,0.5f)), difference/10);
        keepInBounds();

        /*Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touchZero.position);
        Vector3 newPos = Camera.main.transform.position + direction;
        
        if(newPos.x > startPos.x + moveMax){
          newPos.x = startPos.x + moveMax;
        }
        if(newPos.x < startPos.x - moveMax){
          newPos.x = startPos.x - moveMax;
        }
        if(newPos.y > startPos.y + moveMax){
          newPos.y = startPos.y + moveMax;
        }
        if(newPos.y < startPos.y - moveMax){
          newPos.y = startPos.y - moveMax;
        }
        Camera.main.transform.position = newPos;*/

        twoTouches = true;
      }else if (Input.GetMouseButton(0) && panTouch){
        if(twoTouches){
          touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = Camera.main.transform.position + direction;
        
        if(newPos.x > startPos.x + moveMax){
          newPos.x = startPos.x + moveMax;
        }
        if(newPos.x < startPos.x - moveMax){
          newPos.x = startPos.x - moveMax;
        }
        if(newPos.y > startPos.y + moveMax){
          newPos.y = startPos.y + moveMax;
        }
        if(newPos.y < startPos.y - moveMax){
          newPos.y = startPos.y - moveMax;
        }
        Camera.main.transform.position = newPos;
        
        twoTouches = false;
      } else{
        panTouch = false;
        twoTouches = false;
      }
      zoom(Input.GetAxis("Mouse ScrollWheel") * 30f);

      if(Input.GetAxis("Mouse ScrollWheel") != 0){
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1f);
        keepInBounds();
      }
    }
  }

  void zoom(float increment){
    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
  }

  void keepInBounds(){
    Vector3 newPos = Camera.main.transform.position;
    if(Camera.main.transform.position.x > startPos.x + moveMax){
      newPos.x = startPos.x + moveMax;
    }
    if(Camera.main.transform.position.x < startPos.x - moveMax){
      newPos.x = startPos.x - moveMax;
    }
    if(newPos.y > startPos.y + moveMax){
      newPos.y = startPos.y + moveMax;
    }
    if(newPos.y < startPos.y - moveMax){
      newPos.y = startPos.y - moveMax;
    }
    Camera.main.transform.position = newPos;
  }
}