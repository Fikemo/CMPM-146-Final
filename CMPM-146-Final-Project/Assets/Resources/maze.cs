using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze : MonoBehaviour
{
    public GameObject enemy;
    public GameObject wall;
    public GameObject floor;
    public GameObject key;
    public GameObject computer;
	public GameObject exit;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset t1 = (TextAsset)Resources.Load("level", typeof(TextAsset));
        string s = t1.text;

        int i, columnCounter = 0, columns = 0;
        float rows = 0f, columnTracking = 0f;
        
        for (i = 0; i < s.Length; i++) {
            if (s[i] == '\n') {
                break;
            }
            columns++;
        }

        s = s.Replace("\n","");

        for (i = 0; i < s.Length; i++) {
            if (columnCounter == columns) {
                rows = rows - 1.6f;
                columnTracking = 0f;
                columnCounter = 0;
            }
            if (s[i] == '0') {
                GameObject t;
                t = (GameObject)(Instantiate(floor, new Vector2(columnTracking, rows), Quaternion.identity));
            }
            if (s[i] == '2') {
                GameObject t;
                t = (GameObject)(Instantiate(wall, new Vector2(columnTracking, rows), Quaternion.identity));
            }
			if (s[i] == '3') {
                GameObject t;
                t = (GameObject)(Instantiate(exit, new Vector2(columnTracking, rows), Quaternion.identity));
            }
            if (s[i] == '4') {
                GameObject t;
                t = (GameObject)(Instantiate(floor, new Vector2(columnTracking, rows), Quaternion.identity));
                t = (GameObject)(Instantiate(key, new Vector2(columnTracking, rows), Quaternion.identity));
            }
            if (s[i] == '5') {
                GameObject t;
                t = (GameObject)(Instantiate(computer, new Vector2(columnTracking, rows), Quaternion.identity));
            }
            if (s[i] == '6' || s[i] == '7' || s[i] == '8') {
                GameObject t;
                t = (GameObject)(Instantiate(floor, new Vector2(columnTracking, rows), Quaternion.identity));
                t = (GameObject)(Instantiate(enemy, new Vector2(columnTracking, rows), Quaternion.identity));
            }
            columnTracking = columnTracking + 1.6f;
            columnCounter++;
        }
    }
}
