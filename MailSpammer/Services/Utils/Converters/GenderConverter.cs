using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Model;

namespace Services.Utils.Converters
{
    public class GenderConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            Gender gender;

            switch (text)
            {
                case "male":
                    gender = Gender.Male;
                    break;

                case "female":
                    gender = Gender.Female;
                    break;

                default:
                    gender = Gender.Male;
                    break;
            }

            return gender;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            string gender;

            switch (value)
            {
                case Gender.Male:
                    gender = "male";
                    break;

                case Gender.Female:
                    gender = "female";
                    break;

                default:
                    gender = "male";
                    break;
            }

            return gender;
        }
    }
}