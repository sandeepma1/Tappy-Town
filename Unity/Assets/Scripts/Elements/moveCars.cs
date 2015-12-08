using UnityEngine;
using System.Collections;

public class moveCars : MonoBehaviour {
  bool isMoving = false;
  Vector3 moveDirection;// = new Vector3(2, 0, 0);
  float moveInSeconds;

  // Use this for initialization
  void OnEnable() {
    moveInSeconds = Random.Range(1.0f, 3.0f);
    moveDirection = new Vector3(Random.Range(-2.0f, -7.0f), 0, 0);
    transform.localPosition = Vector3.zero;
    isMoving = true;
  }


  void LateUpdate() {
    if (GameEventManager.GetState() == GameEventManager.E_STATES.e_game && isMoving) {
      transform.Translate(moveDirection * (Time.deltaTime * moveInSeconds));
    }
  }

  void OnDisable() {
    isMoving = false;
  }
}
