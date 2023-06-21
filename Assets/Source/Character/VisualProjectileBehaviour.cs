using MO.Character.BodyAspect;
using MO.Item;
using UnityEngine;

namespace MO.Character
{
    public class VisualProjectileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CharacterAppeareance appareance;

        private GameObject projectile;

        public void SpawnProjectile()
        {
            // Get the projectile
            if (appareance.EquipedWeapon == null)
                return;
           var itemVisualData = ItemManager.Instance.GetItemVisualData(appareance.EquipedWeapon.Id);
            if (itemVisualData == null || itemVisualData.ShootProjectile == false)
                return;
            projectile = appareance.CreateItem(itemVisualData.ProjectileData);
        }

        public void LaunchProjectile()
        {
            Debug.Log("Destroy proj");
            Destroy(projectile);
        }
    }
}