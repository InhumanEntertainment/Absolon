using UnityEngine;
using System.Collections;

public class WeaponBeast : Weapon
{
    public Transform Weapon1;
    public Transform Weapon2;
    bool Toggle;

    //=======================================================================================================================================================/
    public override void CreateProjectile()
    {
        Toggle = !Toggle;
        Vector3 pos = Weapon1.transform.position;
        if(Toggle)
            pos = Weapon2.transform.position;    

        GameObject projectile1 = (GameObject)Game.Spawn(Projectile, pos, Quaternion.identity);
        float velocity = Mathf.Lerp(VelocityMinMax.x, VelocityMinMax.y, Game.Instance.Difficulty);
        projectile1.rigidbody2D.velocity = new Vector2(0, velocity);            
    }
}
