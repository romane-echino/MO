using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MO.UI;

public class EnnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private EntityWorldUI entityUI;

    private Ennemy _ennemy;
    private string characterName;
    private int level = 1;
    private int maxLife = 10;
    private int currentLife = 10;


    public void Init(Ennemy ennemy)
    {
        this._ennemy = ennemy;

        characterName = ennemy.Name;
        level = 1;
        maxLife = ennemy.MaxLife;
        currentLife = ennemy.Life;

        entityUI.InitializeLifebar(EntityType.Enemy, characterName, level, maxLife, currentLife);
        entityUI.Show();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit(int currentLife)
    {
        Debug.Log("Hit from behavior"+currentLife);
        animator.SetTrigger("Hit");
        entityUI.UpdateLife(currentLife);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Repop()
    {
        this.currentLife = this.maxLife;
        entityUI.UpdateLife(currentLife);
        gameObject.SetActive(true);
    }
}
