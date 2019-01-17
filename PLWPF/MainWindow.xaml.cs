using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BE;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Admin;
using PLWPF.Nofitications;

namespace PLWPF
{
    /// <summary>
    ///     Login window
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        ///     get an object of BL
        /// </summary>
        private readonly IBl _blimp = FactoryBl.GetObject;

        /// <summary>
        ///     Login window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var bl = FactoryBl.GetObject;

            //On first login enter username and password
            if (Configuration.FirstOpenProgram)
            {
                AdminUsernameTextBox.Text = Configuration.AdminUser;
                AdminPasswordTextBox.Password = Configuration.AdminPassword;
            }

            AdminUsernameTextBox.Focus();
            Configuration.FirstOpenProgram = false;
        }

        /// <summary>
        ///     check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL(uint.Parse(TesterIDTestBox.Text)))
                {
                    //ID is correct
                    TesterLoginButton.IsEnabled = true;
                    TesterIDTestBox.BorderBrush = Brushes.LightGray;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                //ID is not correct
                TesterLoginButton.IsEnabled = false;
                TesterIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        ///     check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL(uint.Parse(TraineeIDTestBox.Text)))
                {
                    //ID is correct
                    TraineeLoginButton.IsEnabled = true;
                    TraineeIDTestBox.BorderBrush = Brushes.LightGray;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                //ID is not correct
                TraineeLoginButton.IsEnabled = false;
                TraineeIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        ///     On login click open trainee window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeLoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                //open window
                var win = new TraineeWin(int.Parse(TraineeIDTestBox.Text));
                win.ShowDialog();
                Show();

                //clean text boxes
                TraineeIDTestBox.Text = "";
                TraineeIDTestBox.BorderBrush = Brushes.LightGray;
                TraineeIDTestBox.Focus();
            }
            catch
            {
                ExceptionMessage.Show("Trainee doesn't exist please contact the administrator.");
                Show();
            }
        }

        /// <summary>
        ///     On login click open tester window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterLoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                //open window
                var win = new TesterWin(int.Parse(TesterIDTestBox.Text));
                win.ShowDialog();
                Show();

                //clean text boxes
                TesterIDTestBox.Text = "";
                TesterIDTestBox.BorderBrush = Brushes.LightGray;
                TesterIDTestBox.Focus();
            }
            catch
            {
                ExceptionMessage.Show("Tester doesn't exist please contact the administrator.");
                Show();
            }
        }

        /// <summary>
        ///     On login Click open administrator window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminLoginButton_Click(object sender, EventArgs e)
        {
            if (AdminUsernameTextBox.Text == Configuration.AdminUser &&
                AdminPasswordTextBox.Password == Configuration.AdminPassword)
            {
                //clean text boxes
                AdminUsernameTextBox.Text = "";
                AdminPasswordTextBox.Password = "";

                AdminUsernameTextBox.Focus();
                Hide();
                //open window
                var win = new Administrator();
                win.ShowDialog();
                Show();
            }
            else
            {
                ExceptionMessage.Show("Wrong Password or UserName.");
            }
        }

        /// <summary>
        ///     Login Admin on Key enter pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabItem_KeyDown_Admin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (AdminLoginButton.IsEnabled)
                    AdminLoginButton_Click(this, new EventArgs());
        }

        /// <summary>
        ///     Login Tester on Key enter pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabItem_KeyDown_Tester(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (TesterLoginButton.IsEnabled)
                    TesterLoginButton_Click(this, new EventArgs());
        }

        //Login Trainee on Key enter pressed
        private void TabItem_KeyDown_Trainee(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (TraineeLoginButton.IsEnabled)
                    TraineeLoginButton_Click(this, new EventArgs());
        }
    }
}