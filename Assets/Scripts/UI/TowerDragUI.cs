using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InputActionReference cancelAction;
    [Header("Prefabs")]
    public GameObject towerPrefab;

    [Header("Raycast")]
    [Tooltip("Include BOTH Ground + EnemyPath layers here.")]
    public LayerMask placementMask;

    [Header("Colors")]
    public Color validColor = Color.white;
    public Color invalidColor = Color.red;

    private GameObject towerInstance;
    private int enemyPathLayer;
    private TowerManager towerManager;

    public void Start()
    {
        // inputActions = new InputSystem_Actions();
        towerManager = FindObjectOfType<TowerManager>();
    }

    private void Update()
    {
        if (cancelAction.action.triggered)
        {
            Destroy(towerInstance);
            return;
        }
    }

    private void OnEnable()
    {
        cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        cancelAction.action.Disable();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        enemyPathLayer = LayerMask.NameToLayer("Enemy Path");

        towerInstance = Instantiate(towerPrefab);

        // Prevent the preview tower from being hit by the raycast
        SetLayerRecursively(towerInstance, LayerMask.NameToLayer("Ignore Raycast"));

        // Default color
        SetTowerColor(validColor);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (towerInstance == null || Camera.main == null || Mouse.current == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementMask))
        {
            towerInstance.transform.position = hit.point;

            // snap to the block
            Vector3 pos = towerInstance.transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.z = Mathf.Round(pos.z);
            towerInstance.transform.position = pos;

            bool validArea = IsOnValidArea();
            SetTowerColor(!validArea ? invalidColor : validColor);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (towerInstance == null || Camera.main == null || Mouse.current == null) return;

        bool validArea = IsOnValidArea();

        // checks if player can afford tower and its on a valid area
        CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();


        if (validArea && currencyManager.CanPlayerAfford(towerInstance.GetComponent<BaseTowerController>().towerData.buildCost))
        {
            // removes tower cost from player credit balance
            currencyManager.SubtractCredits(towerInstance.GetComponent<BaseTowerController>().towerData.buildCost);

            // adds tower to tower manager
            towerManager.AddTower(towerInstance);

            // places tower
            towerInstance = null;
            return;
        }

        // destoys tower
        Destroy(towerInstance);
        return;
    }

    private bool IsOnValidArea()
    {
        bool isOnEnemyPath = false;
        bool isTileOccupied = false;

        // checks if the tower is on the enemy path
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placementMask))
        {
            isOnEnemyPath = hit.collider.gameObject.layer == enemyPathLayer;
        }

        // checks if the tile the tower is on is currenctly occupied
        Vector3 checkPos = towerInstance.transform.position;
        Collider[] hits = Physics.OverlapSphere(checkPos, 0.4f);
        foreach (Collider c in hits)
        {
            // Ignore the tower we are currently placing (including its children)
            if (c.transform.root.gameObject == towerInstance)
                continue;

            if (c.transform.root.CompareTag("Tower") || c.CompareTag("Vegatation"))
            {
                isTileOccupied = true;
            }
        }

        // returns false if the tower is on the enemy path and or is occupied, else returns true
        if (isOnEnemyPath || isTileOccupied)
        {
            return false;
        }
        return true;
    }

    private void SetTowerColor(Color c)
    {
        if (towerInstance == null) return;

        foreach (var r in towerInstance.GetComponentsInChildren<Renderer>())
        {
            // Simple beginner-friendly approach (works for Standard shader; some URP shaders may need _BaseColor instead)
            if (r.material.HasProperty("_Color"))
                r.material.color = c;
            else if (r.material.HasProperty("_BaseColor"))
                r.material.SetColor("_BaseColor", c);
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}