using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using final_app_Firebase;

namespace final_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage    // Defines login page behavior
    {
        FirebaseHelper helper = new FirebaseHelper();// new query helper
        string UserName = "";//username holder
        string Password = "";//password holder
        public LoginPage()  // Constructor
        {
            InitializeComponent();
        }
        public async void btn_clicked(object sender, EventArgs e)   // Defines behavior on button click
        {
            UserName = uname_line.Text;// get username
            Password = pw_line.Text;//get password
            if((await helper.isCorrectpWUn(UserName, Password)))//if correct passord and username
            {
                
                await Navigation.PushAsync(new SearchBrowsePage(await helper.getUser(UserName)));//set current user and move to exercise list
                await DisplayAlert("Welcome Back", "Welcome Back to FitGit " + UserName + "!\nAll of your previous data has been reloaded.", "Continue");//welcomes user back to app
            }
            else
            {
                uname_line.Text = string.Empty;//empties field
                pw_line.Text = string.Empty;//empties field
                await DisplayAlert("Incorrect Username or Password", "You have entered either an invalid username or invalid password for the username.", "Ok");//displays incorrect password or username has been entered
            }
           
        }
    }
}