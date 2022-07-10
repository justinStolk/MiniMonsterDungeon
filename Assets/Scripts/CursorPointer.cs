using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPointer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.Cursor = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }
}
