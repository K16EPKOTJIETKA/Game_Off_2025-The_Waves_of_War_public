using UnityEngine;
using UnityEngine.UI;
using Injection;

public class ManualSliderScroll : MonoBehaviour
{
    [Header("UI Ссылки")]
    public RectTransform container;   // Объект, который будем двигать (Container)
    public Slider slider;             // Слайдер управления

    [Header("Настройки Размеров")]
    [Tooltip("Высота одной карточки товара")]
    public float itemHeight = 100f;
    [Tooltip("Отступ между товарами (из Vertical Layout Group)")]
    public float spacing = 10f;

    [Header("Настройки Списка")]
    public int visibleItemsCount = 3; // Сколько влезает в окно
    private int totalItemsCount = 0;  // Будем считать автоматически

    public void Initialize()
    {
        // Подписываемся на изменения слайдера
        if (slider != null)
        {
            slider.value = 0;
            slider.onValueChanged.AddListener(OnSliderChanged);
            // Инициализация (если товары уже есть)
           // RecalculateCounts();
        }
    }

    // Вызывайте этот метод каждый раз, когда добавляете/удаляете товары
    public void RecalculateCounts()
    {
        // Считаем детей контейнера
        totalItemsCount = container.childCount;

        // Настраиваем размер ручки слайдера (опционально)
        if (totalItemsCount > visibleItemsCount)
        {
            slider.interactable = true;
            // Пропорция ручки
            // (Если слайдер поддерживает size, но у обычного Slider этого нет, 
            // это актуально для Scrollbar. Для Slider можно пропустить).
        }
        else
        {
            // Если товаров мало, слайдер не нужен
            slider.interactable = false;
            slider.gameObject.SetActive(false);
            slider.value = 0;
        }

        // Форсируем обновление позиции
        OnSliderChanged(slider.value);
    }

    // Главная логика движения
    private void OnSliderChanged(float value)
    {
        // 1. Сколько шагов мы можем сделать вниз?
        // (Всего - Видимые). Если товаров 10, а видно 3, то скрыто 7.
        int maxHiddenItems = totalItemsCount - visibleItemsCount;

        if (maxHiddenItems <= 0)
        {
            // Нечего скроллить, ставим в начало
            container.anchoredPosition = new Vector2(container.anchoredPosition.x, 0);
            return;
        }

        // 2. Превращаем 0..1 слайдера в конкретный номер шага (индекс)
        // Mathf.RoundToInt делает движение дискретным (рывками)
        // value у нас идет от 0 (верх) до 1 (низ), если Direction = TopToBottom
        int targetStepIndex = Mathf.RoundToInt(value * maxHiddenItems);

        // 3. Считаем позицию Y
        // Y должен подниматься вверх, чтобы показать нижние элементы.
        // Высота шага = высота элемента + отступ
        float stepSize = itemHeight + spacing;
        float newY = targetStepIndex * stepSize;

        // 4. Применяем позицию
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, newY);
    }
}
