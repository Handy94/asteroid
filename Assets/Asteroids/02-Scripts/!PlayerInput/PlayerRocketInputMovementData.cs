namespace Asteroid
{
    using HandyPackage;
    using Sirenix.Serialization;

    [System.Serializable]
    public class PlayerRocketInputMovementData
    {
        [System.NonSerialized, OdinSerialize] public InputListener ThrustForwardInputListener;
        [System.NonSerialized, OdinSerialize] public InputListener StopThrustForwardInputListener;
        [System.NonSerialized, OdinSerialize] public InputListener ThrustLeftInputListener;
        [System.NonSerialized, OdinSerialize] public InputListener StopThrustLeftInputListener;
        [System.NonSerialized, OdinSerialize] public InputListener ThrustRightInputListener;
        [System.NonSerialized, OdinSerialize] public InputListener StopThrustRightInputListener;
    }

}
