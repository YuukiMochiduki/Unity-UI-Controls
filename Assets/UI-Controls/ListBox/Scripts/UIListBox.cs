using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI.Controls
{
    [System.Serializable]
    public class UIListBoxClickCallBack : UnityEvent<string>
    {
    }

    public class UIListBox : MonoBehaviour {

        public Text buttonText;

        GameObject listbox;

        public int selectedIndex;

        public string[] items;

        ToggleGroup toggleGroup;

        public UIListBoxClickCallBack onSelected;

        [MenuItem("GameObject/UI/Controls/ListBox")]
        static void Create()
        {
            GameObject obj = Instantiate(Resources.Load("UIListBox") as GameObject);

            obj.transform.SetParent(Selection.activeTransform, false);

            obj.name = "ListBox";
        }

        void Awake()
        {
            foreach(string item in items)
            {
                AddItem(item);
            }

            if(listbox.transform.childCount > selectedIndex)
            {
                Toggle toggle = listbox.transform.GetChild(selectedIndex).GetComponent<Toggle>();

                toggle.isOn = true;

                buttonText.text = toggle.GetComponentInChildren<Text>().text;
            }
        }

        public void AddItem(string item)
        {
            if (listbox == null)
                listbox = transform.FindChild("ListBox").gameObject;

            if (toggleGroup == null)
                toggleGroup = listbox.GetComponent<ToggleGroup>();

            GameObject obj = Instantiate(Resources.Load("UIListBoxItem") as GameObject);

            obj.transform.SetParent(listbox.transform, false);

            Toggle toggle = obj.GetComponent<Toggle>();

            toggle.group = toggleGroup;

            toggle.GetComponentInChildren<Text>().text = item;

            toggle.onValueChanged.AddListener((isOn) =>
            {
                Text text = toggle.GetComponentInChildren<Text>();

                if (onSelected != null)
                    onSelected.Invoke(text.text);

                buttonText.text = text.text;

                listbox.gameObject.SetActive(false);
            });
        }

	    public void OnClickListBox()
        {
            listbox.gameObject.SetActive(true);
        }

        void LateUpdate()
        {
            if (listbox.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Hide());
                }
            }
        }

        IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.2f);

            listbox.gameObject.SetActive(false);
        }      
    }
}
