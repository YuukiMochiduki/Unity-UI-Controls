using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEditor;

namespace UnityEngine.UI.Controls
{
    public class UIScrollPicker : MonoBehaviour
    {
        int SelectedIndex = 0;

        UIScrollPickerScrollRect scrollRect;

        float itemHeight = 80f;

        public string[] items;

        bool dragEnded;
        
        float stopSpeed = 5f;

        float tickDuration = 0.25f;

        [MenuItem("GameObject/UI/Controls/ScrollPicker")]
        static void Create()
        {
            GameObject obj = Instantiate(Resources.Load("UIScrollPicker") as GameObject);

            obj.transform.SetParent(Selection.activeTransform, false);

            obj.name = "ScrollPicker";
        }

        void Awake()
        {
            scrollRect = GetComponentInChildren<UIScrollPickerScrollRect>();

            foreach (string item in items)
            {
                AddItem(item);
            }

            scrollRect.onEndDrag += OnEndDrag;

            scrollRect.onValueChanged.AddListener(OnValueChanged);
        }

        void OnDestroy()
        {
            scrollRect.onEndDrag -= OnEndDrag;

            scrollRect.onValueChanged.RemoveAllListeners();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragEnded = true;
        }

        void OnValueChanged(Vector2 pos)
        {
            if(dragEnded && scrollRect.velocity.magnitude < stopSpeed)
            {
                dragEnded = false;

                OnStopped();
            }
        }

        void OnStopped()
        {
            SelectedIndex = (int) Mathf.Floor((itemHeight * 0.5f + scrollRect.content.anchoredPosition.y) / itemHeight);

            SelectedIndex = Mathf.Clamp(SelectedIndex, 0, items.Length);

            Vector2 pos = new Vector2(scrollRect.content.anchoredPosition.x, SelectedIndex * itemHeight);

            StartCoroutine(AutoScroll(scrollRect.content.anchoredPosition, pos, tickDuration));
        }

        IEnumerator AutoScroll(Vector2 from, Vector2 to, float duration)
        {
            Vector2 direc = (to - from);

            Vector2 speed = direc.normalized * direc.magnitude / duration;

            float startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                scrollRect.content.anchoredPosition += speed * Time.deltaTime;

                scrollRect.StopMovement();

                yield return 1;
            }

            scrollRect.content.anchoredPosition = to;

            scrollRect.StopMovement();
        }

        void OnAutoScroll(Vector2 pos)
        {
            scrollRect.content.anchoredPosition = pos;

            scrollRect.StopMovement();
        }

        public void AddItem(string item)
        {
            RectTransform buttonRectTransform = Instantiate(Resources.Load("UIPickerItem") as GameObject).GetComponent<RectTransform>();

            buttonRectTransform.GetComponent<LayoutElement>().preferredHeight = itemHeight;

            buttonRectTransform.SetParent(scrollRect.content.transform, false);

            buttonRectTransform.SetAsLastSibling();

            buttonRectTransform.GetChild(0).GetComponent<Text>().text = item;
        }

        public void RemoveItem(string item)
        {
            for (int i = 0; i < scrollRect.content.transform.childCount; i++)
            {
                if(scrollRect.content.transform.GetChild(i).GetComponent<Text>().text == item)
                {
                    Destroy(scrollRect.content.transform.GetChild(i).gameObject);

                    break;
                }
            }
        }

        public string Selected 
        {
            get
            {
                return scrollRect.content.transform.GetChild(SelectedIndex).GetComponent<Text>().text;
            }
        }
    }
}
