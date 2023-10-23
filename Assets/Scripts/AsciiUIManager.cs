using System.Collections.Generic;
using UnityEngine;

public class AsciiUIManager : MonoBehaviour
{
    [SerializeField] List<AsciiButton> _buttons = new();

    private int _selectedIndex = 0;
    // Start is called before the first frame update
    
    private void SelectButton(int index)
    {
        Debug.Log("SELECT BUTTON: " + index);
        if (index < 0)
        {
            index += _buttons.Count;
        }
        else
        {
            index %= _buttons.Count;
        }

        _buttons[_selectedIndex].SetSelected(false);
        _selectedIndex = index;
        _buttons[_selectedIndex].SetSelected(true);
    }

    // Update is called once per frame
    void Update()
    {
        // INPUT HANDLING CODE
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectButton(_selectedIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectButton(_selectedIndex - 1);
        }
    }

    public void RegisterButton(AsciiButton button)
    {
        if (_buttons.Count == 0)
        {
            button.SetSelected(true);
        }
        
        _buttons.Add(button);
    }
}
