using eBookReader.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eBookReader.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        
        public ProfileView()
        {
            InitializeComponent();
            LoadUsername();
            LoadUserBookShelf();
        }

        private async void LoadUserBookShelf()
        {
            List<string> books = User.GetBooks();
            List<Book> booksToShowOnShelf = new List<Book>();
            
            foreach(string book in books)
            {
                Book newBook = new Book(book);
                await newBook.GetAuthor();
                booksToShowOnShelf.Add(newBook);
            }

            foreach(Book book in booksToShowOnShelf)
            {
                BooksDataGrid.Items.Add(book);
            }
          
        }

        private void LoadUsername()
        {
            string username = User.GetUsername();
            userNameLbl.Content = username;
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Book book = (Book)BooksDataGrid.SelectedItem;
                PDFView pdfViewer = new PDFView(book);
                pdfViewer.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
