using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class TowerDataDisplay : MonoBehaviour
{
    public TowerDataScriptableObject towerData;
    private TextMeshProUGUI towerName;
    private TextMeshProUGUI towerPrice;
    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerName = transform.Find("name").GetComponent<TextMeshProUGUI>();
        towerPrice = transform.Find("price").GetComponent<TextMeshProUGUI>();
        image = transform.Find("image").GetComponent<Image>();

        towerName.text = towerData.name + "<br><i>Mark " + towerData.markLevel.ToString() + "</i>";
        towerPrice.text = "₡" + towerData.buildCost.ToString();
        image.sprite = towerData.towerImg;
    }
}
