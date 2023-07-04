using MO.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MO.UI
{
    public class ShowKeyUIManager : Singleton<MonoBehaviour>
    {
        public List<ShowKeyBehaviour> usedKeyUI = new List<ShowKeyBehaviour>();

        public List<ShowKeyBehaviour> unusedKeyUI = new List<ShowKeyBehaviour>();

        public GameObject KeyUIPrefab;

        [Header("References")]
        [SerializeField]
        private Canvas canvas;

        private Dictionary<KeyCode, ShowKeyData> requiredShowKey = new Dictionary<KeyCode, ShowKeyData>();

        public void ShowKeyUI(KeyCode keycode, Vector3 worldPosition, string description = null, int priority = 0)
        {
            if (requiredShowKey.ContainsKey(keycode))
            {
                var actualData = requiredShowKey[keycode];
                if (priority > actualData.Priority)
                {
                    requiredShowKey[keycode] = new ShowKeyData() { Key = keycode, WorldPosition = worldPosition, Description = description, Priority = priority };
                    return;
                }
            }

            requiredShowKey.Add(keycode, new ShowKeyData() { Key = keycode, WorldPosition = worldPosition, Description = description, Priority = priority });
        }

        private void LateUpdate()
        {
            int itemUsedIdx = 0;
            foreach (KeyCode key in requiredShowKey.Keys)
            {
                ShowKeyBehaviour keyBehaviour;
                if(usedKeyUI.Count > itemUsedIdx)
                {
                    keyBehaviour = usedKeyUI[itemUsedIdx];
                }
                else
                {
                    keyBehaviour = GetKeyBehaviour();
                    usedKeyUI.Add(keyBehaviour);
                }

                keyBehaviour.SetKey(key, requiredShowKey[key].Description);
                keyBehaviour.transform.position = requiredShowKey[key].WorldPosition;

                itemUsedIdx++;
            }

            // Clear request
            requiredShowKey.Clear();

            // Clear unused keyBehaviour
            for (int i = usedKeyUI.Count - 1; i >= itemUsedIdx; i--)
            {
                usedKeyUI[i].gameObject.SetActive(false);
                unusedKeyUI.Add(usedKeyUI[i]);
                usedKeyUI.RemoveAt(i);
            }
        }

        private ShowKeyBehaviour GetKeyBehaviour()
        {
            if(unusedKeyUI.Count > 0)
            {
                return unusedKeyUI[0];
            }
            else
            {
                return CreateNewKeyUIItem();
            }
        }

        private ShowKeyBehaviour CreateNewKeyUIItem()
        {
            var newObject = Instantiate(KeyUIPrefab, transform);
            return newObject.GetComponent<ShowKeyBehaviour>();
        }

        private struct ShowKeyData
        {
            public KeyCode Key;
            public Vector3 WorldPosition;
            public string Description;
            public int Priority;
        }
    }
}

