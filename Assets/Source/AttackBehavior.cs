using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{

    private CharacterBehavior _character;
    public GameObject AttackBoxPrefab;
    private Transform AttackBoxTransform;

    private Dictionary<string, GameObject> Boxes;

    // Start is called before the first frame update
    void Start()
    {
        this._character = GetComponent<CharacterBehavior>();

        if (this._character.IsLocal)
        {
            Boxes = new Dictionary<string, GameObject>();
            var attackBox = new GameObject("AttackBox");
            AttackBoxTransform = attackBox.transform;
            AttackBoxTransform.parent = transform;

            for (int y = -3; y <= 3; y++)
            {
                for (int x = -3; x <= 3; x++)
                {
                    if (x == 0 && y == 0)
                    {

                    }
                    else
                    {
                        var box = Instantiate(AttackBoxPrefab, new Vector3(x, y, 0), Quaternion.identity, AttackBoxTransform);
                        var key = x.ToString() + "_" + y.ToString();
                        box.name = key;
                        box.SetActive(false);
                        Boxes.Add(key, box);
                    }

                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this._character.IsLocal)
        {
            foreach (var box in this.Boxes.Values)
            {
                box.SetActive(false);
            }

            if (GameManager.Instance.Attack.IsPreparing())
            {
                var x = Mathf.FloorToInt(transform.position.x) + 0.5f;
                var y = Mathf.FloorToInt(transform.position.y) + 0.5f;
                AttackBoxTransform.position = new Vector3(x, y, 0);

                Vector2 positionSouris = Input.mousePosition;
                Vector2 positionCentre = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Vector2 difference = positionSouris - positionCentre;
                int pxd = 0;
                int pyd = 0;
                bool reverse = false;
                if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
                {
                    if (difference.x > 0)
                    {
                        //droite
                        pxd = 1;
                        pyd = 1;
                    }
                    else
                    {
                        //gauche
                        pxd = -1;
                        pyd = 1;
                    }
                }
                else
                {
                    if (difference.y > 0)
                    {
                        //haut
                        reverse = true;
                        pxd = 1;
                        pyd = 1;
                    }
                    else
                    {
                        //bas
                        reverse = true;
                        pxd = 1;
                        pyd = -1;
                    }
                }


                foreach (var p in GameManager.Instance.Attack.GetCurrentSkill().Pattern)
                {
                    var pattern = p.Split(":");
                    var damage = int.Parse(pattern[1]);
                    var values = pattern[0].Split("_");
                    var px = 1;
                    var py = 1;
                    if (reverse)
                    {
                        px = int.Parse(values[1]);
                        py = int.Parse(values[0]);
                    }
                    else
                    {
                        px = int.Parse(values[0]);
                        py = int.Parse(values[1]);
                    }
                    var key = (px * pxd) + "_" + (py * pyd);
                    Boxes[key].SetActive(true);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.Instance.Attack.Slay();
                }
            }
            else
            {

            }
        }


    }
}
