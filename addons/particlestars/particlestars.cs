#if TOOLS
using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class particlestars : EditorPlugin
{
    public override void _DisablePlugin()
    {
        base._DisablePlugin();
    }

    public override void _EnablePlugin()
    {
        base._EnablePlugin();
    }

    public override void _EnterTree() 
	{
       
        AddCustomType("StarGenerator", "Node3D", GD.Load<Script>("res://addons/particlestars/StarGeneratorNode.cs"), GD.Load<Texture2D>("res://addons/particlestars/StarIcon.png"));
        base._EnterTree();
	}

	public override void _ExitTree()
	{
        RemoveCustomType("StarGenerator");
		// Clean-up of the plugin goes here.
		base._ExitTree();
	}
}
#endif
