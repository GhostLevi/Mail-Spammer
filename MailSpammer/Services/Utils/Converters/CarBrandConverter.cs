using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Model;

namespace Services.Utils.Converters
{
    public class CarBrandConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            CarBrand brand;

            switch (text)
            {
                case "audi":
                    brand = CarBrand.Audi;
                    break;

                case "skoda":
                    brand = CarBrand.Skoda;
                    break;
                
                case "mercedes":
                    brand = CarBrand.Mercedes;
                    break;
                
                case "fiat":
                    brand = CarBrand.Fiat;
                    break;
                
                case "bmw":
                    brand = CarBrand.Bmw;
                    break;
                
                case "honda":
                    brand = CarBrand.Honda;
                    break;
                
                case "toyota":
                    brand = CarBrand.Toyota;
                    break;
                
                case "volkswagen":
                    brand = CarBrand.Volkswagen;
                    break;

                default:
                    brand = CarBrand.Volkswagen;
                    break;
            }

            return brand;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            string brand;

            switch (value)
            {
                case CarBrand.Audi:
                    brand = "audi";
                    break;

                case CarBrand.Skoda :
                    brand = "skoda";
                    break;
                
                case CarBrand.Mercedes:
                    brand = "mercedes";
                    break;
                
                case CarBrand.Fiat:
                    brand = "fiat";
                    break;
                
                case CarBrand.Bmw:
                    brand = "bmw";
                    break;
                
                case  CarBrand.Honda:
                    brand = "honda";
                    break;
                
                case CarBrand.Toyota :
                    brand = "toyota";
                    break;
                
                case CarBrand.Volkswagen:
                    brand = "volkswagen";
                    break;

                default:
                    brand = "volkswagen";
                    break;
            }

            return brand;
        }
    }
}