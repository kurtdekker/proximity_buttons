using UnityEngine;

public partial class Car : MonoBehaviour
{
	public float maxTorque;
	public float maxBrake;

	float SteeredAngle;
	const float MaxRateOfSteerAngleChange = 120.0f;

	public Transform[] tireMeshes = new Transform[4];
	
	bool isMobile { get { return (
		Application.isEditor ||
		Application.platform == RuntimePlatform.Android ||
		Application.platform == RuntimePlatform.IPhonePlayer);
		}}

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		if (isMobile) StartMobile();
	}
	
	void Update()
	{
		UpdateMeshesPositions();
	}

	float steer, accelerate, brake;

	float speed;
	const float acceleration = 1.0f;

	void FixedUpdate()
	{
		steer = Input.GetAxisRaw("Horizontal");
		accelerate = Input.GetAxisRaw("Vertical");
		brake = Input.GetKey (KeyCode.Space) ? 1.0f : 0.0f;

		if (isMobile) FixedUpdateMobile();

		speed += acceleration * accelerate * Time.deltaTime;

		float finalAngle = steer * 45f;

		SteeredAngle = Mathf.MoveTowards( SteeredAngle, finalAngle, MaxRateOfSteerAngleChange * Time.deltaTime);

		// steer visuals
		tireMeshes[1].localRotation = Quaternion.Euler( 0, SteeredAngle, 0);
		tireMeshes[3].localRotation = Quaternion.Euler( 0, SteeredAngle, 0);

		// steer logical
		var rot = Quaternion.AngleAxis( SteeredAngle * speed, Vector3.up) * rb.rotation;

		rb.MoveRotation(rot);

		// move
		var pos = transform.position + transform.forward * speed;

		speed -= speed * 0.5f * Time.deltaTime;

		rb.MovePosition( pos);
	}
	
	void UpdateMeshesPositions()
	{
		for(int i = 0; i < 4; i++)
		{
			Quaternion quat;
//			Vector3 pos;
//			wheelColliders[i].GetWorldPose(out pos, out quat);
			
//			tireMeshes[i].position = pos;
//			tireMeshes[i].rotation = quat;
		}
	}
}
