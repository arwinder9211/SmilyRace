using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Collections;
using Xceed.Wpf.Toolkit;

namespace SmilyRace
{
    public partial class MainWindow : Window
    {
        Random rnd = new Random(DateTime.Now.Second);
        enum GameStates { selectName, SelectImage, SelectBet, ReadyToStart, RaceInProgress, RaceEnd };
        GameStates CGS1;
        GameStates CGS2;
        GameStates CGS3;
        Image winnerImage;
        Image SI1, SI2, SI3;

        int Amount1, Amount2, Amount3;
        StringBuilder WTimes;
        int ICCount;
        Dictionary<String, int> BFM;
        Storyboard sb = new Storyboard();
        DoubleAnimation da;
        public static int BE1 = 50;
        public static int BE2 = 50;
        public static int BE3 = 50;
        enum BetBettor { Smith, Kevin, Michael };
        BetBettor bettor;
        string FName = "";
        StringBuilder GMessage = new StringBuilder();
        public MainWindow()
        {
            InitializeComponent();
            WTimes = new StringBuilder();
            ICCount = 0;
            CGS1 = CGS2 = CGS3 = GameStates.SelectImage;

            switch (bettor)
            {
                case BetBettor.Smith:
                    FName = "Smith";
                    break;
                case BetBettor.Kevin:
                    FName = "Kevin";
                    break;
                case BetBettor.Michael:
                    FName = "Michael";
                    break;
            }
            System.Windows.MessageBox.Show("Select Your Player and Image to Place Your Bet for " + FName + "\n Hover on Image to get details");

            BFM = new Dictionary<string, int>();
            BFM.Add(DollerSmily.Name, 2);
            BFM.Add(HuggingSmily.Name, 2);
            BFM.Add(AttitudeSmily.Name, 2);
            BFM.Add(KiddingSmily.Name, 2);
            //maxamount = rnd.Next(1, 50);
            betdoller.Maximum = BE1;
            betsMesage.Content = "Max bet is : $ " + BE1;
            FirstBet.Content = "Smith hasn't placed a bet and has $" + BE1;
            SecondBet.Content = "Kevin hasn't placed a bet and has $" + BE2;
            ThirdBet.Content = "Michael hasn't placed a bet and has $" + BE3;

        }

        //start button handler
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (0 == Start.Content.ToString().CompareTo("Reset"))
            {
                ResetGame();
                Start.Content = "Start";
                return;
            }



            if (CGS1 != GameStates.ReadyToStart && CGS2 != GameStates.ReadyToStart && CGS3 != GameStates.ReadyToStart)
            {
                String s = "Game is not ready to Start!, Please Follow The Message Below";
                System.Windows.MessageBox.Show(s);
                return;
            }



            da = new DoubleAnimation(0, 600, TimeSpan.FromSeconds(rnd.Next(10, 15) % 15));
            da.AccelerationRatio = rnd.NextDouble();
            da.Completed += da_Completed;
            da.Name = "DollerSmily";
            DollerSmily.BeginAnimation(Canvas.TopProperty, da);

            da = new DoubleAnimation(0, 600, TimeSpan.FromSeconds(rnd.Next(10, 15) % 15));
            da.AccelerationRatio = rnd.NextDouble();
            da.Completed += da_Completed;
            da.Name = "HuggingSmily";
            HuggingSmily.BeginAnimation(Canvas.TopProperty, da);

            da = new DoubleAnimation(0, 600, TimeSpan.FromSeconds(rnd.Next(10, 15) % 15));
            da.AccelerationRatio = rnd.NextDouble();
            da.Completed += da_Completed;
            da.Name = "AttitudeSmily";
            AttitudeSmily.BeginAnimation(Canvas.TopProperty, da);

            da = new DoubleAnimation(0, 600, TimeSpan.FromSeconds(rnd.Next(10, 15) % 15));
            da.AccelerationRatio = rnd.NextDouble();
            da.Completed += da_Completed;
            da.Name = "KiddingSmily";
            KiddingSmily.BeginAnimation(Canvas.TopProperty, da);
        }

