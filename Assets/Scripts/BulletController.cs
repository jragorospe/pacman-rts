using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 shootDir;
    public float moveSpeed = 100f;

    void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider _col)
    {
        if(_col.tag == "Unit") //When collide with Unit, damage by 1.
        {
            Destroy(gameObject);
            _col.gameObject.GetComponent<UnitHealth>().Damage(1);
        }
        if(_col.tag == "Wall") //Prevents bullets from passing through walls.
        {
            Destroy(gameObject);
        }
    }
}
