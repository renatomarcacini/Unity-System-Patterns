using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public GameObject SelectUI { get; private set; }

    [field: SerializeField] public GameplayUI GameplayUI { get; private set; }
    [field: SerializeField] public MenuUI MenuUI { get; private set; }
    [field: SerializeField] public OptionUI OptionUI { get; private set; }

    public void DisableAll()
    {
        GameplayUI?.Disable();
        MenuUI?.Disable();
        OptionUI?.Disable();
        SelectUI.SetActive(false);
    }
}
