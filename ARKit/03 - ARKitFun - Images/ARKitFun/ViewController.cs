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

            ImageNode imageNode = new ImageNode("Xamagon.png", size, size);
            imageNode.Position = new SCNVector3(0, 0, zPosition / 2);

            sceneView.Scene.RootNode.AddChildNode(imageNode);
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