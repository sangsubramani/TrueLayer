using Pokemon.Api.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Pokemon.Api.Implementation;
using Pokemon.Api.Response;
using Pokemon.Persistency;
using Pokemon.Persistency.Providers;
using Shouldly;
using Xunit;


namespace Pokemon.Tests
{
    public class PokemonApiTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ILogger<PokemonService>>_logger;
        private readonly Mock<IDataProvider> _dataProvider;
        private readonly Mock<IMapper> _mapper;

        private readonly IPokemonService _subject;
        public PokemonApiTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _dataProvider = _mockRepository.Create<IDataProvider>();
            _mapper = _mockRepository.Create<IMapper>();
            _logger = new Mock<ILogger<PokemonService>>();

            _subject = new PokemonService(_dataProvider.Object,
                _logger.Object, _mapper.Object);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Return_Null_When_Endpoint_Is_Incorrect(string search)
        {
            var result = _subject.GetDetails(search);
            result.ShouldBeOfType<ErrorResponse>();
        }

        [Fact]
        public async Task Should_Return_Expected_Data_When_Data_Exits()
        {
            var search = "metwo";
            var expectedData = await SetUpData();

            _dataProvider.Setup(x => x.GetData(search))
                .Returns(expectedData);

            var expectedResponse = new PokemonResponse
            {
                Name = "metwo",
                Description = "It was created by a scientists after years of...",
                HabitantType = HabitantType.Rare.ToString(),
                IsLegendary = true
            };

            _mapper.Setup(m => m.Map<PokemonResponse>(expectedData)).Returns(expectedResponse);

            var result =  _subject.GetDetails(search) as PokemonResponse;
            
            result.ShouldNotBeNull();
            result.ShouldBeOfType<PokemonResponse>();

            result.Name.ShouldBe(expectedResponse.Name);
            result.HabitantType.ShouldBe(expectedResponse.HabitantType);
        }

        [Fact]
        public void  Should_Return_Null_When_Data_Not_Exists()
        {
            var search = "DFGRTYR";
            PokemonResponse expectedData = null;

            _dataProvider.Setup(x => x.GetData(search))
                .Returns((Persistency.Pokemon)null);

            var result = _subject.GetDetails(search);
            result.ShouldBeNull();
        }

        [Fact]
        public async Task Should_Return_Error_Message_When_No_Data_Found()
        {
            var search = "mefour";
            var expectedData = await SetUpData();

            _dataProvider.Setup(x => x.GetData(search))
                .Returns(expectedData);

            var expectedResponse = new PokemonResponse
            {
                Name = "metwo",
                Description = "Lost a planet,  master obiwan has",
                HabitantType = HabitantType.Rare.ToString(),
                IsLegendary = true
            };

            _mapper.Setup(m => m.Map<PokemonResponse>(expectedData)).Returns(expectedResponse);

            var result = await _subject.GetTranslatedDetails(search);
            result.ShouldNotBeNull();
            //  result.ShouldBeOfType<ErrorResponse>();
            //  result.ShouldBeOfType<ApiResponse>();
        }

       
        [Fact]
        public async Task Should_Return_Error_Message_When_Url_Not_Correct()
        {
            var search = "";

            var expectedResponse = new ErrorResponse();
            
            var result = await _subject.GetTranslatedDetails(search);
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ErrorResponse>();
        }


        //Test can be extended to mock all the response form the client Api call.

        [Fact]
        public async Task Should_Return_Translated_Data_Description_When_Data_Is_Valid()
        {
            var search = "";
            var expectedData = await SetUpData();

            _dataProvider.Setup(x => x.GetData(search))
                .Returns(expectedData);

            var expectedResponse = new PokemonResponse
            {
                Name = "metwo",
                Description = "Lost a planet,  master obiwan has",
                HabitantType = HabitantType.Rare.ToString(),
                IsLegendary = true
            };

            _mapper.Setup(m => m.Map<PokemonResponse>(expectedData)).Returns(expectedResponse);

            var result = await _subject.GetTranslatedDetails(search);
            result.ShouldNotBeNull();
            //  result.ShouldBeOfType<ErrorResponse>();
            //  result.ShouldBeOfType<ApiResponse>();
        }

        private async Task<Persistency.Pokemon> SetUpData()
        {
            return new Persistency.Pokemon
            {
                Name = "metwo",
                Description = "It was created by a scientists after years of...",
                HabitantType = HabitantType.Rare,
                IsLegendary = true
            };
        }
    }
}
