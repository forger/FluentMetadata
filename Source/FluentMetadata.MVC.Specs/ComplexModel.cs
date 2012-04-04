using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FluentMetadata.MVC.Specs
{
    public class ComplexModelMetadata : ClassMetadata<ComplexModel>
    {
        public ComplexModelMetadata()
        {
            Class
                .Display.Name("Komplex")
                .AssertThat(
                    cm => !"Robert'); DROP TABLE Students; --".Equals( //http://xkcd.com/327/ :)
                        cm.FirstName + cm.LastName,
                        StringComparison.InvariantCultureIgnoreCase),
                    "Gotcha, little Bobby Tables! You'll never be '{0}'!");
            Property(e => e.Id)
                .Should.HiddenInput()
                .Is.ReadOnly()
                .Should.Not.ShowInDisplay()
                .Should.Not.ShowInEditor();
            Property(e => e.FirstName)
                .Display.Name("Vorname")
                .Is.Not.ConvertEmptyStringToNull()
                .Is.Required();
            Property(e => e.LastName)
                .Display.NullText("No lastname set");
            Property(e => e.Age)
                .As.Custom("Years")
                .UIHint("Spinner");
            Property(e => e.Amount)
                .Display.Format("{0:c}")
                .Editor.Format("{0:c}");
            Property(e => e.Sex)
                .AssertThat(
                    sex => sex != 'm',
                    "'{0}' cannot be male since this is a ComplexModel.");
        }
    }

    [DisplayName("Komplex")]
    public class ComplexModel
    {
        [HiddenInput(DisplayValue = false)]
        [ReadOnly(true)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [DisplayName("Vorname")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string FirstName { get; set; }
        [DisplayFormat(NullDisplayText = "No lastname set")]
        public string LastName { get; set; }
        [DataType("Years")]
        [UIHint("Spinner")]
        public int Age { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
        public char Sex { get; set; }
    }
}