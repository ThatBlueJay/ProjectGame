using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatisGrappleable;
    public Transform gunTip, camera, player;
    public Rigidbody rb;
    public float maxDistance = 100f;
    private SpringJoint joint;

    public bool isGrappling = false;

    float distanceFromPoint;

    //For reeling
    public bool isReeling = false;
    public float grappleSpeed = 5f;
    

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
            Vector3 grappleDirection = (grapplePoint - transform.position);
            rb.velocity = grappleDirection.normalized * grappleSpeed;
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
        RaycastHit hit;
        if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, whatisGrappleable))
        {
            isGrappling = true;
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
        //Only do if grappling
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
        Vector3 grappleDirection = (grapplePoint - transform.position);
        joint.maxDistance = 0f;

        if (distanceFromPoint < grappleDirection.magnitude)
        {
            float velocity = rb.velocity.magnitude;

            Vector3 newDirection = Vector3.ProjectOnPlane(rb.velocity, grappleDirection);
            rb.velocity = newDirection.normalized * velocity;
        }
        else
        {
            rb.AddForce(grappleDirection.normalized * grappleSpeed);
            distanceFromPoint = grappleDirection.magnitude;
        }
    }

    void StopReeling()
    {
        joint.maxDistance = distanceFromPoint * 0.8f;
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
