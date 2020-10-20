using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPrefs2 : MonoBehaviour
{
    // Start is called before the first frame update
     public static void SetBool(string key, bool state)
     {
         PlayerPrefs.SetInt(key, state ? 1 : 0);
     }
 
     public static bool GetBool(string key)
     {
         int value = PlayerPrefs.GetInt(key);
 
         if (value == 1)
         {
             return true;
         }
 
         else
         {
             return false;
         }
     }
     
     public static bool GetBool(string key, bool state)
     {
         int value = PlayerPrefs.GetInt(key, state ? 1 : 0);
 
         if (value == 1)
         {
             return true;
         }
 
         else
         {
             return false;
         }
     }
}
