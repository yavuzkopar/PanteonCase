using UnityEngine;

namespace Yavuz.Build
{

    [CreateAssetMenu(fileName = "New Building Info", menuName = "Custom/Building")]
    public class BuildingInfo : ScriptableObject
    {
        public int height, width;
        public Sprite image;
        public Sprite productionImage;
    }

}