using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{
    public float speed = 20.0f;
    State[] states;

    static string[] names = new string[]
    {
        "Squalian",
        "Acorina",
        "Oka",
        "Humin",
        "Hugin",
        "Chisai",
        "Hambrus",
        "Indri"

    };

    public ResourceContainer inventory, squirrelNeedsPer5s;
    public GameObject highlightPrefab;

    public Animator animator;

    public GameObject meshObject;
    public GameObject exclamationMarkPrefab;
    [HideInInspector] public GameObject exclamationMarkInstance;


    public SquirrelInformationPanel detailInformationPrefab;
    [HideInInspector] public SquirrelInformationPanel detailInformationInstance;
    public Transform upperPoint;
    public InventoryUI inventoryUIPrefab;
    [HideInInspector] public InventoryUI inventoryUIInstance;

    GameObject highlightInstance;
    public int currentStateIndex = 1;
    // Start is called before the first frame update

    float timeUntilNextFoodCheck = 0.0f;

    const float foodCheckInterval = 5.0f;

    string n;

    void Awake() 
    {
        HexGrid.Instance.AddSquirrel(this);
        meshObject.transform.localScale *= Random.Range(0.8f, 1.2f);
        speed = speed * Random.Range(0.8f, 1.2f);
        n = names[Random.Range(0, names.Length)];
    }

    void Start()
    {
        states = new State[3];
        states[0] = new Idle(this);
        states[1] = new TransportationWork(this);
        states[2] = new Tired(this);
        SetState(0);
    }

    public int stateBeforeTiredIndex = 0;

    // Update is called once per frame
    void Update()
    {
        timeUntilNextFoodCheck -= Time.deltaTime;
        if(timeUntilNextFoodCheck <= 0.0f)
        {

            var mainBase = HexGrid.Instance.mainBaseInstance;
            if(mainBase.currentlyStoredResources.storedResources[0] >= squirrelNeedsPer5s.capacities[0] &&
            mainBase.currentlyStoredResources.storedResources[2] >= squirrelNeedsPer5s.capacities[2])
            {
                mainBase.currentlyStoredResources.storedResources[0] -= squirrelNeedsPer5s.capacities[0];
                mainBase.currentlyStoredResources.storedResources[2] -= squirrelNeedsPer5s.capacities[2];
                mainBase.UpdateInventoryUI();
                UIManagement.Instance.UpdateCurrencyPanels();

                if(currentStateIndex == 2)
                {
                    if(exclamationMarkInstance != null)
                        Destroy(exclamationMarkInstance);

                    SetState(stateBeforeTiredIndex);
                }
            }
            else
            {
                if(currentStateIndex != 2)
                {
                    Debug.Log("Getting Tired");
                    stateBeforeTiredIndex = currentStateIndex;
                    exclamationMarkInstance = Instantiate(exclamationMarkPrefab, upperPoint.position, Quaternion.identity);
                    SetState(2);
                }
                
            }

            timeUntilNextFoodCheck = foodCheckInterval;
        }
        states[currentStateIndex].Update();
    }

    public void ShowInventoryUI()
    {
        if(inventoryUIInstance == null)
        {
            inventoryUIInstance = Instantiate(inventoryUIPrefab, upperPoint.position, Quaternion.identity);
            inventoryUIInstance.UpdateUI(inventory, upperPoint);

            detailInformationInstance = Instantiate(detailInformationPrefab, upperPoint.position, Quaternion.identity);
            detailInformationInstance.UpdateUI(n, currentStateIndex, upperPoint);

            if(exclamationMarkInstance != null)
                exclamationMarkInstance.SetActive(false);
        }

    }

    public void HideInventoryUI()
    {
        if(inventoryUIInstance != null)
        {
            Destroy(inventoryUIInstance.gameObject);
            Destroy(detailInformationInstance.gameObject);

            if(exclamationMarkInstance != null)
                exclamationMarkInstance.SetActive(true);
        }
    }

    public void IssueWork(HexCellContent mainBase, HexCellContent otherDeposit)
    {
        if(currentStateIndex != 2)
            SetState(1);
        else 
            stateBeforeTiredIndex = 1;

        var s = states[1] as TransportationWork;
        s.mainBase = mainBase;
        s.otherDeposit = otherDeposit;
        s.path = null;
    }

    public void SetHighlight()
    {
        if(highlightInstance != null)
        {
            return;
        }

        highlightInstance = Instantiate(highlightPrefab, Vector3.zero, Quaternion.identity);
        highlightInstance.transform.SetParent(transform, false);
    }

    public void RemoveHighlight()
    {
        if(highlightInstance != null)
            Destroy(highlightInstance);
    }

    public void Destroy()
    {
        HexGrid.Instance.RemoveSquirrel(this);
        RemoveHighlight();
        HideInventoryUI();
        if(exclamationMarkInstance != null) Destroy(exclamationMarkInstance);
        Destroy(gameObject);
    }

    void SetState(int index)
    {
        states[currentStateIndex].StateExit();
        currentStateIndex = index;
        states[currentStateIndex].StateEnter();
    }

    public void DepositResources()
    {

    }
}

