using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Transform playerModel;
	private CharacterController characterController;
	[Range(1f, 20f)]
	public float speedMovement = 1; // максимальная скорость движения
	private float velocity = 0; // сохранение импульса для разгона и торможения

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	void Update()
	{
		if (playerModel != null && characterController != null)
		{
			float horizontal = Input.GetAxis("Horizontal") * speedMovement;
			float vertical = Input.GetAxis("Vertical") * speedMovement;

			// всё ещё сможем ходить по диагонали быстрее чем по прямой
			// нужна нормализация вектора
			characterController.Move((playerModel.transform.right * horizontal + playerModel.transform.forward * vertical) * Time.deltaTime);

			// на тот случай, если игрок умудрился оторваться от земли
			if (characterController.isGrounded)
			{
				velocity = 0;
			}
			else
			{
				velocity += Physics2D.gravity.y * Time.deltaTime;
				characterController.Move(new Vector3(0, velocity, 0));
			}
		}
	}

	// синхронизируем модель игрока с камерой
	public void SetModelRotation(float newY)
	{
		if (playerModel != null)
			playerModel.eulerAngles = new Vector3(0f, newY, 0f);
	}
}
