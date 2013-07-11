using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Source.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Engine.SceneGraph
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

        private BoundingBox? _relativeBoundingBox;
        public BoundingBox? BoundingBox { get; protected set; }
        private bool _drawBoundingBox;
        public bool DrawBoundingBox
        {
            get
            {
                if (Parent != null)
                    return Parent.DrawBoundingBox || _drawBoundingBox;

                return _drawBoundingBox;
            }
            set
            {
                _drawBoundingBox = value;
            }
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
                child.Scene = Scene;
                child.Parent = this;
                Children.Add(child);
            }
        }

        public void RemoveChild(GameObject3D child)
        {
            if (Children.Remove(child))
            {
                child.Scene = null;
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

        public void CreateBoundingBox(float width, float height, float depth, Vector3 offset)
        {
            var max = new Vector3(width / 2.0f, height / 2.0f, depth / 2.0f);
            var min = -max;

            _relativeBoundingBox = new BoundingBox(min + offset, max + offset);
            BoundingBox = _relativeBoundingBox;
        }

        public void CreateBoundingBox(float width, float height, float depth)
        {
            CreateBoundingBox(width, height, depth, Vector3.Zero);
        }

        public bool HitTest(GameObject3D gameObj)
        {
            //Check Other_Object Itself
            if (gameObj.BoundingBox.HasValue && BoundingBox.HasValue)
            {
                if (BoundingBox.Value.Intersects(gameObj.BoundingBox.Value)) return true;
            }

            //Check this_Object and other_Object's Children
            if (gameObj.Children.FirstOrDefault(HitTest) != null) return true;

            //Check this_Object's children with other_Object
            return Children.FirstOrDefault(child => child.HitTest(gameObj)) != null;
        }

        public virtual void Initialize()
        {
            foreach (var chilld in Children) chilld.Initialize();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (var chilld in Children) chilld.LoadContent(contentManager);
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

            foreach (var chilld in Children) chilld.Update(renderContext);

            if (_relativeBoundingBox.HasValue)
                BoundingBox = _relativeBoundingBox.Value.Update(WorldMatrix);
        }

        public virtual void Draw(RenderContext renderContext)
        {
            if (CanDraw)
                foreach (var chilld in Children) chilld.Draw(renderContext);

            if (CanDraw && DrawBoundingBox && BoundingBox.HasValue)
                BoundingBox.Value.Draw(renderContext, Color.Red);
        }
    }
}