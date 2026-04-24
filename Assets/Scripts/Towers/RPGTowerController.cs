using UnityEngine;
using System.Collections;

public class RPGTowerController : BaseTowerController
{
    public GameObject rocket;
    public float rocketSpeed;
    protected override IEnumerator Fire(GameObject target)
    {
        canFire = false;
        // creates rocket
        GameObject newRocket = Instantiate(rocket, gunObj.position, transform.rotation);
        newRocket.GetComponent<Rocket>().SetTargetAndData(target, rocketSpeed, Mathf.RoundToInt(towerData.bulletDamage));
        shootEffect.Play();

        // controlls the speed of the guns fire rate
        yield return new WaitForSeconds(towerData.fireRate);

        shootEffect.Stop();
        canFire = true;
    }


}
