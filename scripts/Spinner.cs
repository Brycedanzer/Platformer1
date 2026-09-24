using Godot;

// The Spinner stays in one spot and spins around
public partial class Spinner : Enemy
{
	public override void Move(double delta)
	{
		Rotation += 5 * (float)delta;
	}
}
