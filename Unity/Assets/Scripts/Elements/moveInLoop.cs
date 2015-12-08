using UnityEngine;
using System.Collections;

public class moveInLoop : MonoBehaviour {
  public Vector3 moveDirection = new Vector3(2, 0, 0);
  public float moveInSeconds = 0.0f;
  public float moveLimit = 0.0f;

  // Update is called once per frame
  void LateUpdate() {
    transform.Translate(moveDirection * (Time.deltaTime * moveInSeconds));
    if (transform.localPosition.x <= moveLimit) {
      transform.localPosition = Vector3.zero;
    }
  }
}
