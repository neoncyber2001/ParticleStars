using Godot;
using Godot.Collections;
using System;

/// <summary>
/// Node Star Generatior node for the Godot scene.
/// </summary>
/// <remarks>
/// This node instanciates a particle system around a target -- a space ship for example -- but only allows the system to simulate while the target is moving, modulating the particle simulation speed with the speed of the target. This allows the starfield to look static while the target is stopped and new stars are generated (and die) while the target is moving. The particles are generated on the global coordinate system but the particle system itself is locked around the target leaving a 'trail' of stars whereever the target goes. This creates a relitavely convincing illusion of moving through space.
/// </remarks>
public partial class StarGeneratorNode : Node3D
{
    private PackedScene templateStarfield = ResourceLoader.Load<PackedScene>("res://addons/particlestars/DefaultData/StarGenerator.tscn");
    private GpuParticles3D particles;
    private Node3D trackingTarget;
    private bool isEnabled = true;
    /// <summary>
    /// This is the target that the particle system will be attached to. Use the Retarget(Node3d) method to change the target via a script.
    /// </summary>
    /// <see cref="Retarget(Node3D)"/>
    [Export] public Node3D TrackingTarget { get => trackingTarget; protected set => trackingTarget = value; }
    /// <summary>
    /// This is the actual particle system that is used to generate the stars. 
    /// </summary>
    /// <remarks>
    /// The default starfield should be sufficiant in most cases but it can be changed. Note: best results are achieved with particles generated on the surface of a sphere with a radius of between 300m and 1000m. The particles should have no velocity, they should have a life of between 5 and 10 seconds, and should not use local coordinates. The default starfield is stored in a scene at "res://addons/particlestars/DefaultData/StarGenerator.tscn" please feel free to copy it, modify it or use it as the base for your own system.
    /// </remarks>
    [Export] public PackedScene TemplateStarfield { get => templateStarfield; set => templateStarfield = value; }
    /// <summary>
    /// Indicates weather or not the starfield generator is currently active.
    /// </summary>
    /// <see cref="SetEnabled(bool)"/>
    [Export] public bool IsEnabled { get => isEnabled; protected set => isEnabled = value; }
    /// <summary>
    /// 
    /// </summary>
    [Export] public float MinSpeed { get => minSpeed; set => minSpeed = value; }
    [Export] public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    [Export] public bool CalculateSpeedOnPhysicsUpdate { get => calculateSpeedOnPhysicsUpdate; set => calculateSpeedOnPhysicsUpdate = value; }
    private Vector3 previousLocation = new Vector3(0,0,0);
    private Vector3 currentVelocityVector = new Vector3(0,0,0);
    private float minSpeed = 1.0f;
    private float maxSpeed = 50.0f;
    private bool calculateSpeedOnPhysicsUpdate = false;
    private bool warmup = false;

    public Vector3 CurrentVelocityVector { get => currentVelocityVector; protected set => currentVelocityVector = value; }
    public float CurrentSpeed { get => currentVelocityVector.Length(); }
    public override void _EnterTree()
    {

        base._EnterTree();
    }

    public override void _ExitTree()
    {
        particles.CallDeferred("free");
        base._ExitTree();
    }

    public override void _Ready()
    {
        this.ProcessPhysicsPriority = 10;
        this.ProcessPriority = 10;
        particles = TemplateStarfield.Instantiate<GpuParticles3D>();
        this.AddChild(particles);
        particles.LocalCoords = false;
        particles.Preprocess = 5;
        if (trackingTarget == null)
        {
            particles.GlobalPosition = new Vector3(0, 0, 0);
        }
        else
        {
            particles.GlobalPosition = trackingTarget.GlobalPosition;
            previousLocation = trackingTarget.GlobalPosition;
        }
        
        particles.Restart();

        particles.SpeedScale = 0;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (!calculateSpeedOnPhysicsUpdate)
        {
            CalculateSpeed(delta);
        }
        if (isEnabled)
        {
            particles.SpeedScale = Mathf.Clamp(Mathf.Remap(this.CurrentSpeed, minSpeed, maxSpeed, 0, 1), 0, 1);
            particles.Position=trackingTarget.Position;
        }
        
        
        base._Process(delta);
    }
    public void SetEnabled(bool enabled)
    {
        if(enabled && !isEnabled)
        {
            previousLocation = trackingTarget.Position;
        }
        else
        {
            particles.SpeedScale = 0;
        }
        isEnabled = enabled;
    }

    public void Retarget(Node3D node)
    {
        trackingTarget = node;
        previousLocation = trackingTarget.Position;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (calculateSpeedOnPhysicsUpdate)
        {
            CalculateSpeed(delta);
        }
        base._PhysicsProcess(delta);
    }

    private void CalculateSpeed(double delta)
    {
        if (isEnabled)
        {
            currentVelocityVector = (previousLocation - trackingTarget.Position) / (float)delta;
            previousLocation = trackingTarget.Position;

        }
    }
    
}
