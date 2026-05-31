using UnityEngine;

public class ObstacleDestroy : Obstacles
{
    internal override void OnCollision(Collision collision)
    {
        base.OnCollision(collision);
        Destroy(gameObject);
    }


}
