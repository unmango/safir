using MimeSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    internal static class FileExtension
    {
        public static bool Valid(Uri filePath)
        {            
            var mime = new Mime();
            var mimeType = mime.Lookup(filePath.AbsolutePath);
            return TagLib.FileTypes.AvailableTypes.ContainsKey(mimeType);
        }

        public static FrameType PopularimeterFrame<FrameType>(TagLib.File file)
        {
            var frameType = typeof(FrameType);
            var tagType = frameType.Namespace.Replace($"{nameof(TagLib)}.", "");

            if (!Enum.TryParse(tagType, out TagLib.TagTypes outParam))
                throw new ArgumentException("Invalid FrameType");

            var tagTypeVar = Type.GetType($"{frameType.Namespace}.Tag, taglib-sharp");

            var temp2 = file.TagTypes.CompareTo(outParam);

            if (file.TagTypes.CompareTo(outParam) > 1)
                throw new ArgumentException("Invalid FrameType");

            dynamic temp = Convert.ChangeType(file.Tag, tagTypeVar);

            dynamic popTypeVar = Type.GetType($"{frameType.Namespace}.PopularimeterFrame");

            popTypeVar.Get(temp, "WindowsUser", true);

            var tag = (TagLib.Id3v2.Tag)file.Tag;
            var meter = TagLib.Id3v2.PopularimeterFrame.Get(tag, "WindowsUser", true);

            throw new NotImplementedException();
        }
    }
}
