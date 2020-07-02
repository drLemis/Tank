using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float angleVerticalMin = 20f;
	public float angleVerticalMax = -80f;
	private float rotationCurrentX = 0.0f;
	private float rotationCurrentY = 0.0f;

	private PlayerAim playerAim;
	private Transform camRack;
	private PlayerController playerController;
	private GunController gunController;

	void Start()
	{
		camRack = Camera.main.transform.parent;
		playerController = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		playerAim = GetComponent<PlayerAim>();
	}

	void LateUpdate()
	{
		// вращаем камеру синхронно с мышью
		rotationCurrentX += Input.GetAxis("Mouse X");
		rotationCurrentY -= Input.GetAxis("Mouse Y");
		rotationCurrentY = Mathf.Clamp(rotationCurrentY, angleVerticalMax, angleVerticalMin);

		// синхронизируем модель игрока если таковая имеется
		camRack.eulerAngles = new Vector3(rotationCurrentY, rotationCurrentX, 0.0f);
		playerController.SetModelRotation(camRack.eulerAngles.y);

		CheckDistance();

		// курсор залочен в пределах окна
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// сверяемся, куда мы смотрим, переключаем спрайт прицела
	void CheckDistance()
	{
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;

		// учитываем пересечения только с землёй
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
		{
			gunController.SetActive(true);
			playerAim.AimTo(hit.point);
			gunController.SetTarget(hit.point);
		}
		else
		{
			gunController.SetActive(false);
			playerAim.AimTo(Vector3.negativeInfinity);
		}
	}
}