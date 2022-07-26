using UnityEngine;
using Yavuz.PathFind;

namespace Yavuz.Build
{
    public class PreviewBuilding : MonoBehaviour
    {
        [SerializeField] GameObject objectToProduce;
        BuildingInfo buildingInfo;
        Vector3 offset;
        SpriteRenderer visualFeedbackSprite;
        Color goodColor, badColor;
        void Start()
        {
            buildingInfo = objectToProduce.GetComponent<Building>().buildingInfo;
            offset = objectToProduce.GetComponent<Building>().offset;
            visualFeedbackSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            goodColor = new Color(0, 1, 0, 0.3f);
            badColor = new Color(1, 0, 0, 0.3f);
            visualFeedbackSprite.color = goodColor;
            if (buildingInfo.height % 2 == 0)
                offset += Vector3.up * GridSystem.Instance.nodeRadius;
            if (buildingInfo.width % 2 == 0)
                offset += -Vector3.right * GridSystem.Instance.nodeRadius;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = GridSystem.Instance.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).worldPosition + offset;
            bool canMake = !Physics.CheckBox(transform.position, new Vector3(buildingInfo.width, buildingInfo.height) * GridSystem.Instance.nodeRadius, Quaternion.identity, GridSystem.Instance.unwalkableMask);
            if (canMake && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                visualFeedbackSprite.color = goodColor;
                if (Input.GetMouseButtonDown(0) && BuildingManager.Instance.canProduce)
                {
                    Instantiate(objectToProduce, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                    BuildingManager.Instance.canProduce = false;
                    //   GridSystem.Instance.CreateGrid();
                    // for optimization
                    GridSystem.Instance.UpdateGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset, buildingInfo.width, buildingInfo.height);
                }


            }
            else
                visualFeedbackSprite.color = badColor;
        }
    }

}