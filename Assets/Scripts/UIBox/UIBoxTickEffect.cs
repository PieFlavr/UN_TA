namespace Assets.Scripts.UIBox
{
    public class UIBoxTickEffect : UIBoxEffect
    {
        public float tickInterval;
        public float tickDuration;
        public override void Apply(UIBoxController controller)
        {
            base.Apply(controller);
            // Implement tick effect logic here
        }
        public override void Process(UIBoxController controller)
        {
            base.Process(controller);
            // Implement tick effect update logic here
        }
        public override void Clear(UIBoxController controller)
        {
            base.Clear(controller);
            // Implement tick effect clear logic here
        }

        public virtual void Tick(UIBoxController controller)
        {
            if (!isActive) return;

            // Implement tick logic here
            foreach (var effect in sub_effect)
            {
                if (effect is UIBoxTickEffect tickEffect && tickEffect.isActive)
                {
                    tickEffect.Tick(controller);
                }
            }
        }
    }
}
