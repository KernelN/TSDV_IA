using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Obstacle : MonoBehaviour
{
    public System.Action<Obstacle> OnDestroy;

    [SerializeField] Collider2D topCol;
    [SerializeField] Collider2D bottomCol;
    
    public float left { get; private set; }
    public float right { get; private set; }
    public float top { get; private set; }
    public float bottom { get; private set; }
    // public Vector2 pos { get; private set; }
    // public Vector2 holeSize { get; private set; }

    void Start()
    {
        left = topCol.bounds.min.x;
        top = topCol.bounds.max.x;

        top = topCol.bounds.min.y;
        bottom = bottomCol.bounds.max.y;

        // pos = transform.position;
        //
        // Vector2 size;
        // size.y = bottomCol.bounds.max.y - topCol.bounds.min.y;
        // size.x = topCol.bounds.size.x;
        // holeSize = size;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(left, bottom, 0), 0.25f * Vector3.one);
        Gizmos.DrawCube(new Vector3(right, bottom, 0), 0.25f * Vector3.one);
        Gizmos.DrawCube(new Vector3(left, top, 0), 0.25f * Vector3.one);
        Gizmos.DrawCube(new Vector3(right, top, 0), 0.25f * Vector3.one);
        
        // //Draw vertical
        // //Gizmos.color = Color.green;
        // Gizmos.DrawCube(pos, holeSize.x * Vector3.right + holeSize.y * 0.1f * Vector3.up);
        //
        // //Draw horizontal
        // //Gizmos.color = Color.red;
        // Gizmos.DrawCube(pos, holeSize.x * 0.1f * Vector3.right + holeSize.y * Vector3.up);
    }

    public void CheckToDestroy()
    {
        if (this.transform.position.x - Camera.main.transform.position.x < -7.5f)
        {
            if (OnDestroy != null)
                OnDestroy.Invoke(this);

            Destroy(this.gameObject);
        }

    }
}