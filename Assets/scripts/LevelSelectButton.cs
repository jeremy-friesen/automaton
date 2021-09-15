using UnityEngine;
using UnityEngine.SceneManagement;
using System;
 
public class LevelSelectButton : MonoBehaviour{
  [SerializeField]
  private String sceneToLoad = null;

  public void NextScene(){
    SceneManager.LoadScene(sceneToLoad);
  }
}