public class State
{
    public SquirrelBehaviour stateMachine;

    public State(SquirrelBehaviour stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void StateEnter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void StateExit()
    {

    }
}

public class TransportationWork : State
{
    public TransportationWork(SquirrelBehaviour stateMachine) : base(stateMachine){}

    public HexCellContent mainBase;
    public HexCellContent otherDeposit;
    HexCellContent currentGoal, otherGoal;
    public List<HexCell> path;
    int currentPathIndex = 0;
    bool updateRandomV3 = false;
    float remainingWaitTime = 0.0f;

    Vector3 randomV3 = Vector3.zero;

    public void OnGridUpdate()
    {
        Debug.Log("Realize Grid is updating!");
       
        //path = whichWay ? HexGrid.Instance.FindPath(deposit2.hexCellReference, deposit1.hexCellReference) : 
        //HexGrid.Instance.FindPath(deposit1.hexCellReference, deposit2.hexCellReference);
        //currentPathIndex = 0; //TODO: check which parts have changed and maybe you can still stay on the same path
    }

    int villagePathIndex = 0;
    bool currentlyOnVillagePath = false;
    List<Vector3> currentVillagePath;

    public override void StateEnter()
    {
        base.StateEnter();
        HexGrid.Instance.OnGridUpdate += OnGridUpdate;
        path = null;
        stateMachine.animator.SetBool("Walking", true);

    }

    public override void StateExit()
    {
        stateMachine.animator.SetBool("Walking", false);

    }
    public override void Update()
    {
        base.Update();
        if(remainingWaitTime > 0.0f)
        {
            remainingWaitTime-= Time.deltaTime;
            return;
        }

        if(path == null)
        {
            path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), mainBase.hexCellReference);
            currentGoal = mainBase;
            otherGoal = otherDeposit;
            currentPathIndex = 1;
            Debug.Log("PathLength: " + path.Count);
            if(path.Count == 1)
                currentPathIndex = 0;
        }

        if(updateRandomV3)
        {
            randomV3 = new Vector3(Random.Range(-5f,5f), 0f, Random.Range(-5f,5f));
            updateRandomV3 = false;
        }

        if(path.Count > 1)
        {
            if(currentPathIndex < 0 ||currentPathIndex >= path.Count)
                Debug.Log("OOBI: " + currentPathIndex);
            var currentNode = path[currentPathIndex];
            //if on a village and there is no village path yet, get the village path
            if(currentNode.content != null && currentNode.content.Name == "Village" && currentVillagePath == null)
            {
                var villagePathFinder = currentNode.content.meshObject.GetComponentInChildren<VillagePathfinder>();

                if(currentPathIndex > 0 && currentPathIndex < path.Count - 1)
                {
                    var cameFromNode = path[currentPathIndex - 1];
                    var goToNode = path[currentPathIndex + 1];

                    int cameFromIndex = -1;
                    int goToIndex = -1;
                    for (int i = 0; i < 6; i++)
                    {
                        if(cameFromNode == currentNode.GetNeighbor(i)) cameFromIndex = i;
                        if(goToNode == currentNode.GetNeighbor(i)) goToIndex = i;

                    }

                    if(goToIndex == -1 || cameFromIndex == -1)
                    {
                        Debug.LogWarning("Somehow neighbor not found");
                        return;
                    }

                    currentVillagePath = villagePathFinder.GetPath(cameFromIndex, goToIndex);
                    currentlyOnVillagePath = true;
                }
                else
                {
                    if(currentPathIndex == 0)
                        Debug.Log("Village was at first position");

                    if(currentPathIndex == path.Count - 1)
                        Debug.Log("Village was at last position");                    
                }
            }
        }
        

