using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze : MonoBehaviour
{
    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset t1 = (TextAsset)Resources.Load("level", typeof(TextAsset));
        string s = t1.text;
        int i;
        s = s.Replace("\n","");

        for (i = 0; i < s.Length; i++) {
            if (s[i] == '1') {

                int column, row;

                column = i % 10;

                row = i / 10;

                GameObject t;

                t = (GameObject)(Instantiate(wall, new Vector2(50 - column * 10, 50 - row * 10), Quaternion.identity));

            }
        }
    }
}
