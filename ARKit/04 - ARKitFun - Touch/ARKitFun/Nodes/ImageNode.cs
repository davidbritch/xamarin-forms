using SceneKit;
using UIKit;
using Foundation;

namespace ARKitFun.Nodes
{
    public class ImageNode : SCNNode
    {
        public ImageNode(string image, float width, float height)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(image, width, height)
            };
            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(string resource, float width, float height)
        {
            UIImage image;

            if (resource.StartsWith("http"))
                image = FromUrl(resource);
            else
                image = UIImage.FromFile(resource);

            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = image;
            material.DoubleSided = true;

            SCNPlane geometry = SCNPlane.Create(width, height);
            geometry.Materials = new[] { material };
            return geometry;
        }

        UIImage FromUrl(string url)
        {
            using (NSUrl nsUrl = new NSUrl(url))
            using (NSData imageData = NSData.FromUrl(nsUrl))
                return UIImage.LoadFromData(imageData);
        }
    }
}
