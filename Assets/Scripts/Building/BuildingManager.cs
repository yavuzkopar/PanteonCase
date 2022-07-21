using UnityEngine;

namespace Yavuz.Build
{
    public class BuildingManager : MonoBehaviour
    {
        private static BuildingManager instance;
        public static BuildingManager Instance { get { return instance; } }

        public bool canProduce;
        private void Awake()
        {
            instance = this;
        }
        public void ProduceBuild(GameObject previewBuilding)
        {
            if (!canProduce)
            {
                previewBuilding.SetActive(true);
                canProduce = true;
            }
        }
    }


}