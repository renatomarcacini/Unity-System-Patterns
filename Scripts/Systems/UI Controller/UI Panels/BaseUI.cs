using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BaseUI : MonoBehaviour
{
    [Header("Base UI Settings")]
    [Space(10)]
    protected GameObject Panel;
    protected CanvasGroup CanvasGroup;

    public GameObject LastPanelSelectButton;
    [SerializeField] public GameObject FirstSelectButton;

    [SerializeField] private float fadeDelayEnable = 0.1f;
    [SerializeField] private float fadeDelayDisable = 0.1f;

    protected bool IsEnable = false;

    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        Panel = transform.GetChild(0).gameObject;
        Panel.SetActive(false);
    }

    public virtual void Enable()
    {
        Panel.SetActive(true);
        CanvasGroup.alpha = 0.1f;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.DOFade(1, fadeDelayEnable).SetUpdate(true).OnComplete(() =>
        {
            CanvasGroup.blocksRaycasts = true;
            IsEnable = true;
        });

        SetSelectedButton(FirstSelectButton);
    }

    public virtual void Disable()
    {
        if (!Panel.activeInHierarchy)
            return;
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.DOFade(0, fadeDelayDisable).SetUpdate(true).OnComplete(() => { 
            CanvasGroup.blocksRaycasts = false;
            Panel.SetActive(false);
            IsEnable = false;
        });
    }

    public void SetLastPanelSelectButton(Button button)
    {
        LastPanelSelectButton = button.gameObject;
    }

    public void SetSelectedButton(GameObject button)
    {
        if (button == null)
            return;

        EventSystem.current.SetSelectedGameObject(button.gameObject);

        /*
        ButtonSelected buttonSelected = EventSystem.current.currentSelectedGameObject.GetComponent<ButtonSelected>();
        if(buttonSelected != null)
            buttonSelected.UpdateSelectUIPosition();
        */
    }

}