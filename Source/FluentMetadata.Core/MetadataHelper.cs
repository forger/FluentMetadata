using System;
using System.Collections.Generic;
using System.Linq;
using FluentMetadata.Builder;

namespace FluentMetadata
{
    public static class MetadataHelper
    {
        static QueryFluentMetadata query = new QueryFluentMetadata();

        internal static void CopyMetadata(Type from, Type to)
        {
            var toBuilder = FluentMetadataBuilder.GetTypeBuilder(to);
            var nameBuilder = new PropertyNameMetadataBuilder(from);
            //copy property metadata
            foreach (var namedMetaData in nameBuilder.NamedMetaData)
            {
                if (to.GetProperty(namedMetaData.PropertyName) != null)
                {
                    toBuilder.MapProperty(to, namedMetaData.PropertyName, namedMetaData.Metadata);
                }
            }
            //copy type metadata

            query.GetMetadataFor(to).CopyMetaDataFrom(query.GetMetadataFor(from));
        }

        public static void CopyMappedMetadata(Type from, Type to, IEnumerable<MemberMap> memberMaps)
        {
            var toBuilder = FluentMetadataBuilder.GetTypeBuilder(to);
            var fromBuilder = new PropertyNameMetadataBuilder(from);
            //copy property metadata
            foreach (var fromMetaData in fromBuilder.NamedMetaData)
            {
                var memberMap = memberMaps.SingleOrDefault(mm => mm.SourceName == fromMetaData.PropertyName);
                if (memberMap != null)
                {
                    toBuilder.MapProperty(to, memberMap.DestinationName, fromMetaData.Metadata);
                }
            }
            //copy type metadata
            query.GetMetadataFor(to).CopyMetaDataFrom(query.GetMetadataFor(from));
        }
    }

    public class MemberMap
    {
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
    }
}