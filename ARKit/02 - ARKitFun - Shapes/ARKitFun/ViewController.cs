using System;
using ARKit;
using ARKitFun.Nodes;
using SceneKit;
using UIKit;

namespace ARKitFun
{
    public partial class ViewController : UIViewController
    {
        readonly ARSCNView sceneView;
        const float size = 0.1f;
        const float zPosition = -0.5f;

        public ViewController(IntPtr handle) : base(handle)
        {
            sceneView = new ARSCNView
            {
                AutoenablesDefaultLighting = true,
                ShowsStatistics = true
            };
            View.AddSubview(sceneView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            sceneView.Frame = View.Frame;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                LightEstimationEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            CubeNode cubeNode = new CubeNode(size, UIColor.Red);
            cubeNode.Position = new SCNVector3(0, 0, zPosition);

            SphereNode sphereNode = new SphereNode(size, UIColor.Yellow);
            sphereNode.Position = new SCNVector3(0.3f, 0, zPosition);

            TorusNode torusNode = new TorusNode(size / 2, size / 4, UIColor.Blue);
            torusNode.Position = new SCNVector3(0.6f, 0, zPosition);

            TubeNode tubeNode = new TubeNode(size / 2, size / 2, size * 2, UIColor.Orange);
            tubeNode.Position = new SCNVector3(0.9f, 0, zPosition);

            ConeNode coneNode = new ConeNode(0.0001f, size / 2, size * 2, UIColor.Green);
            coneNode.Position = new SCNVector3(0, -0.3f, zPosition);

            CylinderNode cylinderNode = new CylinderNode(size, size * 2, UIColor.Cyan);
            cylinderNode.Position = new SCNVector3(0.3f, -0.3f, zPosition);

            PyramidNode pyramidNode = new PyramidNode(size * 2, size * 2, size * 2, UIColor.Brown);
            pyramidNode.Position = new SCNVector3(0.6f, -0.3f, zPosition);

            PlaneNode planeNode = new PlaneNode(size, size, UIColor.Purple);
            planeNode.Position = new SCNVector3(0.9f, -0.3f, zPosition);

            TextNode textNode = new TextNode("Hello from ARKit", 0.01f, UIColor.Orange);
            textNode.Position = new SCNVector3(0, -0.85f, zPosition);

            sceneView.Scene.RootNode.AddChildNode(cubeNode);
            sceneView.Scene.RootNode.AddChildNode(sphereNode);
            sceneView.Scene.RootNode.AddChildNode(torusNode);
            sceneView.Scene.RootNode.AddChildNode(tubeNode);
            sceneView.Scene.RootNode.AddChildNode(coneNode);
            sceneView.Scene.RootNode.AddChildNode(cylinderNode);
            sceneView.Scene.RootNode.AddChildNode(pyramidNode);
            sceneView.Scene.RootNode.AddChildNode(planeNode);
            sceneView.Scene.RootNode.AddChildNode(textNode);
        }


        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            sceneView.Session.Pause();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}