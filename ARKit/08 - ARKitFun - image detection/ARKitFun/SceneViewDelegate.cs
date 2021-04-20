using System;
using ARKit;
using ARKitFun.Nodes;
using SceneKit;
using UIKit;

namespace ARKitFun
{
    public class SceneViewDelegate : ARSCNViewDelegate
    {
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARImageAnchor imageAnchor)
            {
                ARReferenceImage image = imageAnchor.ReferenceImage;
                nfloat width = image.PhysicalSize.Width;
                nfloat height = image.PhysicalSize.Height;

                PlaneNode planeNode = new PlaneNode(width, height, new SCNVector3(0, 0, 0), UIColor.Red);
                float angle = (float)(-Math.PI / 2);
                planeNode.EulerAngles = new SCNVector3(angle, 0, 0);
                node.AddChildNode(planeNode);
            }
        }
    }
}