        void da_Completed(object sender, EventArgs e)
        {
            GMessage = new StringBuilder();
            AnimationClock ac = (AnimationClock)sender;
            DoubleAnimation d = (DoubleAnimation)ac.Timeline;
            StringBuilder s = new StringBuilder(d.Name);
            s.Append(" Completed Race in");
            s.Append(d.Duration.ToString());
            s.Append("sec");
            GMessage.Append(s.ToString()+"\n");


            WTimes.Append(d.Name);
            WTimes.Append("=");
            WTimes.Append(d.Duration.TimeSpan.Seconds.ToString());
            WTimes.Append("sec");
            WTimes.Append("\n");

            ICCount++;


            if (ICCount == 1)
            {
                winnerImage = (Image)GameGrid.FindName(d.Name);
            }

            if (ICCount == 4)
            {
                CGS1 = CGS2 = CGS3 = GameStates.RaceEnd;
                DisplayStateMessage();
                System.Windows.MessageBox.Show(GMessage.ToString());
            }



        }

        private void Image1_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            
            if (bets1.IsChecked == true)
            {
                if (CGS1 != GameStates.SelectImage)
                {
                    System.Windows.MessageBox.Show("You cannot select Image now. please follow message below");
                    return;
                }
            }
            else if (bets2.IsChecked == true)
            {
                if (CGS2 != GameStates.SelectImage)
                {
                    System.Windows.MessageBox.Show("You cannot select Image now. please follow message below");
                    return;
                }
            }
            else if (bets3.IsChecked == true)
            {
                if (CGS3 != GameStates.SelectImage)
                {
                    System.Windows.MessageBox.Show("You cannot select Image now. please follow message below");
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select your player first to initiate game.");
                return;
            }

            Image w = (Image)sender;
            switch (bettor)
            {
                case BetBettor.Smith:
                    FName = "Smith";
                    CGS1 = GameStates.SelectBet;
                    SI1 = w;
                    w.IsEnabled = false;
                    break;
                case BetBettor.Kevin:
                    FName = "Kevin";
                    CGS2 = GameStates.SelectBet;
                    SI2 = w;
                    w.IsEnabled = false;
                    break;
                case BetBettor.Michael:
                    FName = "Michael";
                    CGS3 = GameStates.SelectBet;
                    SI3 = w;
                    w.IsEnabled = false;
                    break;
            }

            StringBuilder txt = new StringBuilder("You Placed Bet on Image");
            txt.Append(w.Name);
            txt.Append(" for " + FName);
            GMessage.Append(txt.ToString() + "\n");
        }

        private void Image1_MouseEnter_1(object sender, MouseEventArgs e)
        {
            GMessage = new StringBuilder();
            Image w = (Image)sender;
            StringBuilder txt = new StringBuilder("This is ");
            txt.Append(w.Name);
            if (w.IsEnabled == false)
            {
                if (w.Name == SI1.Name)
                {
                    txt.Append(" already selected for Smith");
                    GMessage.Append(txt.ToString() + "\n");
                    return;
                }
                else if (w.Name == SI2.Name)
                {
                    txt.Append(" already selected for Kevin");
                    GMessage.Append(txt.ToString() + "\n");
                    return;
                }
                else if (w.Name == SI3.Name)
                {
                    txt.Append(" already selected for Michael");
                    GMessage.Append(txt.ToString() + "\n");
                    return;
                }
                else
                {
                    txt.Append(", You will get your Bet x ");
                    txt.Append(BFM[w.Name].ToString());
                    GMessage.Append(txt.ToString() + "\n");
                    return;
                }
            }

            //w.border.BorderThickness = new Thickness(2, 2, 2, 2);
            //w.border.BorderBrush = Brushes.LightCoral;

            txt.Append(", You will get your Bet x ");
            txt.Append(BFM[w.Name].ToString());
            GMessage.Append(txt.ToString()+"\n");
        }

