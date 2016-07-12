using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Controls
{
    public class UIScrollPickerScrollRect : ScrollRect
    {
        public event Action<PointerEventData> onEndDrag;

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (onEndDrag != null) onEndDrag(eventData);
        }
    }
}