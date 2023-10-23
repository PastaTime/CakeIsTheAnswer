using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class AsciiButton : MonoBehaviour
{
    [SerializeField] private Outline outline;

    [SerializeField] private float flashTime;

    private AsciiUIManager _manager;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        _manager = FindObjectOfType<AsciiUIManager>();
        _manager.RegisterButton(this);
    }
    private void Flash()
    {
        StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        Debug.Log("FLASH");
        outline.enabled = true;
        yield return new WaitForSeconds(flashTime);
        outline.enabled = false;
        yield return new WaitForSeconds(flashTime);
    }

    public void SetSelected(bool selected)
    {
        Debug.Log("SELECTED: " + selected);
        CancelInvoke();
        if (selected)
        {
            Debug.Log(">??");
            InvokeRepeating("Flash", 0f, 2 * flashTime);
        }
    }
}