using UnityEngine;
using UnityEngine.UI;

public class PlayerAim : MonoBehaviour
{
	// картинка прицела на UI
	public Image aimImage;

	// состояния прицела
	public enum AimState
	{
		INACTIVE = 0,
		SEMIACTIVE = 1,
		ACTIVE = 2
	}

	// спрайты для состояний
	public Sprite[] aimSprites;

	private GunController gunController;

	private void Awake()
	{
		gunController = GetComponent<GunController>();
	}

	public void AimTo(Vector3 toVec)
	{
		// если смотрим в бесконечность
		if (float.IsInfinity(toVec.x))
		{
			SetAim((int)AimState.INACTIVE);
		}
		else
		{
			SetAim((int)AimState.ACTIVE);

			// если смотрим на цель, но она дальше максимальной дальности
			// потенциальный допил - автоподстройка дальности, но тогда теряется смысл в анимации ствола и реальной баллистике
			if (SpecialMath.CalculateAngleToHitTarget(toVec, gunController.gunEndTransform.position, gunController.velocity) <= 45f)
			{
				SetAim((int)AimState.SEMIACTIVE);
			}
		}
	}
	
	// выставляем картинку в зависимости от состояния
	public void SetAim(int index)
	{
		if (index >= 0 && index < aimSprites.Length)
		{
			aimImage.sprite = aimSprites[index];
		}
	}
}