using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using Application.DataAccess.MetaData;
using Application.ViewModels;
using Application.DataAccess.MetaData.Privilege;
using Application.DataAccess.MetaData.Role;
using Application.DataAccess.MetaData.User;

namespace Application
{
    public partial class App : Microsoft.UI.Xaml.Application
    {
        public IServiceProvider? serviceProvider { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.signInClicked += OnSignInClicked;
            m_window.Activate();
        }

        private async  void OnSignInClicked(string username, string password)
        {
            try
            {
                string connectionString = "";

                if(username.Equals("sys"))
                    connectionString = $"User Id={username};Password={password};Data Source=localhost:1521/XEPDB1;DBA Privilege=SYSDBA";
                else
                    connectionString = $"User Id={username};Password={password};Data Source=localhost:1521/XEPDB1";

                var sqlConnection = new OracleConnection(connectionString);
                await sqlConnection.OpenAsync();
                
                var services = new ServiceCollection();

                // Đăng ký OracleConnection
                services.AddSingleton(sqlConnection);

                // Đăng ký PrivilegeOracleDao
                services.AddSingleton<IPrivilegeDao, PrivilegeOracleDao>();
                services.AddSingleton<IRoleDao, RoleOracleDao>();
                services.AddSingleton<IUserDao, UserOracleDao>();

                serviceProvider = services.BuildServiceProvider();

                m_window.SignInSuccessHandler();
            }
            catch (Exception ex)
            {
                m_window.SignInFailedHandler();
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
            finally
            {
                if (serviceProvider != null)
                {
                    var sqlConnection = serviceProvider.GetService<OracleConnection>();
                    if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }

                }
            }
        }

        private MainWindow? m_window;
    }
}
