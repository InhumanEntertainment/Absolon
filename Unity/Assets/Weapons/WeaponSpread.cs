using UnityEngine;
using System.Collections;

public class WeaponSpread : Weapon
{
    public float SpreadAmount = 2;
    public float SpreadWavelength = 0.2f;
    public float SpreadValue = 0;
    public float WaveOffset;
    float Spread;
    Vector3 LastPosition = Vector3.zero;

    //=======================================================================================================================================================/
    void Update()
    {
        if(Time.timeScale == 1)
        {
            // Sine Wave //
            //WaveOffset = (float)(Mathf.Cos(Time.timeSinceLevelLoad / SpreadWavelength) * 1);

            // Ship Velocity //
            float newVal = (Player.Instance.transform.position.x - LastPosition.x) / Time.deltaTime * 1f;
            float v = Time.deltaTime * 10f;
            SpreadValue = SpreadValue * (1f - v) + newVal * v;
            Spread = SpreadValue + (float)(Random.value * .2f) - .1f;

            if (Spread > 2)
                Spread = 2;
            if (Spread < -2)
                Spread = -2;

            LastPosition = Player.Instance.transform.position;
        }
    }

    //=======================================================================================================================================================/
    public override void CreateProjectile()
    {
        GameObject projectile1 = (GameObject)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        float velocity = Mathf.Lerp(VelocityMinMax.x, VelocityMinMax.y, Game.Instance.Difficulty);
        projectile1.rigidbody2D.velocity = new Vector2(Spread, velocity);            
    }
}
