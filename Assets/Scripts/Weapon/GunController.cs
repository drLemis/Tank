using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	public Transform gunPivotTransform;
	public Transform gunEndTransform;
	public Transform bulletsRoot;
	public GameObject bulletPrefab;
	public GameObject muzzleFlashPrefab;

	// С увеличением скорости снаряда увеличивается дальность покрытия
	[Range(10f, 30f)]
	public float velocity = 30f;

	private LineRenderer lineRenderer;
	private Vector3 targetPos;
	private bool active = true;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	void LateUpdate()
	{
		AimGun(targetPos);

		if (active)
		{
			RenderTrajectory();

			// Правая кнопка мыши или левый Alt
			// Корректнее, чем Input.GetKey(Keycode.Mouse1)
			if (Input.GetButtonDown("Fire2"))
				FireBullet();
		}
		else
		{
			lineRenderer.positionCount = 0;
		}
	}

	public void SetActive(bool active)
	{
		this.active = active;
	}

	public void SetTarget(Vector3 pos)
	{
		targetPos = pos;
	}

	// Пускаем снаряд с теми же параметрами, что использовали для расчёта траектории
	public void FireBullet()
	{
		GameObject newBullet = Instantiate(bulletPrefab, gunEndTransform.position, Quaternion.identity, bulletsRoot);
		Destroy(newBullet, 10f); // на тот случай, если игрок умудрится найти способ выкидывать снаряды в бездну
		Destroy(Instantiate(muzzleFlashPrefab, gunEndTransform.position, gunEndTransform.rotation, bulletsRoot), 2f);

		newBullet.GetComponent<Rigidbody>().velocity = velocity * gunPivotTransform.forward;
	}

	// Поворот ствола к цели
	void AimGun(Vector3 target)
	{
		float angle = SpecialMath.CalculateAngleToHitTarget(target, gunEndTransform.position, velocity);

		gunPivotTransform.localEulerAngles = new Vector3(360f - angle, 0f, 0f);

		transform.LookAt(target);
		transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
	}

	void RenderTrajectory()
	{
		// Начало дуги
		Vector3 currentVel = gunPivotTransform.forward * velocity;
		Vector3 currentPos = gunEndTransform.position;

		Vector3 newPos = Vector3.zero;
		Vector3 newVel = Vector3.zero;

		List<Vector3> futurePositions = new List<Vector3>();

		// Начало списка всех возможных будущих позиций снаряда на этой траектории
		futurePositions.Add(currentPos);

		// Защита от эджкейса с прицелом в бесконечность
		int maxIterations = 10000;

		for (int i = 0; i < maxIterations; i++)
		{
			// Делаем шаг, интерполируя положение снаряда в будущее
			SpecialMath.PositionStepInterpolate(Time.fixedDeltaTime, currentPos, currentVel, out newPos, out newVel);

			currentPos = newPos;
			currentVel = newVel;
			futurePositions.Add(currentPos);

			// Примем, что земли ниже чем 0 по Y не бывает
			if (currentPos.y < 0f)
				break;
		}

		lineRenderer.positionCount = futurePositions.Count;
		lineRenderer.SetPositions(futurePositions.ToArray());
	}
}
