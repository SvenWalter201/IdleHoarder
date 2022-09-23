using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBehaviour : MonoBehaviour
{
    public float speed = 20.0f;
    State[] states;

    public ResourceContainer inventory;
    int currentStateIndex = 1;
    // Start is called before the first frame update
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

    public HexCell beginLocation;
    public HexCell endLocation;

    List<HexCell> path;
    int currentPathIndex = 0;
    bool whichWay = false;

    public void OnGridUpdate()
    {
        Debug.Log("Realize Grid is updating!");
        if(!beginLocation.isAvailable)
        {
            beginLocation = HexGrid.Instance.GetRandom();
        }
        if(!endLocation.isAvailable)
        {
            endLocation = HexGrid.Instance.GetRandom();
        }
        path = whichWay ? HexGrid.Instance.FindPath(endLocation, beginLocation) : HexGrid.Instance.FindPath(beginLocation, endLocation);
        currentPathIndex = 0; //TODO: check which parts have changed and maybe you can still stay on the same path
    }

    public override void StateEnter()
    {
        base.StateEnter();
        HexGrid.Instance.OnGridUpdate += OnGridUpdate;
    }
    public override void Update()
    {
        base.Update();
        if(beginLocation == null)
        {
            beginLocation = HexGrid.Instance.GetRandom();
            endLocation = HexGrid.Instance.GetRandom();
        }

        if(path == null)
        {
            path = whichWay ? HexGrid.Instance.FindPath(endLocation, beginLocation) : HexGrid.Instance.FindPath(beginLocation, endLocation);
            Debug.Log(path.Count);
        }


        HexCell currentGoal = whichWay ? endLocation : beginLocation;
        HexCell nextWayPoint = path[currentPathIndex];
        float distanceToGoal = (currentGoal.transform.position - stateMachine.transform.position).magnitude;


        if(distanceToGoal < HexMetrics.innerRadius)
        {
            whichWay = !whichWay;
            path = null;
            currentPathIndex = 0;
            return;
        }

        Vector3 nextWayPointDir = nextWayPoint.transform.position - stateMachine.transform.position;
        if(nextWayPointDir.magnitude < (HexMetrics.innerRadius * 0.5f))
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
