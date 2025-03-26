using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model;

namespace Application.ViewModels
{
    public class DataViewModel<T> : INotifyPropertyChanged
    {
        public ObservableCollection<T> items { get; set; }

        public DataViewModel()
        {
            items = LoadData();
        }
        private ObservableCollection<T> LoadData()
        {
            List<T> list = new List<T>();

            if (typeof(T) == typeof(User))
            {
                User user = new User(){
                    username = "admin",
                    password = "admin"
                };

                list.Add((T)Convert.ChangeType(user, typeof(T)));
            }
            else if (typeof(T) == typeof(Role))
            {
                Role role = new Role(){
                    name = "NVCB",
                };

                list.Add((T)Convert.ChangeType(role, typeof(T)));
            }

            return new ObservableCollection<T>(list);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
