using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorButtonTest : MonoBehaviour {
    [InspectorButton]
    public void Click_Me()
    {
        Debug.Log("Hello!");
    }
}
