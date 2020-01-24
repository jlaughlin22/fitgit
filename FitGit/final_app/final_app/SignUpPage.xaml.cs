using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using final_app_Firebase;
using System.Collections.Generic;

namespace final_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage   // Defines signup page behavior
    {
        FirebaseHelper helper = new FirebaseHelper();//new query helper
        string Username = "";//username holder
        string Password = "";//password holder
        string DateOfBirth = "";//date of birth holder
        double Weight = 0.0;//weight holder

        public SignUpPage() // Constructor
        {
            InitializeComponent();
        }

        private async void enter_clicked(object sender, EventArgs e)    // Defines functionality for button click
        {
            Username = uname_line.Text;//set username
            Password = pw_again.Text;//set password
            DateOfBirth = date_pick.Date.ToShortDateString().ToString();//set date of birth
            bool Exists = await helper.UserExists(Username);//check if user exists
            if (!Exists)//if does not already exists
            {
                if (double.TryParse(weight_line.Text, out double weightVal))//check if valid weight
                {
                    Weight = weightVal;//set weight
                    if (pw_line.Text == pw_again.Text)//check if passwords are the same
                    {
                        if (pw_line.Text.Length > 7)//check if passwords are longer than 7 chars
                        {
                            await helper.AddUser(Password, Username, DateOfBirth, Weight);// add user to databse
                            await DisplayAlert("Success", "You have been successfully added.", "Ok");//display user added
                            uname_line.Text = string.Empty;//empty val
                            pw_line.Text = string.Empty;//empty val
                            pw_again.Text = string.Empty;//empty val
                            weight_line.Text = string.Empty;//empty val
                            DOB_label.Text = string.Empty;//empty val
                            await Navigation.PushAsync(new SearchBrowsePage(await helper.getUser(Username)));//set new page
                        }
                        else
                        {
                            await DisplayAlert("Failed", "Password must be at least 8 characters.", "Ok");// passwords do not meett criteria
                            uname_line.Text = string.Empty;//empty val
                            pw_line.Text = string.Empty;
                            pw_again.Text = string.Empty;
                            weight_line.Text = string.Empty;
                            DOB_label.Text = string.Empty;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Password Invalid", "The passwords entered do not match each other and or are less than 8 characters.", "Ok");//feilds invalid
                        pw_line.Text = string.Empty;
                        pw_again.Text = string.Empty;
                    }
                }
                else
                {
                    await DisplayAlert("Must Correctly Fill All Fields", "You have inputed your weight incorrectly", "Ok");//fields not input correctly
                    pw_line.Text = string.Empty;
                    pw_again.Text = string.Empty;
                    weight_line.Text = string.Empty;                
                }
            }
            else
            {
                uname_line.Text = string.Empty;
                pw_line.Text = string.Empty;
                pw_again.Text = string.Empty;
                weight_line.Text = string.Empty;
                await DisplayAlert("User Name Already Exists", "The username entered already exists.", "Ok");//user already exists
            }
        }
    }
}