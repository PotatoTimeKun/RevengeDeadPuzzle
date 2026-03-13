namespace MyGame.Domain
{
    public class WoodState
    {
        public float Health { get; private set; }
        public bool IsBurning { get; private set; }

        public WoodState(float initialHealth = 2.0f)
        {
            Health = initialHealth;
        }

        public void StartBurning() => IsBurning = true;

        public void ReduceHealth(float amount)
        {
            if (IsBurning) Health -= amount;
        }

        public bool IsDead => Health <= 0;
    }
}