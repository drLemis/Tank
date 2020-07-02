using UnityEngine;

public class BulletHit : MonoBehaviour
{
	public GameObject prefabExplosion;

	// При столкновении с чем-нибудь (землёй, другими снарядами), уничтожается со взрывом
	void OnCollisionEnter(Collision other)
	{
		Destroy(Instantiate(prefabExplosion, this.transform.position, Quaternion.identity, this.transform.parent), 1f);
		Destroy(gameObject);
	}
}