        private void Image1_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Image w = (Image)sender;
            //w.border.BorderBrush = Brushes.Transparent;
            DisplayStateMessage();
            System.Windows.MessageBox.Show(GMessage.ToString());
        }

        private void DisplayStateMessage()
        {
            if (CGS1 == GameStates.RaceEnd && CGS2 == GameStates.RaceEnd && CGS3 == GameStates.RaceEnd)
            {
                StringBuilder s = new StringBuilder(WTimes.ToString());
                s.Append("Winner is :");
                s.Append(winnerImage.Name);
                s.Append("\nBet Placement was below:");
                s.Append("\nSmith : " + SI1.Name + ", Amount : $" + Amount1);
                s.Append("\nKevin : " + SI2.Name + ", Amount : $" + Amount2);
                s.Append("\nMichael : " + SI3.Name + ", Amount : $" + Amount3);
                s.Append(", Click Reset to play again");
                GMessage.Append(s.ToString()+"\n");
                Start.Content = "Reset";
                if (0 == winnerImage.Name.CompareTo(SI1.Name))
                {
                    s = new StringBuilder();
                    BE1 -= Amount1;
                    BE1 += (Amount1 * BFM[SI1.Name]);
                    s.Append("Smith Won and now has $" + BE1);


                    FirstBet.Content = s.ToString();
                    BE2 -= Amount2;
                    SecondBet.Content = "Kevin lost and now has $" + BE2;
                    BE3 -= Amount3;
                    ThirdBet.Content = "Michael lost and now has $" + BE3;
                }
                else if (0 == winnerImage.Name.CompareTo(SI2.Name))
                {
                    s = new StringBuilder();
                    BE2 -= Amount2;
                    BE2 += (Amount2 * BFM[SI2.Name]);
                    s.Append("Kevin Won and now has $" + BE2);

                    SecondBet.Content = s.ToString();
                    BE1 -= Amount1;
                    FirstBet.Content = "Smith lost and now has $" + BE1;
                    BE3 -= Amount3;
                    ThirdBet.Content = "Michael lost and now has $" + BE3;
                }
                else if (0 == winnerImage.Name.CompareTo(SI3.Name))
                {
                    s = new StringBuilder();
                    BE3 -= Amount3;
                    BE3 += (Amount3 * BFM[SI3.Name]);
                    s.Append("Michael Won and now has $" + BE3);

                    ThirdBet.Content = s.ToString();
                    BE1 -= Amount1;
                    FirstBet.Content = "Smith lost and now has $" + BE1;
                    BE2 -= Amount2;
                    SecondBet.Content = "Kevin lost and now has $" + BE2;
                }
                else
                {
                    s.Append("\n\n");
                    s.Append("Sorry! Better Luck Next Time\t");
                    s.Append("All players are lost.");
                    
                    s.Append(", Click Reset to play again");
                    GMessage.Append(s.ToString() + "\n");

                    BE1 -= Amount1;
                    FirstBet.Content = "Smith lost and now has $" + BE1;
                    BE2 -= Amount2;
                    SecondBet.Content = "Kevin lost and now has $" + BE2;
                    BE3 -= Amount3;
                    ThirdBet.Content = "Michael lost and now has $" + BE3;
                }
                if (BE1 == 0)
                {
                    FirstBet.Content = "Smith lost all money, BUSTED !";
                    bets1.IsEnabled = false;
                }
                if (BE2 == 0)
                {
                    SecondBet.Content = "Kevin lost all money, BUSTED !";
                    bets2.IsEnabled = false;
                }
                if (BE3 == 0)
                {
                    ThirdBet.Content = "Michael lost all money, BUSTED !";
                    bets3.IsEnabled = false;
                }

            }
            else if (bets1.IsChecked == true)
            {
                switch (CGS1)
                {
                    case GameStates.SelectImage:
                        {
                            GMessage.Append("\n Select Your Image By Clicking on Image for Smith ");
                        }
                        break;
                    case GameStates.SelectBet:
                        {
                            StringBuilder s = new StringBuilder("\n You have selected Image : ");
                            s.Append(SI1.Name);
                            s.Append("\n Now Select Amount You Want to Bet for Smith");
                            GMessage.Append(s.ToString() + "\n");
                            betdoller.IsEnabled = true;
                            buttonConfirm.Visibility = Visibility.Visible;
                            betdoller.Value = 0;
                        }
                        break;
                    case GameStates.ReadyToStart:
                        {
                            StringBuilder s = new StringBuilder("\n Your have selected Image:");
                            s.Append(SI1.Name);
                            s.Append(", bet amount of player Smith is ");
                            s.Append(": $" + Amount1);
                            GMessage.Append(s.ToString() + "\n");
                        }
                        break;
                }
            }
            else if (bets2.IsChecked == true)
            {
                switch (CGS2)
                {
                    case GameStates.SelectImage:
                        {
                            GMessage.Append("\n Select Your Image By Clicking on Image for Kevin ");
                        }
                        break;
                    case GameStates.SelectBet:
                        {
                            StringBuilder s = new StringBuilder("\n You have selected Image : ");
                            s.Append(SI2.Name);
                            s.Append("\n Now Select Amount You Want to Bet for Kevin");
                            GMessage.Append(s.ToString() + "\n");
                            betdoller.IsEnabled = true;
                            buttonConfirm.Visibility = Visibility.Visible;
                            betdoller.Value = 0;
                        }
                        break;
                    case GameStates.ReadyToStart:
                        {
                            StringBuilder s = new StringBuilder("\n Your have selected Image:");
                            s.Append(SI2.Name);
                            s.Append(", bet amount of player Kevin is ");
                            s.Append(": $" + Amount2);
                            GMessage.Append(s.ToString() + "\n");
                        }
                        break;
                }
            }
            else if (bets3.IsChecked == true)
            {
                switch (CGS3)
                {
                    case GameStates.SelectImage:
                        {
                            GMessage.Append("\n Select Your Image By Clicking on Image for Kevin ");
                        }
                        break;
                    case GameStates.SelectBet:
                        {
                            StringBuilder s = new StringBuilder("\n You have selected Image : ");
                            s.Append(SI3.Name);
                            s.Append("\n Now Select Amount You Want to Bet for Michael");
                            GMessage.Append(s.ToString() + "\n");
                            betdoller.IsEnabled = true;
                            buttonConfirm.Visibility = Visibility.Visible;
                            betdoller.Value = 0;
                        }
                        break;
                    case GameStates.ReadyToStart:
                        {
                            StringBuilder s = new StringBuilder("\n Your have selected Image:");
                            s.Append(SI3.Name);
                            s.Append(", bet amount of player Michael is ");
                            s.Append(": $" + Amount3);
                            GMessage.Append(s.ToString() + "\n");
                        }
                        break;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select your player first to initiate game.");
                return;
            }
            

        }

        private void ResetGame()
        {
            GMessage = new StringBuilder();
            sb.Children.Remove(da);
            DoubleAnimation da1 = new DoubleAnimation(600, 0, TimeSpan.FromSeconds(1));
            DollerSmily.BeginAnimation(Canvas.TopProperty, da1);
            HuggingSmily.BeginAnimation(Canvas.TopProperty, da1);
            AttitudeSmily.BeginAnimation(Canvas.TopProperty, da1);
            KiddingSmily.BeginAnimation(Canvas.TopProperty, da1);


            ICCount = 0;
            CGS1 = GameStates.SelectImage;
            CGS2 = GameStates.SelectImage;
            CGS3 = GameStates.SelectImage;
            DollerSmily.IsEnabled = HuggingSmily.IsEnabled = AttitudeSmily.IsEnabled = KiddingSmily.IsEnabled = true;
            WTimes = new StringBuilder();
            winnerImage = null;
            GMessage.Append("Start again with new bets. \n and Proceed further.");
            WTimes.Clear();

            BFM = new Dictionary<string, int>();
            BFM.Add(DollerSmily.Name, rnd.Next(3, 6));
            BFM.Add(HuggingSmily.Name, rnd.Next(3, 6));
            BFM.Add(AttitudeSmily.Name, rnd.Next(3, 6));
            BFM.Add(KiddingSmily.Name, rnd.Next(3, 6));

            if (BE1 < Amount1)
                Amount1 = BE1;
            if (BE2 < Amount2)
                Amount2 = BE2;
            if (BE3 < Amount3)
                Amount3 = BE3;

            System.Windows.MessageBox.Show(GMessage.ToString());
        }

        

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (bets1.IsChecked == true)
            {
                if (CGS1 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else if (bets2.IsChecked == true)
            {
                if (CGS2 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else if (bets3.IsChecked == true)
            {
                if (CGS3 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select your player first to initiate game.");
                return;
            }

            switch (bettor)
            {
                case BetBettor.Smith:
                    CGS1 = GameStates.ReadyToStart;

                    break;
                case BetBettor.Kevin:
                    CGS2 = GameStates.ReadyToStart;
                    break;
                case BetBettor.Michael:
                    CGS3 = GameStates.ReadyToStart;
                    break;
            }
            buttonConfirm.Visibility = Visibility.Hidden;
            DisplayStateMessage();
            if (CGS1 == GameStates.ReadyToStart && CGS2 == GameStates.ReadyToStart && CGS3 == GameStates.ReadyToStart)
            {
                StringBuilder s = new StringBuilder("Your players and Images are selected");
                s.Append(", bet amount of players are as below.");
                s.Append("\nBet Placement was below:");
                s.Append("\nSmith : " + SI1.Name + ", Amount : $" + Amount1);
                s.Append("\nKevin : " + SI2.Name + ", Amount : $" + Amount2);
                s.Append("\nMichael : " + SI3.Name + ", Amount : $" + Amount3);
                s.Append("\n Click Start");
                GMessage.Append(s.ToString() + "\n");
            }
            System.Windows.MessageBox.Show(GMessage.ToString());
        }

        private void ListBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void bets1_Checked(object sender, RoutedEventArgs e)
        {

            switch (((RadioButton)sender).Name)
            {
                case "bets1":
                    if (CGS1 == GameStates.SelectImage)
                        betdoller.IsEnabled = false;
                    else if (CGS1 == GameStates.ReadyToStart)
                    {
                        betdoller.IsEnabled = false;
                        buttonConfirm.Visibility = Visibility.Hidden;
                    }
                    else if (CGS1 == GameStates.SelectBet)
                    {
                        buttonConfirm.Visibility = Visibility.Visible;
                    }
                    bettor = BetBettor.Smith;
                    if (betdoller != null) betdoller.Maximum = BE1;
                    if (betsMesage != null) betsMesage.Content = "Max bet is : $ " + BE1;
                    break;
                case "bets2":
                    if (CGS2 == GameStates.SelectImage)
                        betdoller.IsEnabled = false;
                    else if (CGS1 == GameStates.ReadyToStart)
                    {
                        betdoller.IsEnabled = false;
                        buttonConfirm.Visibility = Visibility.Hidden;
                    }
                    else if (CGS1 == GameStates.SelectBet)
                    {
                        buttonConfirm.Visibility = Visibility.Visible;
                    }
                    bettor = BetBettor.Kevin;
                    if (betdoller != null) betdoller.Maximum = BE2;
                    if (betsMesage != null) betsMesage.Content = "Max bet is : $ " + BE2;
                    break;
                case "bets3":
                    if (CGS3 == GameStates.SelectImage)
                        betdoller.IsEnabled = false;
                    else if (CGS1 == GameStates.ReadyToStart)
                    {
                        betdoller.IsEnabled = false;
                        buttonConfirm.Visibility = Visibility.Hidden;
                    }
                    else if (CGS1 == GameStates.SelectBet)
                    {
                        buttonConfirm.Visibility = Visibility.Visible;
                    }
                    bettor = BetBettor.Michael;
                    if (betdoller != null) betdoller.Maximum = BE3;
                    if (betsMesage != null) betsMesage.Content = "Max bet is : $ " + BE3;
                    break;
            }
        }

        private void betdoller_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            if (bets1.IsChecked == true)
            {
                if (CGS1 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else if (bets2.IsChecked == true)
            {
                if (CGS2 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else if (bets3.IsChecked == true)
            {
                if (CGS3 != GameStates.SelectBet)
                {
                    String s = "Game is not ready to Start!, Please Follow The Message Below";
                    System.Windows.MessageBox.Show(s);
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select your player first to initiate game.");
                return;
            }

            switch (bettor)
            {
                case BetBettor.Smith:
                    Amount1 = (int)betdoller.Value;
                    FirstBet.Content = "Smith bets $" + betdoller.Value + " on " + SI1.Name;

                    break;
                case BetBettor.Kevin:
                    Amount2 = (int)betdoller.Value;
                    SecondBet.Content = "Kevin bets $" + betdoller.Value + " on " + SI2.Name;
                    break;
                case BetBettor.Michael:
                    Amount3 = (int)betdoller.Value;
                    ThirdBet.Content = "Michael bets $" + betdoller.Value + " on " + SI3.Name;
                    break;
            }

        }

    }
}
