using System.Windows.Input;

namespace save_our_souls.ViewModels
{
    public class SignUpVM
    {
        private readonly Services.UserAccountDatabase _userAccountDatabase;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; }

        public ICommand ShowPhotoOptionsCommand { get; }

        public SignUpVM(Services.UserAccountDatabase userAccountDatabase)
        {
            _userAccountDatabase = userAccountDatabase;
            ShowPhotoOptionsCommand = new Command(async () => await ShowPhotoOptionsAsync());
        }

        public async Task<bool> SignUpUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;

            var userAccount = new Models.UserAccountModel
            {
                Name = Name,
                Username = Username,
                Password = Password,
                Photo = Photo
            };

            await _userAccountDatabase.AddUserAccountAsync(userAccount);

            return true;
        }

        private async Task ShowPhotoOptionsAsync()
        {
            if (Shell.Current is null)
                return;

            var selectedOption = await Shell.Current.DisplayActionSheetAsync(
                "Profile photo",
                "Cancel",
                null,
                "Choose a photo",
                "Take a photo");

            if (selectedOption == "Choose a photo")
                await PickPhotoAsync();
            else if (selectedOption == "Take a photo")
                await TakePhotoAsync();
        }

        private async Task PickPhotoAsync()
        {
            var photo = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
            {
                Title = "Please select a photo"
            });
            if (photo != null)
            {
                Photo = photo.FirstOrDefault()?.FullPath;
            }
        }

        private async Task TakePhotoAsync()
        {
            if (!MediaPicker.Default.IsCaptureSupported)
                return;

            var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Take a profile photo"
            });

            if (photo != null)
            {
                Photo = photo.FullPath;
            }
        }
    }
}
