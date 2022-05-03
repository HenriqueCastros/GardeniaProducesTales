using UnityEngine;

public class FollowPath : MonoBehaviour {

    // Tank targer
    Transform goal;
    // Tank speed
    float speed = 50.0f;
    // Final distance from target
    float accuracy = 1.0f;
    // Tank rotation speed
    float rotSpeed = 20.0f;
    // Access to the WPManager script
    public GameObject wpManager;
    public GameObject player;
    // Array of waypoints
    GameObject[] wps;
    // Current waypoint
    GameObject currentNode;
    // Starting waypoint index
    int currentWP = 0;
    // Access to the Graph script
    Graph g;

    // Use this for initialization
    void Start() {

        // Get hold of wpManager and Graph scripts
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        // Set the current Node
        currentNode = wpManager.GetComponent<WPManager>().findNearestNode(transform.position);
        SetNewGoal(player.transform.position);
    }

    public void SetNewGoal(Vector3 GoalPosition) {
        GameObject goalNode = wpManager.GetComponent<WPManager>().findNearestNode(GoalPosition);

        // Use the AStar method passing it currentNode and distination
        g.AStar(currentNode, goalNode);
        // Reset index
        currentWP = 0;
    }

    public void GoBehindHeli() {
        // Use the AStar method passing it currentNode and distination
        g.AStar(currentNode, wps[11]);
        // Reset index
        currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (Vector3.Distance(
            g.getPathPoint(g.getPathLength() - 1).transform.position,
            wpManager.GetComponent<WPManager>().findNearestNode(player.transform.position).transform.position) > accuracy) {
            SetNewGoal(player.transform.position);
        }

        // If we've nowhere to go then just return
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;

        //the node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);

        //if we are close enough to the current waypoint move to next
        if (Vector3.Distance(
            g.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        //if we are not at the end of the path
        if (currentWP < g.getPathLength()) {
            goal = g.getPathPoint(currentWP).transform;

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goal.position, step);
        }

    }
}