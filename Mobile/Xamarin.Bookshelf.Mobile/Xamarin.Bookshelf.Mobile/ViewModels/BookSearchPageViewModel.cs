using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Bookshelf.Shared.Models;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.ViewModels
{
    public class BookSearchPageViewModel: BaseViewModel
    {
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private ObservableCollection<Book> books;
        public ObservableCollection<Book> Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        public ICommand SearchCommand { get; }

        public BookSearchPageViewModel()
        {
            SearchCommand = new Command(SearchBooks, CanSearchBooks);
        }

        private void SearchBooks(object arg)
        {            
        }

        private bool CanSearchBooks(object arg)
        {
            return !string.IsNullOrWhiteSpace(searchText);
        }
    }
}
