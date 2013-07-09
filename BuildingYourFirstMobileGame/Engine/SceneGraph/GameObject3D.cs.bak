using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.SceneGraph
{
    abstract class GameObject3D
    {
        protected Matrix WorldMatrix;

        public Vector3 LocalPosition { get; set; }
        public Vector3 WorldPosition { get; private set; }
        public Quaternion LocalRotation { get; set; }
        public Quaternion WorldRotation { get; private set; }
        public Vector3 LocalScale { get; set; }
        public Vector3 WorldScale { get; private set; }
        public bool CanDraw { get; set; }

        public GameObject3D Parent { get; private set; }
        public List<GameObject3D> Children { get; private set; }
        public GameObject3D this[int childIndex]
        {
            get
            {
                if (childIndex >= 0 && childIndex < Children.Count)
                    return Children[childIndex];

                return null;

            }
        }

        private GameScene _scene;
        public GameScene Scene
        {
            get
            {
                if (_scene != null) return _scene;
                if (Parent != null) return Parent.Scene;
                return null;
            }

            set { _scene = value; }
        }

        protected GameObject3D()
        {
            LocalScale = WorldScale = Vector3.One;
            Children = new List<GameObject3D>();
            CanDraw = true;
        }

        public void AddChild(GameObject3D child)
        {
            if (!Children.Contains(child))
            {
                child.Parent = this;
                Children.Add(child);
            }
        }

        public void RemoveChild(GameObject3D child)
        {
            if (Children.Remove(child))
            {
                child.Parent = null;
            }
        }

        public void Translate(Vector3 translation)
        {
            LocalPosition = translation;
        }

        public void Translate(float x, float y, float z)
        {
            LocalPosition = new Vector3(x, y, z);
        }

        public void Scale(Vector3 scale)
        {
            LocalScale = scale;
        }

        public void Scale(float x, float y, float z)
        {
            LocalScale = new Vector3(x, y, z);
        }

        public void Rotate(Quaternion rotation)
        {
            LocalRotation = rotation;
        }

        public void Rotate(float pitch, float yaw, float roll)
        {
            LocalRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw), MathHelper.ToRadians(pitch), MathHelper.ToRadians(roll));
        }

        public virtual void Initialize()
        {
            Children.ForEach(child => child.Initialize());
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            Children.ForEach(child => child.LoadContent(contentManager));
        }

        public virtual void Update(RenderContext renderContext)
        {
            WorldMatrix = Matrix.CreateFromQuaternion(LocalRotation) *
                          Matrix.CreateScale(LocalScale) *
                          Matrix.CreateTranslation(LocalPosition);


            if (Parent != null)
            {
                WorldMatrix = Matrix.Multiply(WorldMatrix, Parent.WorldMatrix);

                Vector3 scale, position;
                Quaternion rotation;

                if (!WorldMatrix.Decompose(out scale, out rotation, out position))
                {
                    System.Diagnostics.Debug.WriteLine("Object3D Decompose World Matrix FAILED!");
                }

                WorldPosition = position;
                WorldScale = scale;
                WorldRotation = rotation;
            }
            else
            {
                WorldPosition = LocalPosition;
                WorldScale = LocalScale;
                WorldRotation = LocalRotation;
            }

            Children.ForEach(child => child.Update(renderContext));

        }

        public virtual void Draw(RenderContext renderContext)
        {
            if (CanDraw)
                Children.ForEach(child => { if (child.CanDraw) child.Draw(renderContext); });
        }
    }
}
