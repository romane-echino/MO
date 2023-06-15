using MO.Character;
using MO.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MO.Character.BodyAspect;
using MO.Item;

public partial class CharacterBehavior : MonoBehaviour
{
    public string PlayerId { get; private set; }
    public bool IsLocal { get; private set; }

    [Header("Animation")]
    public float DisplacementLimitToMove = 0.01f;

    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private EntityWorldUI entityUI;
    [SerializeField]
    private CharacterAppeareance appeareance;

    private Vector3 lastPosition;
    private bool lastDirAnimationLeft = true;

    // TODO : Replace it with a proper system to handle life  and level?
    private string characterName;
    private int level = 1;
    private int maxLife = 10;
    private int currentLife = 10;

    private int equipedWeaponSlot = 0;

    private ItemObject[] equipedItemsSlots;

    public void Init(string id, bool isLocal = false)
    {
        this.PlayerId = id;
        this.IsLocal = isLocal;

        if (isLocal)
        {
            characterName = GameManager.Instance.Auth.user.username;
        }
        else
        {
            characterName = $"Player {id}";
        }

        level = 1;
        maxLife = 10;
        currentLife = 10;
        entityUI.InitializeLifebar(EntityType.Ally, characterName, level, maxLife, currentLife);
        entityUI.Show();
    }

    public void EquipItems(ItemObject[] slots)
    {
        equipedItemsSlots = slots;
        appeareance.ApplyEquipedItems(slots);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        // Animation update
        Vector3 movement = transform.position - lastPosition;
        bool onMovement = Mathf.Abs(movement.x) > DisplacementLimitToMove || Mathf.Abs(movement.y) > DisplacementLimitToMove;
        animator.SetBool("Move", onMovement);

        bool leftDir = movement.x < 0f;
        if (leftDir != lastDirAnimationLeft && onMovement) // Just to be sure to not change the facing when not moving
        {
            animator.SetBool("DirectionLeft", leftDir);
            lastDirAnimationLeft = leftDir;
        }

        lastPosition = transform.position;
    }

    /// <summary>
    /// Start the attack animation
    /// </summary>
    /// <param name="animationType">The type of animation, 0 is fist punch</param>
    public void AttackAnimation()
    {
        var animationType = AnimationAttackType.Punch;
        if(equipedItemsSlots != null && equipedItemsSlots[(int)ItemType.EquipedWeapon] != null){
            animationType = ItemManager.Instance.GetItemVisualData(equipedItemsSlots[(int)ItemType.EquipedWeapon].Id).AnimationAttackType;
        }
        animator.SetFloat("AttackType", (float)animationType);
        animator.SetTrigger("Attack");
    }

#if UNITY_EDITOR
    [ContextMenu("-10 life")]
    private void TestGetDamage()
    {
        currentLife -= 2;
        entityUI.UpdateLife(currentLife);
    }

    [ContextMenu("Full life")]
    private void FullLife()
    {
        currentLife = maxLife;
        entityUI.UpdateLife(maxLife);
    }
#endif
}
