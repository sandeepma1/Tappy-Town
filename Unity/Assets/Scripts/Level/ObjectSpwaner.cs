using UnityEngine;
using System.Collections;

public class ObjectSpwaner : MonoBehaviour {

  public Transform playerPos;
  int posAdder = 10;
  int blockPos = 20;
  int spwaner = 3;
  float ranValue = 0.0f;
  GameObject[] blocks;
  float timeT = 0;

  void Awake() {
    blocks = new GameObject[10];
    blocks[0] = GameObject.Find("easy0"); ;
  }

  void Start() {
    for (int i = 1; i < 10; i++) {
      blocks[i] = GameObject.Find("easy" + i);
      blocks[i].gameObject.SetActive(false);
    }
  }

  // Update is called once per frame
  void Update() {
    if (GameEventManager.GetState() == GameEventManager.E_STATES.e_game) {
      timeT += Time.deltaTime;
      if (timeT >= 2.93f) {
        timeT = 0;
        ranValue = Random.Range(0f, 3.0f);
        if (ranValue <= 1f) {
          if (spwaner <= 9 && spwaner >= 2) {
            spwaner = spwaner - 1;
          }
        }
        if (ranValue >= 1f) {
          if (spwaner <= 9 && spwaner >= 2) {
            spwaner = spwaner + 1;
          }
        }
        if (spwaner >= 9 || spwaner <= 2) {
          spwaner = 3;
        }
        print(spwaner);
        blocks[spwaner].SetActive(false);
        blocks[spwaner].transform.position = new Vector3(15, 0);
        blocks[spwaner].SetActive(true);
      }
    }
  }
}
