using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    public Transform aim;
    public Transform head;
    public Transform muzzlePos;
    public Transform bullet;

    public float reloadTime;
    public float turnSpeed;
    public float firePauseTime;
    public float range;

    public bool canSee = false;
    public bool isEnabled = true;

    private float nextFireTime;
    private float nextMoveTime;

    public AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(isEnabled) //Allows control during pause.
        {
            Aim();
            Tracking();
        }
    }

    void Aim()
    {
        if(target) //If something hits the sphere collider
        {
            if(Time.time >= nextMoveTime) //Check if we can move based on our move time cooldown.
            {
                aim.LookAt(target); //Grab vector data
                aim.eulerAngles = new Vector3(0, aim.eulerAngles.y, 0); //Translate movements
                head.rotation = Quaternion.Lerp(head.rotation, aim.rotation, Time.deltaTime * turnSpeed); //Move head model
            }

            if(Time.time >= nextFireTime && canSee) //Fire when shot is done cooldown and we enabled sight.
                Fire();
        }
    }

    void Fire()
    {
        nextFireTime = Time.time + reloadTime; //Set time between shots.
        nextMoveTime = Time.time + firePauseTime; //Set time between when the turret can aim.

        Transform bulletTransform = Instantiate(bullet, muzzlePos.position, Quaternion.identity); //Angle the "bullet" based on the direction determined by measuring the center of the turret and the muzzle.

        Vector3 shootDir = (muzzlePos.position - aim.position).normalized; //Bullet travel angle.
        bulletTransform.GetComponent<BulletController>().Setup(shootDir); //Fire bullet.

        audioSource.Play(); //Play gunshot noise.
    }

    void Tracking()
    {
        Vector3 fwd = muzzlePos.TransformDirection(Vector3.forward); //Determine direction of raycast.
        RaycastHit hit;
        Debug.DrawRay(muzzlePos.position, fwd * range, Color.red); //Draw raycast in Scene for testing.

        if(Physics.Raycast(muzzlePos.position, fwd, out hit, range))
        {
            if(hit.collider.CompareTag("Unit")) //If ray hits unit, turret has vision on target.
                canSee = true;
            else    
                canSee = false;
        }
    }

    public void DisableInput()
    {
        isEnabled = false;
    }

    public void EnableInput()
    {
        isEnabled = true;
    }

    void OnTriggerStay(Collider _col)
    {
        if(!target)
        {
            if(_col.CompareTag("Unit"))
            {
                nextFireTime = Time.time + (reloadTime * 0.5f);
                target = _col.gameObject.transform;
            }
        }
    }

    void OnTriggerExit(Collider _col)
    {
        if(_col.gameObject.transform == target)
            target = null;
    }
}
