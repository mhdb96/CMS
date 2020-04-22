using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace CMSLibrary.Evaluation
{
    public class StudentDataModel : ObservableObject
    {
        private string _firstName;

        [Required(ErrorMessage = "Must not be empty.")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Must be between 2 and 12 characters.")]
        [NotHaveIlligalChar(ErrorMessage = "Must not have illigal charchters")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                ValidateProperty(value, "FirstName");
                OnPropertyChanged(ref _firstName, value);
            }
        }

        private string _lastName;

        [Required(ErrorMessage = "Must not be empty.")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Must be between 2 and 12 characters.")]
        [NotHaveIlligalChar(ErrorMessage = "Must not have illigal charchters")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                ValidateProperty(value, "LastName");
                OnPropertyChanged(ref _lastName, value);
            }
        }

        private int _regNo;

        [Required(ErrorMessage = "Must not be empty.")]
        [RegNoCount(ErrorMessage = "Must be between 9 numbers.")]
        public int RegNo
        {
            get { return _regNo; }
            set
            {
                ValidateProperty(value, "RegNo");
                OnPropertyChanged(ref _regNo, value);
            }
        }

        private string _group;

        [Required(ErrorMessage = "Must not be empty.")]
        [GroupCount(ErrorMessage = "Format error")]
        public string Group
        {
            get { return _group; }
            set
            {
                ValidateProperty(value, "Group");
                OnPropertyChanged(ref _group, value);
            }
        }


        public string AnswersList { get; set; }

        private void ValidateProperty<T>(T value, string name)
        {
            Validator.ValidateProperty(value, new ValidationContext(this, null, null)
            {
                MemberName = name
            });
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NotHaveIlligalChar : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = true;

            if (!string.IsNullOrEmpty(inputValue))
            {
                inputValue = inputValue.Replace(" ", "");
                inputValue = inputValue.Replace("-", "");
                isValid = inputValue.All(char.IsLetter);
            }

            return isValid;
        }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RegNoCount : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int inputValue = int.Parse(value.ToString());
            var isValid = true;

            if (Math.Floor(Math.Log10(inputValue) + 1) != 9)
            {
                isValid = false;
            }

            return isValid;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class GroupCount : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string inputValue = value.ToString();
            var isValid = true;

            if (inputValue.Length > 1)
            {
                isValid = false;
            }
            if (!(inputValue == "A" || inputValue == "B" || inputValue == "C" || inputValue == "D"))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
