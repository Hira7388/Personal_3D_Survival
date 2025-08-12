using UnityEngine;

public enum PlatformType
{ 
    Up,
    Forward
}

public interface ILauncher
{
    public void Jump(Rigidbody target);
}