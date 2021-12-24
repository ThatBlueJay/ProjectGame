using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector4 grapplePoint;
    public LayerMask whatisGrappleable;
    public Transform gunTip, camera, player;
    public float maxDistance = 100f;
    private SpringJoint joint;

    public bool isGrappling = false;
    public bool isReeling = false;

    //Grapple point properties
    public RaycastHit hit;
    public float distanceFromPoint;

    private void Awake()
    {
        isGrappling = false;
        isReeling = false;
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !isGrappling)
        {
            isGrappling = true;
            StartGrapple();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isGrappling)
        {
            isGrappling = false;
            StopGrapple();
        }

        if(isGrappling && Input.GetKeyDown(KeyCode.E))
        {
            isReeling = true;
            StartReeling();
        }
        else if (isGrappling && Input.GetKeyUp(KeyCode.E))
        {
            isReeling = false;
            StopReeling();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, whatisGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            distanceFromPoint = Vector3.Distance(a: player.position, b: grapplePoint);

            //The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7;
            joint.massScale = 4.5f;

            lr.positionCount = 2;


        }
    }

    void DrawRope()
    {
        //If not grapple
        if (!joint) return;
        lr.SetPosition(index: 0, gunTip.position);
        lr.SetPosition(index: 1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void StartReeling()
    {
        
    }

    void StopReeling()
    {

    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
