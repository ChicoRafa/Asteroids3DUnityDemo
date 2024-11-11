
using UnityEngine;

public class ExtraLifeMovement : ForwardContinuousMovement
{
    /// <summary>
    /// This method is called when the extra life goes off-screen.
    /// </summary>
    /// <remarks>
    /// It overrides the OnOffScreen method from the ForwardContinuousMovement class.
    /// The method destroys the game object associated with the extra life.
    /// </remarks>
    protected override void OnOffScreen()
    {
        SpawnPool.Instance.Despawn(transform);
    }
}
