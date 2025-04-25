using Assets.Scripts.UIBox;

public interface UIBehaviour
{
    public abstract void Initialize(UIBoxUIController controller);
    public abstract void Process(UIBoxUIController controller);

}