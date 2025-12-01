using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

// ”спадковуЇмо в≥д IEndDragHandler
public class DiscreteScroll : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public GameObject shopItemPrefab;

    [Header("Ќалаштуванн€")]
    public float snapSpeed = 10f;
    public float snapThreshold = 0.2f;

    private float cardHeight;
    private bool isSnapping = false;
    private int totalItems;
    private float targetPosition;

    void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        RectTransform prefabRect = shopItemPrefab.GetComponent<RectTransform>();
        VerticalLayoutGroup layoutGroup = contentPanel.GetComponent<VerticalLayoutGroup>();

        if (prefabRect != null && layoutGroup != null)
        {
            // ¬исота картки + spacing
            cardHeight = prefabRect.sizeDelta.y + layoutGroup.spacing;
        }
    }

    void Update()
    {
        // 1. ”правл≥нн€ ѕрит€гуванн€м (Snapping)
        if (isSnapping)
        {
            // якщо ми "прит€гуЇмо", ≥гноруЇмо рух користувача
            scrollRect.velocity = Vector2.zero;

            // ≤нтерпол€ц≥€ до ц≥льовоњ позиц≥њ (Lerp)
            float newY = Mathf.Lerp(contentPanel.anchoredPosition.y, targetPosition, Time.deltaTime * snapSpeed);
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);

            // якщо ми майже дос€гли ц≥л≥, зупин€Їмо
            if (Mathf.Abs(contentPanel.anchoredPosition.y - targetPosition) < snapThreshold)
            {
                contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, targetPosition);
                isSnapping = false;
            }
        }
        // *Ћог≥ка визначенн€ к≥нц€ перет€гуванн€ повн≥стю винесена в OnEndDrag*
    }

    // Ќова функц≥€: ¬икликаЇтьс€ системою под≥й, коли користувач в≥дпускаЇ прокрутку.
    public void OnEndDrag(PointerEventData eventData)
    {
        // якщо користувач в≥дпустив прокрутку, починаЇмо "прит€гуванн€"
        SnapToNearestItem();
    }

    // ѕубл≥чна функц≥€ дл€ оновленн€ к≥лькост≥ елемент≥в
    public void SetItemCount(int count)
    {
        totalItems = count;
    }

    private void SnapToNearestItem()
    {
        if (totalItems <= 0 || cardHeight == 0) return;

        float currentY = contentPanel.anchoredPosition.y;

        // ¬изначаЇмо найближчий ≥ндекс картки
        int nearestIndex = Mathf.RoundToInt(currentY / cardHeight);

        // ќбчислюЇмо ≥деальну позиц≥ю Y
        targetPosition = nearestIndex * cardHeight;

        // ќбмежуЇмо ц≥льову позиц≥ю, щоб не вийти за меж≥
        float maxScroll = (totalItems * cardHeight) - scrollRect.GetComponent<RectTransform>().rect.height;
        if (maxScroll < 0) maxScroll = 0;

        targetPosition = Mathf.Clamp(targetPosition, 0, maxScroll);

        isSnapping = true;
    }
}