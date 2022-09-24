using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceContainer
{
    public int[] storedResources = new int[4];
    public int[] capacities = new int[4];
}

[RequireComponent(typeof(SphereCollider))]
public class HexCellContent : MonoBehaviour
{
    public GameObject detailInformationPrefab;
    [HideInInspector] public GameObject detailInformationInstance;
    public InventoryUI inventoryUIPrefab;
    [HideInInspector] public InventoryUI inventoryUIInstance;

    public List<GameObject> meshPrefabList = new List<GameObject>();

    public Transform upperPosition;
    public ResourceContainer currentlyStoredResources;

    public GameObject meshObject;

    public virtual bool IsDestructible => true;
    public virtual bool IsInteractable => true;
    public virtual ResourceContainer Storable => new ResourceContainer();
    public virtual string Name => "DefaultContent";

    public virtual ECurrency RequiredResource => ECurrency.Acorn;
    public virtual ResourceContainer Retrievable => new ResourceContainer();

    public virtual ResourceContainer GetCost() {return new ResourceContainer(); }

    public HexCell hexCellReference;

    public bool cellCurrentlyAvailable = true;

    public virtual void Initialize(HexCell hexCell)
    {
        hexCellReference = hexCell;
        if(meshPrefabList.Count > 0)
        {
            int r = Random.Range(0, meshPrefabList.Count);
            var meshToInstantiate = meshPrefabList[r];
            var meshInstance = Instantiate(meshToInstantiate, transform.position, Quaternion.identity);
            meshInstance.transform.SetParent(meshObject.transform, true);
        }
    }

    public virtual bool SpawnConditions => true;

    void Awake() 
    {
        UpdateInventoryUI();
    }

    void Update() 
    {
        ContentUpdate();
    }

    public virtual void ContentUpdate()
    {
        if(!cellCurrentlyAvailable)
            return;
    }

    public virtual void Destruct()
    {

    }

    public void ShowDetails()
    {
        if(detailInformationInstance == null)
            detailInformationInstance = Instantiate(detailInformationPrefab, upperPosition.position, Quaternion.identity);
        
    }

    public void HideDetails()
    {
        if(detailInformationInstance != null)
            Destroy(detailInformationInstance.gameObject);
    }

    public virtual void SetAvailable(bool available)
    {
        if(inventoryUIInstance != null)
            inventoryUIInstance.gameObject.SetActive(available);

        cellCurrentlyAvailable = available;
        meshObject.SetActive(available);
    }

    public void UpdateInventoryUI()
    {
        if(inventoryUIInstance == null)
        {
            inventoryUIInstance = Instantiate(inventoryUIPrefab, upperPosition.position, Quaternion.identity);
        }
        inventoryUIInstance.UpdateUI(currentlyStoredResources);

    }

    public virtual void Store(ResourceContainer input, int type) =>
        ExchangeResources(input, currentlyStoredResources, type);

    public void Take(ResourceContainer input, int type) =>
        ExchangeResources(currentlyStoredResources, input, type);

    public virtual void ExchangeResources(ResourceContainer giver, ResourceContainer taker, int typeIndex)
    {
        int takerRemainingSpace = taker.capacities[typeIndex] - taker.storedResources[typeIndex];
        if(takerRemainingSpace >= giver.storedResources[typeIndex])
        {
            taker.storedResources[typeIndex] += giver.storedResources[typeIndex];
            giver.storedResources[typeIndex] = 0;
        } 
        else
        {
            taker.storedResources[typeIndex] += takerRemainingSpace;
            giver.storedResources[typeIndex] -= takerRemainingSpace;            
        }

        UpdateInventoryUI();
        UIManagement.Instance.UpdateCurrencyPanels();
    }
}
