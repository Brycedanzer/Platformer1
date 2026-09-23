using Godot;

public partial class Apple : Area2D
{
	[Signal]
	public delegate void CollectedEventHandler();

	private AnimatedSprite2D animatedSprite2D;
	private AudioStreamPlayer2D collectedSound;
	private CollisionShape2D collisionShape2D;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collectedSound = GetNode<AudioStreamPlayer2D>("CollectedSound");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	private void OnBodyEntered(Node2D _body)
	{
		animatedSprite2D.Animation = "collected";
		collectedSound.Play();
		EmitSignal(SignalName.Collected);
		CallDeferred(MethodName.DisableCollision);
	}

	private void DisableCollision()
	{
		collisionShape2D.Disabled = true;
	}

	private void OnAnimatedSprite2DAnimationLooped()
	{
		if (animatedSprite2D.Animation == "collected")
		{
			QueueFree();
		}
	}
}
