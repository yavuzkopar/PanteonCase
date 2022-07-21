using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yavuz.Build;

namespace Yavuz.Control
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] LayerMask mask;
        public Transform selectedObject;
        static SelectionController instance;
        [SerializeField] UnityEvent OnBuildSelected;
        [SerializeField] UnityEvent OnNotBuildSelected;
        [SerializeField] Image buildingImage;
        [SerializeField] Image productionImage;

        public static SelectionController Instance { get { return instance; } }
        private void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool hasHit = Physics.Raycast(ray, out RaycastHit hit, mask);
                if (hasHit)
                {
                    selectedObject = hit.transform;
                    if (selectedObject.CompareTag("Building"))
                    {
                        // UI Update
                        OnBuildSelected?.Invoke();

                    }
                    else if (selectedObject.CompareTag("Unit"))
                    {
                        //       


                        OnNotBuildSelected?.Invoke();
                    }
                }
                else
                {

                    selectedObject = null;
                    OnNotBuildSelected?.Invoke();
                }

            }
        }
        public void UIUpdate()
        {
            if (selectedObject.GetComponent<Building>() == null) return;

            Building b = selectedObject.GetComponent<Building>();
            buildingImage.sprite = b.buildingInfo.image;
            productionImage.sprite = b.buildingInfo.productionImage;
        }
        public void Spawn()
        {
            if (selectedObject.GetComponent<Building>() == null) return;

            Building b = selectedObject.GetComponent<Building>();
            b.SpawnUnit();
        }
    }

}