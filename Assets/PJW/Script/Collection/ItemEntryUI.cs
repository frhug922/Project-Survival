using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ItemEntryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;

    /// <summary>
    /// 아이템 엔트리를 초기화합니다.
    /// </summary>
    /// <param name="item">데이터가 담긴 ItemSO</param>
    /// <param name="isCollected">획득 여부</param>
    public void Initialize(CollectionSO item, bool isCollected)
    {
            if (!isCollected)
        {
            nameText.text        = "???";
            descriptionText.text = "???";
            icon.sprite          = item.SilhouetteSprite;
        }
        else
        {
            nameText.text        = item.CollectionName;
            descriptionText.text = item.CollectionDescription;
            icon.sprite          = item.CollectionIcon;
        }
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipPanel == null) return;
        tooltipPanel.transform.SetAsLastSibling();
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPanel == null) return;
        tooltipPanel.SetActive(false);
    }
}

