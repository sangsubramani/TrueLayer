using System.Collections.Generic;
using System.Linq;
using Pokemon.Persistency.Providers;

namespace Pokemon.Persistency.Implementation
{
    public class DataProvider : IDataProvider
    {
        public Pokemon GetData(string name)
        {
            var data = SetUpData();

            return  data.FirstOrDefault(x => x.Name == name);
        }

        private static List<Pokemon> SetUpData()
        {
            var pokemonList = new List<Pokemon>
            {
                new Pokemon
                {
                    Name = "meone",
                    Description = "It was created by a Sangeetha",
                    HabitantType = HabitantType.Unknown,
                    IsLegendary = true
                },
                new Pokemon
                {
                    Name = "metwo",
                    Description = "Master Obiwan has lost a planet",
                    HabitantType = HabitantType.Cave,
                    IsLegendary = true
                },
                new Pokemon
                {
                    Name = "methree",
                    Description = "You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die.",
                    HabitantType = HabitantType.Rare,
                    IsLegendary = false
                }
            };

            return pokemonList;
        }
    }
}
