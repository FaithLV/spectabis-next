using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SpectabisLib.Enums;
using SpectabisLib.Models;
using SpectabisService.Abstractions.Interfaces;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace SpectabisService.Services
{
    public class PCSX2DatabaseProvider
    {
        private readonly IHttpClient _httpClient;

        private static readonly Uri DatabaseUri = new Uri("https://forums.pcsx2.net/data/data.csv");
        private static IEnumerable<GameMetadata> _dbCache;

        public PCSX2DatabaseProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GameMetadata>> GetDatabase()
        {
            if(_dbCache != null)
            {
                return _dbCache;
            }

            var dbContent = await _httpClient.GetAsync(DatabaseUri).ConfigureAwait(false);
            var contentStream = await dbContent.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var parserOptions = new CsvParserOptions(true, '\t');
            var csvMapper = new DatabaseModelMapping();
            var parser = new CsvParser<GameMetadata>(parserOptions, csvMapper);

            _dbCache = parser.ReadFromStream(contentStream, Encoding.UTF8).Select(x => x.Result).ToList();
            return _dbCache;
        }
    }

    internal class DatabaseModelMapping : CsvMapping<GameMetadata>
    {
        public DatabaseModelMapping()
        {
            MapProperty(0, x => x.Serial);
            MapProperty(1, x => x.Compatibility, new EnumConverter<GameCompatibility>(true));
            MapProperty(5, x => x.Title);
            MapProperty(6, x => x.Region);
        }
    }
}