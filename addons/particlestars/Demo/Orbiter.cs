using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Orbiter : MeshInstance3D
{
    [Export] public Node3D OrbitalCenter { get; set; }
    [Export] public float OrbitalRadius { get; set; } = 25f;
    [Export] public float OrbitalVelocity { get; set; } = 10;

    private double tFac = 0;
    public override void _Process(double delta)
    {
        float z = OrbitalRadius * Mathf.Sin(((float)tFac*OrbitalVelocity ) * (Mathf.Pi / 180));
        float x = OrbitalRadius * Mathf.Cos(((float)tFac *OrbitalVelocity) * (Mathf.Pi / 180));

        Vector3 offset = new Vector3(x, 0, z);
        this.Position = OrbitalCenter.Position + offset;
        tFac += delta;
    }

}
