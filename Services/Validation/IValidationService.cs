namespace HW_4.Services.Validation
{
    public interface IValidationService
    {

        bool IsFullnameValid(string text);
        bool IsPhoneValid(string text);
        bool IsEmailValid(string text);
        bool IsLoginValid(string text);
        bool IsPasswordValid(string text);
        public bool IsAvatarValid(string fileName);

    }
}
