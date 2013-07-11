using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Source.Engine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source.Engine.SceneGraph
{
    class GameObject2D
    {
        protected Matrix WorldMatrix;

        public Vector2 LocalPosition { get; set; }
        public Vector2 WorldPosition { get; private set; }
        public Vector2 LocalScale { get; set; }
        public Vector2 WorldScale { get; private set; }
        public float LocalRotation { get; set; }
        public float WorldRotation { get; private set; }

        public Vector2 PivotPoint { get; set; }
        public bool DrawInFrontOf3D { get; set; }
        public bool CanDraw { get; set; }

        public GameObject2D Parent { get; set; }
        public List<GameObject2D> Children { get; private set; }
        public GameObject2D this[int childIndex]
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


        private Rectangle? _relativeBoundingRect;
        public Rectangle? BoundingRect { get; protected set; }
        private bool _drawBoundingRect;
        public bool DrawBoundingRect
        {
            get
            {
                if (Parent != null)
                    return Parent.DrawBoundingRect || _drawBoundingRect;

                return _drawBoundingRect;
            }
            set
            {
                _drawBoundingRect = value;
            }
        }

        public GameObject2D()
        {
            LocalScale = WorldScale = Vector2.One;
            Children = new List<GameObject2D>();
            CanDraw = true;
            DrawInFrontOf3D = true;
        }

        public void AddChild(GameObject2D child)
        {
            if (!Children.Contains(child))
            {
                Children.Add(child);
                child.Scene = Scene;
                child.Parent = this;
            }
        }

        public void RemoveChild(GameObject2D child)
        {
            if (Children.Remove(child))
            {
                child.Scene = null;
                child.Parent = null;
            }
        }

        public void Rotate(float rotation)
        {
            LocalRotation = rotation;
        }

        public void Translate(float posX, float posY)
        {
            Translate(new Vector2(posX, posY));
        }

        public void Translate(Vector2 position)
        {
            LocalPosition = position;
        }

        public void Scale(float scaleX, float scaleY)
        {
            Scale(new Vector2(scaleX, scaleY));
        }

        public void Scale(Vector2 scale)
        {
            LocalScale = scale;
        }

        public void CreateBoundingRect(int width, int height, Vector2 offset)
        {
            _relativeBoundingRect = new Rectangle(0, 0, width + (int)offset.X, height + (int)offset.Y);
            BoundingRect = _relativeBoundingRect;
        }

        public void CreateBoundingRect(int width, int height)
        {
            CreateBoundingRect(width, height, Vector2.Zero);
        }

        public bool HitTest(GameObject2D gameObj)
        {
            if (!gameObj.BoundingRect.HasValue) return false;
            if (BoundingRect.HasValue && BoundingRect.Value.Intersects(gameObj.BoundingRect.Value)) return true;

            return Children.FirstOrDefault(child => child.HitTest(gameObj)) != null;
        }

        public bool HitTest(Vector2 position, bool includeChildren)
        {
            if (BoundingRect.HasValue && BoundingRect.Value.Contains((int)position.X, (int)position.Y)) return true;

            if (includeChildren)
                return Children.FirstOrDefault(child => HitTest(position, includeChildren)) != null;

            return false;
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
            WorldMatrix =
                Matrix.CreateTranslation(new Vector3(-PivotPoint, 0)) *
                Matrix.CreateScale(new Vector3(LocalScale, 1)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(LocalRotation)) *
                Matrix.CreateTranslation(new Vector3(LocalPosition, 0));

            if (Parent != null)
            {
                WorldMatrix = Matrix.Multiply(WorldMatrix, Matrix.CreateTranslation(new Vector3(Parent.PivotPoint, 0)));
                WorldMatrix = Matrix.Multiply(WorldMatrix, Parent.WorldMatrix);
            }

            Vector3 pos, scale;
            Quaternion rot;
            if (!WorldMatrix.Decompose(out scale, out rot, out pos))
                System.Diagnostics.Debug.WriteLine("Object2D Decompose World Matrix FAILED!");

            var direction = Vector2.Transform(Vector2.UnitX, rot);
            WorldRotation = (float)Math.Atan2(direction.Y, direction.X);
            WorldRotation = float.IsNaN(WorldRotation) ? 0 : MathHelper.ToDegrees(WorldRotation);

            WorldPosition = new Vector2(pos.X, pos.Y);
            WorldScale = new Vector2(scale.X, scale.Y);

            foreach (var chilld in Children) chilld.Update(renderContext);

            if (_relativeBoundingRect.HasValue)
                BoundingRect = _relativeBoundingRect.Value.Update(WorldMatrix);
        }

        public virtual void Draw(RenderContext renderContext)
        {
            if (CanDraw)
                foreach (var chilld in Children) chilld.Draw(renderContext);

            if (CanDraw && DrawBoundingRect && BoundingRect.HasValue)
                BoundingRect.Value.Draw(renderContext, Color.Red);
        }
    }
}