        float distanceToGoal = (currentGoal.transform.position - stateMachine.transform.position).magnitude;

        if(distanceToGoal < HexMetrics.innerRadius * 0.8f)
        {
            currentPathIndex = 1;
            if(currentGoal == mainBase)
            {
                for (int i = 0; i < 4; i++)
                    currentGoal.Store(stateMachine.inventory, i, false);

                if(otherGoal.RequiredResource != ECurrency.None)
                    currentGoal.Take(stateMachine.inventory, (int)otherGoal.RequiredResource, true);

                currentGoal = otherDeposit;
                otherGoal = mainBase;
                path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), otherDeposit.hexCellReference);
            }
            else
            {
                if(currentGoal.RequiredResource != ECurrency.None)
                    currentGoal.Store(stateMachine.inventory, (int)currentGoal.RequiredResource, false);

                for (int i = 0; i < 4; i++)
                {
                    if((int)currentGoal.RequiredResource != i)
                        currentGoal.Take(stateMachine.inventory, i, false);
                }

                currentGoal = mainBase;
                otherGoal = otherDeposit;
                path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), mainBase.hexCellReference);
            }
            remainingWaitTime = Random.Range(1.0f, 1.3f);
            return;
        }

        if(currentlyOnVillagePath) //special village behaviour
        {
            Vector3 nextVillagePathWayPoint = currentVillagePath[villagePathIndex];
            Vector3 nextWayPointDir = nextVillagePathWayPoint - stateMachine.transform.position;

            if(nextWayPointDir.magnitude < (HexMetrics.innerRadius * 0.06f))
            {
                ++villagePathIndex;
                if(villagePathIndex >= currentVillagePath.Count)
                {
                    villagePathIndex = 0;
                    currentVillagePath = null;
                    currentlyOnVillagePath = false;
                    updateRandomV3 = true;
                    currentPathIndex++;
                    if(currentPathIndex >= path.Count)
                    {
                    Debug.LogWarning("Somehow goal was not reached at the end of path?");   
                    --currentPathIndex;
                    }
                }
            }

            nextWayPointDir.Normalize();
            stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, 
            Quaternion.LookRotation(nextWayPointDir, Vector3.up), 360.0f * Time.deltaTime);
            stateMachine.transform.position += nextWayPointDir * stateMachine.speed * Time.deltaTime;
        }
        else //regular behaviour
        {
            Vector3 nextWayPoint = path[currentPathIndex].transform.position + randomV3;
            Vector3 nextWayPointDir = nextWayPoint - stateMachine.transform.position;
            if(nextWayPointDir.magnitude < (HexMetrics.innerRadius * 0.15f))
            {
                ++currentPathIndex;
                if(currentPathIndex >= path.Count)
                {
                    Debug.LogWarning("Somehow goal was not reached at the end of path?");   
                    --currentPathIndex;
                }
                updateRandomV3 = true;
            }

            nextWayPointDir.Normalize();
            stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, 
            Quaternion.LookRotation(nextWayPointDir, Vector3.up), 360.0f * Time.deltaTime);
            stateMachine.transform.position += nextWayPointDir * stateMachine.speed * Time.deltaTime;
        }
    }
}

public class Tired : State
{
    public Tired(SquirrelBehaviour stateMachine) : base(stateMachine){}

    public override void StateEnter()
    {
        stateMachine.animator.SetBool("Tired", true);

    }

    public override void StateExit()
    {
        stateMachine.animator.SetBool("Tired", false);

    }
    public override void Update()
    {
        base.Update();
    }
}

public class Idle : State
{
    public Idle(SquirrelBehaviour stateMachine) : base(stateMachine){}

    public override void StateEnter()
    {
        stateMachine.animator.SetBool("Idle", true);
    }

    public override void StateExit()
    {
        stateMachine.animator.SetBool("Idle", false);
    }
    public override void Update()
    {
        base.Update();
    }
}
