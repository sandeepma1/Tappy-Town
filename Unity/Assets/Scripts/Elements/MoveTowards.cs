using UnityEngine;
using System.Collections;

public class MoveTowards : MonoBehaviour {
  public float time = 1f;
  public Vector3 moveDirection = new Vector3(0, 0, 2);
  public float maxMove = 0;
  Vector3 iniPos;
  public Light l1, l2;
  public GameObject mover;
  bool isMoving = false;
  void Awake() {
    iniPos = mover.transform.localPosition;
  }

  void onEnable() {
    print("moving cars enabled");
  }

  void OnTriggerEnter(Collider other) {
    if (other.name == "man") {
      isMoving = true;
      StartCoroutine("Blink");
    }
  }

  void Update() {
    if (isMoving) {
      mover.transform.Translate(moveDirection * (Time.deltaTime * time));
      if (mover.transform.position.z <= maxMove) {
        StartCoroutine("moveToIniPosition");
        isMoving = false;
      }
    }
  }

  IEnumerator moveToIniPosition() {
    yield return new WaitForSeconds(1f);
    if (GameEventManager.GetState() == GameEventManager.E_STATES.e_game) {
      mover.transform.localPosition = iniPos;
      isMoving = false;
    }

  }
  IEnumerator Blink() {
    l1.GetComponent<Light>().intensity = 0;
    l2.GetComponent<Light>().intensity = 0;
    yield return new WaitForSeconds(0.2f);
    l1.GetComponent<Light>().intensity = 8;
    l2.GetComponent<Light>().intensity = 8;
    yield return new WaitForSeconds(0.2f);
    StartCoroutine("Blink");
  }
}
