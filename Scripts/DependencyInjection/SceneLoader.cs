
using System;
using Godot;

namespace DiveRollPlatformer.DependencyInjection
{
    public interface ISceneLoader
    {
        /// <summary>
        /// Loads a scene and returns its root node.  Dependency Injection is
        /// automagically applied to all scripts in that scene
        /// </summary>
        TNode Instantiate<TNode>(string path) where TNode : Node;
    }

    public class SceneLoader : ISceneLoader
    {
        private readonly IServiceProvider _services;

        public SceneLoader(IServiceProvider services)
        {
            _services = services;
        }

        public TNode Instantiate<TNode>(string path) where TNode : Node
        {
            var node = (TNode)GD.Load<PackedScene>(path).Instance();
            Utils.InitializeNode(node, _services);

            return node;
        }
    }
}
