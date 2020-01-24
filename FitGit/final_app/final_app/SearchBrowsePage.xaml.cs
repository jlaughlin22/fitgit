using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using final_app.Models;
using final_app_Firebase;

namespace final_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchBrowsePage : ContentPage // Contains exercise info and displays it
    {
        FirebaseHelper helper = new FirebaseHelper();// new query helper
        User CurrentUser = new User();//current user of app
        List<Exercise> holder = new List<Exercise>();//holder list
        public SearchBrowsePage(User CurUserName)
        {
            helperFunc(CurUserName);//displays all exercises
            InitializeComponent();
        }

        public async void helperFunc(User user) // Allows you to call thread inside constructor
        {
            //displays all exercises
            CurrentUser = await helper.getUser(user);
            if (CurrentUser.ex_hist.Count > 0)
            {
                holder = CurrentUser.ex_hist;
            }
            for (int i = 0; i < CurrentUser.ex_list.Count; i++)
            {
                //displays one exercise for each loop
                ContentView myContentView = new ContentView();
                StackLayout StackLayoutTop = new StackLayout();
                Label topLabel = new Label();

                StackLayout StackLayoutIntensity = new StackLayout();
                Label IntLabel1 = new Label();
                Label IntLabel2 = new Label();

                StackLayout StackLayoutCals = new StackLayout();
                Label CalLabel1 = new Label();
                Label CalLabel2 = new Label();

                StackLayout StackLayoutIntMins = new StackLayout();
                Label IntMinLabel1 = new Label();
                Label IntMinLabel2 = new Label();

                Button addEx = new Button();

                myContentView.BackgroundColor = Color.LightGray;
                StackLayoutIntensity.Orientation = StackOrientation.Horizontal;
                StackLayoutCals.Orientation = StackOrientation.Horizontal;
                StackLayoutIntMins.Orientation = StackOrientation.Horizontal;

                topLabel.Text = CurrentUser.ex_list[i].Name;
                topLabel.FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
                topLabel.FontAttributes = FontAttributes.Bold;
                topLabel.HorizontalOptions = LayoutOptions.Center;

                IntLabel1.Text = "Intensity level: ";
                IntLabel1.Padding = new Thickness(5, 0, 0, 0);
                IntLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                IntLabel2.Text = CurrentUser.ex_list[i].Intensity;
                IntLabel2.Padding = new Thickness(-5, 0, 0, 0);
                IntLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                CalLabel1.Text = "Calories burned: ";
                CalLabel1.Padding = new Thickness(5, 0, 0, 0);
                CalLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                CalLabel2.Text = CurrentUser.ex_list[i].Calories(CurrentUser.ex_list[i].MET, CurrentUser.Weight).ToString("0.00");
                CalLabel2.Padding = new Thickness(-5, 0, 0, 0);
                CalLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                IntMinLabel1.Text = "Intensity minutes: ";
                IntMinLabel1.Padding = new Thickness(5, 0, 0, 0);
                IntMinLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                IntMinLabel2.Text = CurrentUser.ex_list[i].IntMins(0).ToString("0.00");
                IntMinLabel2.Padding = new Thickness(-5, 0, 0, 0);
                IntMinLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                addEx.Text = "Add to exercise list";
                addEx.TextColor = Color.Linen;
                addEx.Clicked += AddCalories;
                addEx.BackgroundColor = Color.MidnightBlue;
                addEx.VerticalOptions = LayoutOptions.End;
                addEx.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                addEx.CommandParameter = CurrentUser.ex_list[i];

                StackLayoutIntensity.Children.Add(IntLabel1);
                StackLayoutIntensity.Children.Add(IntLabel2);

                StackLayoutCals.Children.Add(CalLabel1);
                StackLayoutCals.Children.Add(CalLabel2);

                StackLayoutIntMins.Children.Add(IntMinLabel1);
                StackLayoutIntMins.Children.Add(IntMinLabel2);

                StackLayoutTop.Children.Add(topLabel);
                StackLayoutTop.Children.Add(StackLayoutIntensity);
                StackLayoutTop.Children.Add(StackLayoutCals);
                StackLayoutTop.Children.Add(StackLayoutIntMins);
                StackLayoutTop.Children.Add(addEx);

                myContentView.Content = StackLayoutTop;
                
                OverheadStacklayoutm.Children.Add(myContentView);
            }
            fakeBTNClikEVnt(CurrentUser.ex_list[0].Intensity);//tricks app into thinking button was clicked so all values are displayed
        }

        private async void fakeBTNClikEVnt(string val)  // Necessary for auto population of data
        {
            CurrentUser = await helper.getUser(CurrentUser);//seet currnet user again
            foreach (ContentView CV in OverheadStacklayoutm.Children)
            {
                StackLayout slOne = CV.Content as StackLayout;
                StackLayout IntensityLayout = slOne.Children[1] as StackLayout;
                Label IntensityValue = IntensityLayout.Children[1] as Label;
                IntensityValue.Text = val;
            }

            foreach (Exercise ex in CurrentUser.ex_list)
            {
                ex.Intensity = val;
                ex.CaloriesVal = ex.Calories(ex.ExerciseTime, CurrentUser.Weight);
            }

            setIntensityMinutes();
            setCalories();
            await helper.UpdateUser(CurrentUser.Password, CurrentUser.UserName, CurrentUser.DOB, CurrentUser.Weight, CurrentUser.ex_hist, CurrentUser.ex_list);
        }

        public async void MinutesChanged(object sender, TextChangedEventArgs e) // Gets number of minutes from user entry line
        {
            //upon text change in entry box do following
            CurrentUser = await helper.getUser(CurrentUser);
            string newMin = e.NewTextValue;//new text value
            if(double.TryParse(newMin, out double newtime))//if can parse as a double
            {
                for(int i = 0; i < CurrentUser.ex_list.Count; i++)//set all exercise times
                {
                    CurrentUser.ex_list[i].ExerciseTime = newtime;
                }
                setCalories();//set calories burned for each exercise
                setIntensityMinutes();//set intensity minutes for each exercise
            }
            else if(newMin != string.Empty)//if its not empty and a non integer is entered then error
            {
                await DisplayAlert("Integers Only", "Integers can only be placed in this field!", "Ok");
                ExerciseTime.Text = string.Empty;//empty entry
            }

            await helper.UpdateUser(CurrentUser.Password, CurrentUser.UserName, CurrentUser.DOB, CurrentUser.Weight, CurrentUser.ex_hist, CurrentUser.ex_list);//update user
        }

        public void setCalories()   // Changes the number of calories outputted on the screen under each exercise
        {
            int i = 0;
            foreach (ContentView CV in OverheadStacklayoutm.Children)
            {
                // go through each content views and change the calories for each
                StackLayout slOne = CV.Content as StackLayout;//get first stacklayout
                StackLayout calLayout = slOne.Children[2] as StackLayout;//get third child ie calorie info
                Label CalValue = calLayout.Children[1] as Label;//get second child ie value
                CalValue.Text = CurrentUser.ex_list[i].Calories(CurrentUser.ex_list[i].ExerciseTime, CurrentUser.Weight).ToString("0.00");//set new value of calroies
                i++;//inc i
            }
        }

        public void setIntensityMinutes()   // Changes intensity minutes based on user entry
        {
            int i = 0;
            foreach (ContentView CV in OverheadStacklayoutm.Children)
            {
                StackLayout slOne = CV.Content as StackLayout;//get content of contentview
                StackLayout calLayout = slOne.Children[3] as StackLayout;//get forth child of stacklaoy ie intensity minutes
                Label CalValue = calLayout.Children[1] as Label;// get second child of stacklout ie value of intensity minutes
                CalValue.Text = CurrentUser.ex_list[i].IntMins(CurrentUser.ex_list[i].ExerciseTime).ToString("0.00");//set intensity minutes
                i++;//inc i
            }
        }

        public async void AddCalories(object sender, EventArgs e)   // Adds calories to bottom total calories bar
        {

            var button = sender as Button;//cast sender to a button
            double.TryParse(TotalCal.Text, out double numBurnt);//cast tect to a double
            Exercise exe = button.CommandParameter as Exercise;//cast cmd param to an exercise
            double.TryParse(ExerciseTime.Text, out double time);//parse into a value
            numBurnt += exe.Calories(time, CurrentUser.Weight);//add value to calories burned
            TotalCal.Text = numBurnt.ToString("0.00");//add value to text value
            holder.Add(CurrentUser.ex_list.Find(x => x.Name == exe.Name));
            await helper.UpdateUser(CurrentUser.Password, CurrentUser.UserName, CurrentUser.DOB, CurrentUser.Weight, holder, CurrentUser.ex_list);//update history
            CurrentUser.ex_hist = (await helper.getUser(CurrentUser.UserName)).ex_hist;//update history
            await helper.UpdateUser(CurrentUser.Password, CurrentUser.UserName, CurrentUser.DOB, CurrentUser.Weight, CurrentUser.ex_hist, CurrentUser.ex_list);//update history
        }

        public async void setIntensityLvl(object sender, EventArgs e)   // Allows user to pick high, medium, or low intensity buttons
        {
            CurrentUser = await helper.getUser(CurrentUser);//reset the current user to up to date
            var button = sender as Button;//cast sender to a button
            foreach (ContentView CV in OverheadStacklayoutm.Children)
            {
                //set intensity of each exercise dsiplayed on screen
                StackLayout slOne = CV.Content as StackLayout;
                StackLayout IntensityLayout = slOne.Children[1] as StackLayout;
                Label IntensityValue = IntensityLayout.Children[1] as Label;
                IntensityValue.Text = button.CommandParameter.ToString();//set intensity lvl to cmmd param
            }

            foreach (Exercise ex in CurrentUser.ex_list)
            {
                ex.Intensity = button.CommandParameter.ToString();//set intensity of exercise
                ex.CaloriesVal = ex.Calories(ex.ExerciseTime, CurrentUser.Weight);//set calories of exercise
            }

            setIntensityMinutes();//update intensity minutes
            setCalories();//update calories
            await helper.UpdateUser(CurrentUser.Password, CurrentUser.UserName, CurrentUser.DOB, CurrentUser.Weight, CurrentUser.ex_hist, CurrentUser.ex_list);//update user
        }

        public async void ExerciseHistoryPage(object sender, EventArgs e)   // Navigates to exercies history page
        {
            await Navigation.PushAsync(new ExerciseHistoryDisplay(CurrentUser));//navigate to exercise page
        }

        public async void DeleteAccount(object sender, EventArgs e) // Deletes user account
        {
            await helper.DeleteUser(CurrentUser.UserName, CurrentUser.Password);//delete user from database
            await Navigation.PushAsync(new MainPage());//navigate back to mainpage
        }

        public async void Logout(object sender, EventArgs e)    // Logs out user
        {
            CurrentUser = new User();//set current user to a new user
            await Navigation.PushAsync(new MainPage());//navigate to mainpage
        }

        public void SearchTextChanged(object sender, TextChangedEventArgs e)    // Updates the search results to the screen based on user input
        {
            OverheadStacklayoutm.Children.Clear();//apon text change clear layout

            foreach (Exercise ex in CurrentUser.ex_list)//for every exercise check if name contains the sequence entered
            {

                string name = ex.Name.ToLower();//cast to lowercase
                if (name.Contains(e.NewTextValue.ToLower() ))//if exercise name casted to lower case contains the text entered
                {
                    //display exercise
                    ContentView myContentView = new ContentView();
                    StackLayout StackLayoutTop = new StackLayout();
                    Label topLabel = new Label();

                    StackLayout StackLayoutIntensity = new StackLayout();
                    Label IntLabel1 = new Label();
                    Label IntLabel2 = new Label();

                    StackLayout StackLayoutCals = new StackLayout();
                    Label CalLabel1 = new Label();
                    Label CalLabel2 = new Label();

                    StackLayout StackLayoutIntMins = new StackLayout();
                    Label IntMinLabel1 = new Label();
                    Label IntMinLabel2 = new Label();

                    Button addEx = new Button();

                    myContentView.BackgroundColor = Color.LightGray;
                    StackLayoutIntensity.Orientation = StackOrientation.Horizontal;
                    StackLayoutCals.Orientation = StackOrientation.Horizontal;
                    StackLayoutIntMins.Orientation = StackOrientation.Horizontal;

                    topLabel.Text = ex.Name;
                    topLabel.FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
                    topLabel.FontAttributes = FontAttributes.Bold;
                    topLabel.HorizontalOptions = LayoutOptions.Center;

                    IntLabel1.Text = "Intensity level: ";
                    IntLabel1.Padding = new Thickness(5, 0, 0, 0);
                    IntLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    IntLabel2.Text = ex.Intensity;
                    IntLabel2.Padding = new Thickness(-5, 0, 0, 0);
                    IntLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    CalLabel1.Text = "Calories burned: ";
                    CalLabel1.Padding = new Thickness(5, 0, 0, 0);
                    CalLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    CalLabel2.Text = ex.Calories(ex.MET, CurrentUser.Weight).ToString("0.00");
                    CalLabel2.Padding = new Thickness(-5, 0, 0, 0);
                    CalLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    IntMinLabel1.Text = "Intensity minutes: ";
                    IntMinLabel1.Padding = new Thickness(5, 0, 0, 0);
                    IntMinLabel1.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    IntMinLabel2.Text = ex.IntMins(0).ToString("0.00");
                    IntMinLabel2.Padding = new Thickness(-5, 0, 0, 0);
                    IntMinLabel2.FontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                    addEx.Text = "Add to exercise list";
                    addEx.TextColor = Color.Linen;
                    addEx.Clicked += AddCalories;
                    addEx.BackgroundColor = Color.MidnightBlue;
                    addEx.VerticalOptions = LayoutOptions.End;
                    addEx.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    addEx.CommandParameter = ex;

                    StackLayoutIntensity.Children.Add(IntLabel1);
                    StackLayoutIntensity.Children.Add(IntLabel2);

                    StackLayoutCals.Children.Add(CalLabel1);
                    StackLayoutCals.Children.Add(CalLabel2);

                    StackLayoutIntMins.Children.Add(IntMinLabel1);
                    StackLayoutIntMins.Children.Add(IntMinLabel2);

                    StackLayoutTop.Children.Add(topLabel);
                    StackLayoutTop.Children.Add(StackLayoutIntensity);
                    StackLayoutTop.Children.Add(StackLayoutCals);
                    StackLayoutTop.Children.Add(StackLayoutIntMins);
                    StackLayoutTop.Children.Add(addEx);

                    myContentView.Content = StackLayoutTop;

                    OverheadStacklayoutm.Children.Add(myContentView);
                }
            }
        }
    }
}