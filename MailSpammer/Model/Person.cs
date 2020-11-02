namespace Model
{
    public class Person
    {
        public Person()
        {
            
        }
        public Person(string id, string firstName, string lastName, Gender gender, CarBrand carBrand, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            CarBrand = carBrand;
            Email = email;
        }
        
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public CarBrand CarBrand { get; set; }
        public string Email { get; set; }
    }
}