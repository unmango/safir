namespace Safir.Core.Helpers
{
    using System;
    using Popularimeter;

    internal class Popularity
    {
        public static IPopularimeter Get(TagLib.File song) {
            throw new NotImplementedException();
        }

        [Obsolete("Kept for development reference")]
        public static FrameType PopularimeterFrame<FrameType>(TagLib.File file) {
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
