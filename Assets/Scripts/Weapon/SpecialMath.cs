using UnityEngine;

public class SpecialMath
{
	// https://en.wikipedia.org/wiki/Projectile_motion
	// Angle THETA required to hit coordinate (x,y)
	// Чудеса школьной геометрии
	public static float CalculateAngleToHitTarget(Vector3 target, Vector3 origin, float velocity)
	{
		float v = velocity;

		// Направление на цель
		Vector3 targetVec = target - origin;

		// Расстояние Y
		float y = targetVec.y;

		// Сброс Y для корректного расчёта X
		targetVec.y = 0f;

		// Расстояние X
		float x = targetVec.magnitude;

		float g = -Physics.gravity.y;

		// Расчёт вертикального угла сведения
		float vPow = v * v;
		float rootedPart = (vPow * vPow) - g * (g * x * x + 2 * y * vPow);

		// Если цель в пределах максимальной дальности
		if (rootedPart >= 0f)
		{
			float rightSide = Mathf.Sqrt(rootedPart);
			float topPart = vPow + rightSide;
			float bottomPart = g * x;

			// Угол THETA, под которым должен быть произведён выстрел
			return Mathf.Atan2(topPart, bottomPart) * Mathf.Rad2Deg;
		}
		else
		{
			// Если цель за пределом дальности - возвращаем оптимальный угол для наиболее дальнего выстрела
			return 45f;
		}
	}

	// https://en.wikipedia.org/wiki/Heun%27s_method
	// грязно и быстро, но точно, учитываяется только гравитация
	public static void PositionStepInterpolate(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
	{
		Vector3 newVelEuler = currentVel + Time.fixedDeltaTime * Physics.gravity;
		newVel = currentVel + Time.fixedDeltaTime * 0.5f * (Physics.gravity + Physics.gravity);
		newPos = currentPos + Time.fixedDeltaTime * 0.5f * (currentVel + newVelEuler);
	}
}
