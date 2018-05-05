namespace Climbing.Web.Model
{
    using System;

    public class Person : BaseEntity
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
    }
}