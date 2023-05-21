using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillsUIBehavior : MonoBehaviour
{


    public GameObject SkillButtonPrefab;
    private Dictionary<string, Transform> Buttons = new Dictionary<string, Transform>();
    private Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();

    // Start is called before the first frame update
    void Start()
    {
        var skills = GameManager.Instance.Attack.GetSkills();
        var index = 1;
        foreach (var skill in skills)
        {
            var button = Instantiate(SkillButtonPrefab, Vector3.zero, Quaternion.identity, transform);
            button.transform.GetComponent<Button>().onClick.AddListener(delegate { this.ButtonClicked(skill); });

            button.transform.Find("Name").GetComponent<Text>().text = skill.Icon;
            button.transform.Find("Keybinding").GetComponent<Text>().text = index.ToString();

            Buttons.Add(index.ToString(), button.transform);
            Skills.Add(index.ToString(), skill);
            index++;
        }
    }

    void ButtonClicked(Skill s)
    {
        GameManager.Instance.Attack.PrepareAttack(s);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var keybinding in this.Buttons.Keys)
        {
            if (Input.GetKeyDown(keybinding))
            {
                var go = this.Buttons[keybinding].gameObject;
                var ped = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler);
            }
        }
    }
}
