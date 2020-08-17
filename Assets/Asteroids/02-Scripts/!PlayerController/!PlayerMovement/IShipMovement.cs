namespace Asteroid
{
    public interface IShipMovement
    {
        void MoveForward();
        void StopMoveForward();
        void RotateCounterClockwise();
        void RotateClockwise();
        void StopRotate();
    }

}
