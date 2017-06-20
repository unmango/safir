using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Safir.Manager.Query.Song
{
    public static partial class Query
    {
        public static IEnumerable<SongSummary> Execute(
            this IQueryHandler<Song.Summary, IEnumerable<SongSummary>> handler,
            Expression<Func<Data.Entities.Song, bool>> songType) {
            return handler.Execute(new Song.Summary(songType));
        }

        public static partial class Song
        {
            public class Summary : IQuery<IEnumerable<SongSummary>>
            {
                internal Summary(Expression<Func<Data.Entities.Song, bool>> songType) {
                    SongType = songType;
                }

                public Expression<Func<Data.Entities.Song, bool>> SongType { get; set; }
            }

            internal static partial class Handlers
            {
                internal sealed class GetSongSummaryHandler
                    : IQueryHandler<GetSongSummary, IEnumerable<SongSummary>>
                {
                    public IEnumerable<SongSummary> Execute(GetSongSummary query) {
                        return Enumerable.Empty<SongSummary>();
                    }
                }
            }
        }
    }
}
