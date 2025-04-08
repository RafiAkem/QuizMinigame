using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Addanswbutton : MonoBehaviour
{
    [SerializeField]
    private Transform buttonField;
    [SerializeField]
    private GameObject buttonPrefab; // Prefab untuk tombol

    private void Awake()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.name = "testing" + i;
            button.transform.SetParent(buttonField, false);
        }
    }
}
