using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{
    public float speed = 20.0f;
    State[] states;

    public ResourceContainer inventory;
    public GameObject highlightPrefab;

    GameObject highlightInstance;
    int currentStateIndex = 1;
    // Start is called before the first frame update

    void Awake() 
    {
        HexGrid.Instance.allSquirrels.Add(this);
    }

    void Start()
    {
        states = new State[2];
        states[0] = new Idle(this);
        states[1] = new TransportationWork(this);
        SetState(0);
    }

    // Update is called once per frame
    void Update()
    {
        states[currentStateIndex].Update();
    }

    void SquirrelTransporting()
    {

    }

    public void IssueWork(HexCellContent mainBase, HexCellContent otherDeposit)
    {
        SetState(1);
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

    void SetState(int index)
    {
        currentStateIndex = index;
        states[currentStateIndex].StateEnter();
    }

    public void DepositResources()
    {

    }
}

/**
- Commands: 

    - Do nothing
    - Wood to Base -> Trade Post to Base
    - Stone to Base -> Trade Post to Base
    - Acorn to Base -> Production Site to Base
    - Water to Base -> Production Site to Base
    - Acorn to Beaver Post -> Base to Trade Post || Production Site to Trade Post
    - Water to Mole Post -> Base to Trade Post || Production Site to Trade Post
*/

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
}

public class TransportationWork : State
{
    public TransportationWork(SquirrelBehaviour stateMachine) : base(stateMachine){}

    public HexCellContent mainBase;
    public HexCellContent otherDeposit;
    HexCellContent currentGoal, otherGoal;
    public List<HexCell> path;
    int currentPathIndex = 0;

    public void OnGridUpdate()
    {
        Debug.Log("Realize Grid is updating!");
       
        //path = whichWay ? HexGrid.Instance.FindPath(deposit2.hexCellReference, deposit1.hexCellReference) : 
        //HexGrid.Instance.FindPath(deposit1.hexCellReference, deposit2.hexCellReference);
        //currentPathIndex = 0; //TODO: check which parts have changed and maybe you can still stay on the same path
    }

    public override void StateEnter()
    {
        base.StateEnter();
        HexGrid.Instance.OnGridUpdate += OnGridUpdate;
    }
    public override void Update()
    {
        base.Update();


        if(path == null)
        {
            path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), mainBase.hexCellReference);
            currentGoal = mainBase;
            otherGoal = otherDeposit;
            currentPathIndex = 0;
            Debug.Log(path.Count);
        }

        HexCell nextWayPoint = path[currentPathIndex];
        float distanceToGoal = (currentGoal.transform.position - stateMachine.transform.position).magnitude;

        if(distanceToGoal < HexMetrics.innerRadius * 0.8f)
        {
            currentPathIndex = 0;
            if(currentGoal == mainBase)
            {
                for (int i = 0; i < 4; i++)
                    currentGoal.Store(stateMachine.inventory, i);

                if(otherGoal.RequiredResource != ECurrency.None)
                    currentGoal.Take(stateMachine.inventory, (int)otherGoal.RequiredResource);

                currentGoal = otherDeposit;
                otherGoal = mainBase;
                path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), otherDeposit.hexCellReference);
            }
            else
            {
                if(currentGoal.RequiredResource != ECurrency.None)
                    currentGoal.Store(stateMachine.inventory, (int)currentGoal.RequiredResource);

                for (int i = 0; i < 4; i++)
                {
                    if((int)currentGoal.RequiredResource != i)
                        currentGoal.Take(stateMachine.inventory, i);
                }

                currentGoal = mainBase;
                otherGoal = otherDeposit;
                path = HexGrid.Instance.FindPath(HexGrid.Instance.CellFromPosition(stateMachine.transform.position), mainBase.hexCellReference);
            }
            return;
        }

        Vector3 nextWayPointDir = nextWayPoint.transform.position - stateMachine.transform.position;
        if(nextWayPointDir.magnitude < (HexMetrics.innerRadius * 0.1f))
        {
            ++currentPathIndex;
        }

        nextWayPointDir.Normalize();
        stateMachine.transform.position += nextWayPointDir * stateMachine.speed * Time.deltaTime;
    }
}

public class Idle : State
{
    public Idle(SquirrelBehaviour stateMachine) : base(stateMachine){}

    public override void Update()
    {
        base.Update();
    }
}
