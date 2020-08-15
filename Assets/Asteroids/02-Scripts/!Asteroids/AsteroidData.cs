namespace Asteroid
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "@Asteroid/Asteroid Data", fileName = "New AsteroidData")]
    public class AsteroidData : ScriptableObject
    {
        public AsteroidComponent[] prefabVariants;
        public float minSpeed = 1;
        public float maxSpeed = 5;

        public string AsteroidID => this.name;
    }

}
