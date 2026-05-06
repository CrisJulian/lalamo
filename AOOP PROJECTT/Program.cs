namespace AOOP_PROJECTT
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Form loginHost = new Form();
            loginHost.Text = "CommonCents";
            loginHost.Size = new System.Drawing.Size(1180, 730);
            loginHost.StartPosition = FormStartPosition.CenterScreen;
            loginHost.FormBorderStyle = FormBorderStyle.FixedSingle;
            loginHost.MaximizeBox = false;

            usLogin loginControl = new usLogin();
            loginControl.Dock = DockStyle.Fill;
            loginHost.Controls.Add(loginControl);

            bool loggedIn = false;

            loginControl.LoginSuccessful += (s, e) =>
            {
                loggedIn = true;
                loginHost.Close();
            };

            loginControl.NavigateToSignin += (s, e) =>
            {
                loginHost.Controls.Clear();
                usSignin signinControl = new usSignin();
                signinControl.Dock = DockStyle.Fill;

                signinControl.AccountCreated += (s2, e2) =>
                {
                    loggedIn = true;
                    loginHost.Close();
                };

                signinControl.NavigateToLogin += (s2, e2) =>
                {
                    loginHost.Controls.Clear();
                    loginControl.Dock = DockStyle.Fill;
                    loginHost.Controls.Add(loginControl);
                };

                loginHost.Controls.Add(signinControl);
            };

            loginHost.ShowDialog();

            if (loggedIn)
                Application.Run(new Form1());
        }
    }
}