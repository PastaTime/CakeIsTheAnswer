using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDayNumber : MonoBehaviour
{
    [SerializeField] private Manager _manager;
    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<Manager>();
        _manager.ResetState();
    }
}
