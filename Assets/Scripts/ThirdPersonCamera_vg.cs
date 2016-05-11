using UnityEngine;
using System.Collections;

public class ThirdPersonCamera_vg : MonoBehaviour {

	#region Variables (private)

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXform;
	[SerializeField]
	private Vector3 offset = new Vector3(0f, 1.5f, 0f);

	//private global only
	private Vector3 lookDir;
	private Vector3 targetPosition;

	//smoothing and damping
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;

	#endregion



	#region Unity event functions
	// Use this for initialization
	void Start () {
		followXform = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {

		Vector3 characterOffset = followXform.position + offset;

		//Calc direction from cam to player, kill y, and normalize to give a valid direction with unit magnitude
		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0;
		lookDir.Normalize();


		targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
//		Debug.DrawRay (follow.position, Vector3.up * distanceUp, Color.red);
//		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
//		Debug.DrawLine(follow.position, targetPosition, Color.magenta);

		smoothPosition(this.transform.position, targetPosition);

		transform.LookAt (followXform);

	}
	#endregion

	#region Methods

	private void smoothPosition(Vector3 fromPos, Vector3 toPos)
	{
		//making a smooth transition between cameras current position and the position it wants to be in
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}
	#endregion Methods
}
