using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.SceneGraph
{
    class GameScene
    {
        public string SceneName { get; private set; }
        public List<GameObject2D> SceneObjects2D { get; private set; }
        public List<GameObject3D> SceneObjects3D { get; private set; }

        public GameScene(string name)
        {
            SceneName = name;
            SceneObjects2D = new List<GameObject2D>();
            SceneObjects3D = new List<GameObject3D>();
        }

        public void AddSceneObject(GameObject2D sceneObject)
        {
            if (!SceneObjects2D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                SceneObjects2D.Add(sceneObject);
            }
        }

        public void RemoveSceneObject(GameObject2D sceneObject)
        {
            if (SceneObjects2D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }

        public void AddSceneObject(GameObject3D sceneObject)
        {
            if (!SceneObjects3D.Contains(sceneObject))
            {
                sceneObject.Scene = this;
                SceneObjects3D.Add(sceneObject);
            }
        }

        public void RemoveSceneObject(GameObject3D sceneObject)
        {
            if (SceneObjects3D.Remove(sceneObject))
            {
                sceneObject.Scene = null;
            }
        }

        public virtual void Initialize()
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.Initialize());
            SceneObjects3D.ForEach(sceneObject => sceneObject.Initialize());
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.LoadContent(contentManager));
            SceneObjects3D.ForEach(sceneObject => sceneObject.LoadContent(contentManager));
        }

        public virtual void Update(RenderContext renderContext)
        {
            SceneObjects2D.ForEach(sceneObject => sceneObject.Update(renderContext));
            SceneObjects3D.ForEach(sceneObject => sceneObject.Update(renderContext));
        }

        public virtual void Draw2D(RenderContext renderContext, bool drawInFrontOf3D)
        {
            SceneObjects2D.ForEach(obj =>
            {
                if (obj.DrawInFrontOf3D == drawInFrontOf3D)
                    obj.Draw(renderContext);
            });
        }

        public virtual void Draw3D(RenderContext renderContext)
        {
            SceneObjects3D.ForEach(sceneObject => sceneObject.Draw(renderContext));
        }

        public virtual void Activated() { }
        public virtual void Deactivated() { }
    }
}
