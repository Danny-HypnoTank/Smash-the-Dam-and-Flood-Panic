//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CursourManager : MonoBehaviour
//{
//    [SerializeField]
//    private Texture2D[] cursorTexture;

//    private void Start()
//    {
//        Cursor.SetCursor(cursorTexture[0], new Vector2(120, 0), CursorMode.ForceSoftware);
//    }

//    private void Update()
//    {
//        if (Input.GetKey(KeyCode.Mouse0))
//        {
//            Cursor.SetCursor(cursorTexture[1], new Vector2(120, 0), CursorMode.ForceSoftware);
//        }
//        else
//        {
//            Cursor.SetCursor(cursorTexture[0], new Vector2(120, 0), CursorMode.ForceSoftware);
//        }
//    }
//}
