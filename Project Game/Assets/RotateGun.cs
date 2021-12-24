using UnityEngine;

public class RotateGun : MonoBehaviour
{
    // Start is called before the first frame update
    public GrapplingGun grappling;

    public Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    private void Update()
    {
        if (grappling.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(a: transform.rotation, b: desiredRotation, t: Time.deltaTime * rotationSpeed);
    }
}
