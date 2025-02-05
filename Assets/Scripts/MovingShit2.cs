using UnityEngine;

namespace DefaultNamespace
{
    public class MovingShit2 : MonoBehaviour
    {
        public void UpdatePOsition(Vector2 movement)
        {
            transform.position += new Vector3(movement.x, 0, movement.y);
        }
    }
}