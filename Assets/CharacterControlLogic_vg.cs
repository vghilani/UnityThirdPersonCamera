using UnityEngine;
using System.Collections;

public class CharacterControlLogic_vg : MonoBehaviour {

    #region Variables (private)

	//inspector serialized
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = 0.25f;
	[SerializeField]
	private ThirdPersonCamera_vg gamecam;
	[SerializeField]
	private float directionSpeed= 3.0f; 

	//private global only
    private float speed = 0.0f;
	private float direction = 0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        if (animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }

    }

    // Update is called once per frame
    void Update () {
	
		if (animator)
		{
			// Pull values from controller/keyboard
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");	

			//translate controls stick coordinates into world/cam/character space
			StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);

			animator.SetFloat ("Speed", speed);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);
		}
					}	
	#endregion

	#region Methods
	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
	{
		Vector3 rootDirection = root.forward;

		Vector3 stickDirection = new Vector3 (horizontal, 0, vertical);

		speedOut = stickDirection.sqrMagnitude;

		//Get camera rotation
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f; //kill y 
		Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

		// convert joystick input in Worldspace coordinates
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

//		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
//		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
//		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
//		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);

		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		angleRootToMove /= 180f;

		directionOut = angleRootToMove * directionSpeed;
	}

	#endregion Methods
}