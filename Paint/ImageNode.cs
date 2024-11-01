using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class ImageNode
    {
        public Bitmap Image { get; set; }
        public ImageNode Next { get; set; }

        public ImageNode(Bitmap image)
        {
            Image = image;
            Next = null;
        }
    }

    public class ImageLinkedList
    {
        public ImageNode Head { get; private set; }

        public void AddImage(Bitmap image)
        {
            var newNode = new ImageNode(image);
            if (Head == null)
                Head = newNode;
            else
            {
                var current = Head;
                while (current.Next != null)
                    current = current.Next;
                current.Next = newNode;
            }
        }

        public Bitmap GetLatestImage()
        {
            var current = Head;
            while (current?.Next != null)
                current = current.Next;
            return current?.Image;
        }
    }

}
