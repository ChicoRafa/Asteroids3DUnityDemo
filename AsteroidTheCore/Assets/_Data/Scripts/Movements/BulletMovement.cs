public class BulletMovement : ForwardContinuousMovement
{
    /// <summary>
    /// This method is called when the bullet goes off-screen.
    /// </summary>
    /// <remarks>
    /// It overrides the OnOffScreen method from the ForwardContinuousMovement class.
    /// The method destroys the game object associated with the bullet.
    /// </remarks>
    protected override void OnOffScreen()
    {
        SpawnPool.Instance.Despawn(transform);
    }
}