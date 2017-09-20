using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : Singleton<ToggleCanvas> {

    public GameObject canvas;

    public void Show()
    {
        canvas.SetActive(true);
    }

    public void Hide()
    {
        canvas.SetActive(false);
    }


}
