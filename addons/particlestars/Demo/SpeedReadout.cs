using Godot;
using System;

public partial class SpeedReadout : ProgressBar
{
    [Export] public Label Min { get; set; }
    [Export] public Label Max { get; set; }
    [Export] public CameraMovement Camera { get; set; }
    public override void _Ready()
    {
        Min.Text = Camera.MinSpeed.ToString();
        Max.Text = Camera.MaxSpeed.ToString();
        this.MinValue = Camera.MinSpeed;
        this.MaxValue = Camera.MaxSpeed;
    }
    public override void _Process(double delta)
    {
        this.Value = Camera.CurrentSpeed;
        base._Process(delta);
    }
}
