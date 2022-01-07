using UnityEngine;
using System.Collections;

public class FOVE3DCursor : FOVEBehavior
{
	public enum LeftOrRight
	{
		Left,
		Right
	}

	[SerializeField]
	public LeftOrRight whichEye;

    int layerMask;
	// Use this for initialization
	void Start ()
    {
        layerMask = 1 << LayerMask.NameToLayer("gazeCheck");
    }

	// Latepdate ensures that the object doesn't lag behind the user's head motion
	void Update()
    {
		var rays = FoveInterface.GetGazeRays().value;
		var ray = whichEye == LeftOrRight.Left ? rays.left : rays.right;

		RaycastHit hit;
		Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

     
        if (hit.point != Vector3.zero) // Vector3 is non-nullable; comparing to null is always false
		{
            transform.position = hit.point;
        }
        else
		{
			transform.position = ray.GetPoint(19.0f);
        }
    }
}
