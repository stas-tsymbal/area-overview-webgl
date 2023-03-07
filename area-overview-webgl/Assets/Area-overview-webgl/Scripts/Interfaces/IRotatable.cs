namespace Area_overview_webgl.Scripts.Interfaces
{
    public interface IRotatable
    {
        void Rotate(float horizontalValue, float verticalValue);
        void DampRotation();
        
    }
}