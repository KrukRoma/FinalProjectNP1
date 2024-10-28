using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MusicPlayer
{
    public partial class FavoriteMusicWindow : Window
    {
        private string favoritesFilePath = @"C:\Users\Roman\Desktop\Favorite Music\Favorites.txt";
        private MediaPlayer mediaPlayer;

        public FavoriteMusicWindow()
        {
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
            LoadFavoriteSongs();
        }

        private void LoadFavoriteSongs()
        {
            if (File.Exists(favoritesFilePath))
            {
                var favoriteSongs = File.ReadAllLines(favoritesFilePath);
                FavoriteMusicListBox.ItemsSource = favoriteSongs.Select(Path.GetFileName).ToList();
            }
            else
            {
                MessageBox.Show("No favorite songs found.");
            }
        }

        private void FavoriteMusicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FavoriteMusicListBox.SelectedItem != null)
            {
                string selectedSong = (string)FavoriteMusicListBox.SelectedItem;
                string songPath = Path.Combine(@"C:\Users\Roman\Desktop\Music", selectedSong);

                try
                {
                    mediaPlayer.Stop();
                    mediaPlayer.Open(new Uri(songPath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading song: {ex.Message}");
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavoriteMusicListBox.SelectedItem != null)
            {
                mediaPlayer.Play();
            }
            else
            {
                MessageBox.Show("Please select a song to play.");
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void RemoveFromFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedSong = (string)FavoriteMusicListBox.SelectedItem;

            if (selectedSong != null)
            {
                string songFileName = selectedSong;

                var lines = File.ReadAllLines(favoritesFilePath).Where(line => Path.GetFileName(line) != songFileName).ToList();

                File.WriteAllLines(favoritesFilePath, lines);

                MessageBox.Show("Пісню видалено з улюблених.");
                LoadFavoriteSongs(); 
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть пісню для видалення з улюблених.");
            }
        }


        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            this.Close();
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
        }
    }
}
