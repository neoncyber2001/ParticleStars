using Godot;
using System;

public partial class CameraMovement : Camera3D
{
    [Export] public float StartingSpeed { get; set; } = 10.0f;
    [Export] public float MaxSpeed { get; set; } = 50.0f;
    [Export] public float MinSpeed { get; set; } = 1.0f;
    [Export] public float SpeedStep { get; set; } = 1f;
    public float CurrentSpeed { get; set; }

    public override void _Ready()
    {
        CurrentSpeed = StartingSpeed;
        base._Ready();
    }
    public override void _Process(double delta)
    {
        
        if (Input.IsActionPressed("FlyForward"))
        {
            this.GlobalPosition = this.GlobalPosition+ ((this.Quaternion * Vector3.Forward) * (CurrentSpeed * (float)delta));
        }
        if (Input.IsActionPressed("FlyBack"))
        {
            this.GlobalPosition = this.GlobalPosition + ((this.Quaternion * -Vector3.Forward) * (CurrentSpeed * (float)delta));
        }
        if (Input.IsActionPressed("FlyLeft"))
        {
            this.RotateY(1 * (Mathf.Pi / 180));
        }
        if (Input.IsActionPressed("FlyRight"))
        {

            this.RotateY(-1 * (Mathf.Pi / 180));
        }
        if (Input.IsActionJustPressed("SpeedUp"))
        {
            if(CurrentSpeed + SpeedStep < MaxSpeed)
            {
                CurrentSpeed+= SpeedStep;
            }
            else
            {
                CurrentSpeed = MaxSpeed;
            }
        }
        if (Input.IsActionJustPressed("SlowDown"))
        {
            if (CurrentSpeed - SpeedStep > MinSpeed)
            {
                CurrentSpeed -= SpeedStep;
            }
            else
            {
                CurrentSpeed = MinSpeed;
            }
        }
        base._Process(delta);
    }

}
