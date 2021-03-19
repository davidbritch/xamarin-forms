﻿using System;
using System.Linq;
using ARKit;
using ARKitFun.Extensions;
using ARKitFun.Nodes;
using CoreGraphics;
using SceneKit;
using UIKit;

namespace ARKitFun
{
    public partial class ViewController : UIViewController
    {
        readonly ARSCNView sceneView;
        const float size = 0.1f;
        const float zPosition = -0.5f;
        bool isAnimating;
        float zAngle;

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

            SphereNode sphereNode = new SphereNode(size, "world-map.jpg");
            sphereNode.Position = new SCNVector3(0, 0, zPosition);

            sceneView.Scene.RootNode.AddChildNode(sphereNode);

            UITapGestureRecognizer tapGestureRecognizer = new UITapGestureRecognizer(HandleTapGesture);
            sceneView.AddGestureRecognizer(tapGestureRecognizer);

            UIPinchGestureRecognizer pinchGestureRecognizer = new UIPinchGestureRecognizer(HandlePinchGesture);
            sceneView.AddGestureRecognizer(pinchGestureRecognizer);

            UIRotationGestureRecognizer rotationGestureRecognizer = new UIRotationGestureRecognizer(HandleRotateGesture);
            sceneView.AddGestureRecognizer(rotationGestureRecognizer);
        }

        void HandleTapGesture(UITapGestureRecognizer sender)
        {
            SCNView areaPanned = sender.View as SCNView;
            CGPoint point = sender.LocationInView(areaPanned);
            SCNHitTestResult[] hitResults = areaPanned.HitTest(point, new SCNHitTestOptions());
            SCNHitTestResult hit = hitResults.FirstOrDefault();

            if (hit != null)
            {
                SCNNode node = hit.Node;
                if (node != null)
                {
                    if (!isAnimating)
                    {
                        node.AddRotationAction(SCNActionTimingMode.Linear, 10, true);
                        isAnimating = true;
                    }
                    else
                    {
                        node.RemoveAction("rotation");
                        isAnimating = false;
                    }
                }                    
            }
        }

        void HandlePinchGesture(UIPinchGestureRecognizer sender)
        {
            SCNView areaPanned = sender.View as SCNView;
            CGPoint point = sender.LocationInView(areaPanned);
            SCNHitTestResult[] hitResults = areaPanned.HitTest(point, new SCNHitTestOptions());
            SCNHitTestResult hit = hitResults.FirstOrDefault();

            if (hit != null)
            {
                SCNNode node = hit.Node;

                float scaleX = (float)sender.Scale * node.Scale.X;
                float scaleY = (float)sender.Scale * node.Scale.Y;
                float scaleZ = (float)sender.Scale * node.Scale.Z;

                node.Scale = new SCNVector3(scaleX, scaleY, scaleZ);
                sender.Scale = 1; // Reset the node scale value
            }
        }

        void HandleRotateGesture(UIRotationGestureRecognizer sender)
        {
            SCNView areaPanned = sender.View as SCNView;
            CGPoint point = sender.LocationInView(areaPanned);
            SCNHitTestResult[] hitResults = areaPanned.HitTest(point, new SCNHitTestOptions());
            SCNHitTestResult hit = hitResults.FirstOrDefault();

            if (hit != null)
            {
                SCNNode node = hit.Node;
                zAngle += (float)(-sender.Rotation);
                node.EulerAngles = new SCNVector3(node.EulerAngles.X, node.EulerAngles.Y, zAngle);
            }
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