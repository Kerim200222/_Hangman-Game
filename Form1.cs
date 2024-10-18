using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;


namespace AdamAsmacaa
{
    public partial class Form1 : Form
    {
        #region Constructors
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region Variables
        // Kelime listesi, oyun sırasında rastgele bir kelimenin seçilmesi için kullanılır.
        List<string> lstWords = new List<string>()
        {
            "adana","konya", "gaziantep", "bursa","bursa", "antalya", "izmir","ankara",
            "mersin","elazig","kilis","diyarbakir"
        };
        int incorrectGuess;  // Yanlış tahmin sayısını takip eder.
        Random random;  // Rastgele kelime seçimi için kullanılır.
        string selectedWord;  // Seçilen kelimeyi depolar.
        char[] displayword;  // Kullanıcıya gösterilecek olan kelimenin tahmin edilmiş harflerini depolar.
        #endregion

        // Adam asmaca resmi güncellenir, yanlış tahmin sayısına göre resimler değişir.
        private void UpdateAdamAsmacaImage()
        {
            incorrectGuess++;  // Yanlış tahmin sayısını artırır.
            switch (incorrectGuess)
            {
                case 1:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_1;
                    break;
                case 2:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_2;
                    break;
                case 3:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_3;
                    break;
                case 4:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_4;
                    break;
                case 5:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_5;
                    break;
                case 6:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_6;
                    break;
                case 7:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_7;
                    break;
                case 8:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_8;
                    break;
                case 9:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_9;
                    break;
                case 10:
                    pictureBox1.Image = Properties.Resources.AdamAsmaca_10;
                    playSoundGameOver(); // Oyun bitti sesi çalar.
                    MessageBox.Show("You,ve lost! The word was " + selectedWord); // Oyuncuya kaybettiğini ve kelimeyi gösterir.
                    Application.Restart(); // Oyun yeniden başlatılır.
                    break;
            }
        }

        // Hata sesi çalmak için kullanılır.
        public void playSoundError()
        {
            using (var audioFile = new AudioFileReader("C:\\Users\\abooa\\source\\repos\\AdamAsmacaa\\Resources\\error.mp3"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100); // Ses çalma işlemi bitene kadar beklenir.
                }
            }
        }

        // Oyun bittiğinde çalan ses.
        public void playSoundGameOver()
        {
            using (var audioFile = new AudioFileReader("C:\\Users\\abooa\\source\\repos\\AdamAsmacaa\\Resources\\gameOver.mp3"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        // Doğru tahmin yapıldığında çalan ses.
        public void playSoundDing()
        {
            using (var audioFile = new AudioFileReader("C:\\Users\\abooa\\source\\repos\\AdamAsmacaa\\Resources\\ding.mp3"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        // Rastgele bir kelime seçer ve oyun başlangıç durumunu sıfırlar.
        public void RandomWord()
        {
            incorrectGuess = 0; // Yanlış tahmin sayısını sıfırlar.
            random = new Random();
            selectedWord = lstWords[random.Next(lstWords.Count)];// Liste içerisinden rastgele bir kelime seçer.
            displayword = new string('_', selectedWord.Length).ToCharArray();// Kelimenin her harfini _ ile gösterir.
            string formattedDisplayWord = string.Join(" ", displayword);
            label1.Text = formattedDisplayWord;// Kelimenin her harfini _ ile gösterir.
        }
        #endregion

        #region UI Elements
        // Form yüklendiğinde rastgele kelime seçilir.
        private void Form1_Load(object sender, EventArgs e)
        {
            RandomWord();
        }

        // Tahmin edilen harfin kontrolünü yapan buton işlemi.
        private void button1_Click(object sender, EventArgs e)
        {
            char guess = textBox1.Text.ToLower()[0];// Girilen harfi alır ve küçük harfe çevirir.
            bool correctGuess = false; // Doğru tahmin yapılıp yapılmadığını takip eder.
            for (int i = 0; i < selectedWord.Length; i++)
            {
                if (selectedWord[i] == guess)// Girilen harf kelimede varsa
                {
                    displayword[i] = guess;// Harfi doğru yere koyar.
                    correctGuess = true;
                }
            }

            label1.Text = string.Join(" ", displayword);// Güncellenen kelimeyi ekranda gösterir.
            if (!correctGuess) // Eğer tahmin yanlışsa
            {
                UpdateAdamAsmacaImage();// Adam asmaca resmini günceller.
                playSoundError(); // Hata sesi çalar.
                textBox1.Clear();// TextBox'ı temizler.
            }
            else// Eğer tahmin doğruysa
            {
                playSoundDing();// Doğru tahmin sesi çalar.
                textBox1.Clear();// TextBox'ı temizler.
            }

            if (!label1.Text.Contains('_'))// Eğer tüm harfler doğru tahmin edilmişse
            {
                MessageBox.Show("Congratulations! You,ve won");// Kazanma mesajı gösterir.
                Application.Restart();// Oyun yeniden başlar.
            }
        }

        // Yeni bir oyun başlatmak için kullanılan buton
        private void button2_Click(object sender, EventArgs e)
        {
            RandomWord();// Yeni bir kelime seçer ve oyunu sıfırlar.
        }
        #endregion
    }
}