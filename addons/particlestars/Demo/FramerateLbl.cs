using Godot;
using System;

public partial class FramerateLbl : Label
{
    public override void _Process(double delta)
    {
        this.Text = Engine.GetFramesPerSecond().ToString("000");
        base._Process(delta);
    }
}
