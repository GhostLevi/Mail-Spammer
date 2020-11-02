using CsvHelper.Configuration;
using Model;
using Services.Utils.Converters;

namespace Services.Utils
{
    public sealed class CsvPersonMapper: ClassMap<Person>
    {
        public CsvPersonMapper()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.FirstName).Index(1);
            Map(m => m.LastName).Index(2);
            Map(m => m.Gender).Index(3).TypeConverter<GenderConverter>();
            Map(m => m.CarBrand).Index(4).TypeConverter<CarBrandConverter>();
            Map(m => m.Email).Index(5);
        }
    }
}