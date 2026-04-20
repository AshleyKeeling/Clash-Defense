using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class AbilityDataDisplay : MonoBehaviour
{
    public AbilitySciptableObject abilityData;
    private TextMeshProUGUI abilityName;
    private TextMeshProUGUI abilityCost;
    private Image abilityImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityName = transform.Find("name").GetComponent<TextMeshProUGUI>();
        abilityCost = transform.Find("price").GetComponent<TextMeshProUGUI>();
        abilityImage = transform.Find("image").GetComponent<Image>();

        abilityName.text = abilityData.name;
        abilityCost.text = "₡" + abilityData.cost.ToString();
        abilityImage.sprite = abilityData.image;
    }
}
