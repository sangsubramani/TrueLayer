using System;

namespace Pokemon.Persistency
{
    public class Pokemon
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public HabitantType HabitantType { get; set; }

        public bool IsLegendary { get; set; }
    }
}
