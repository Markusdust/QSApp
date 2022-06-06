using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Notes.Views
{

    public partial class NotesPage : ContentPage
    {
        EmailMessage message;
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");
        string imagePath;

        public NotesPage()
        {
            InitializeComponent();

            errorlocation.Items.Add("SG Intern");
            errorlocation.Items.Add("SG Extern");
            errorlocation.Items.Add("Biel Intern");
            errorlocation.Items.Add("Biel Extern");

            errorTypePicker.Items.Add("Mangelhafte Handhabung Intern");
            errorTypePicker.Items.Add("Mangelhafte Ausführung Intern");

            errorTypePicker.Items.Add("Mangelhafte Handhabung Extern");
            errorTypePicker.Items.Add("Mangelhafte Ausführung Extern");

            // Read the file.
            if (File.Exists(_fileName))
            {
                editor.Text = File.ReadAllText(_fileName);
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // // Save the file.
            //// File.WriteAllText(_fileName, editor.Text);

            //Save and send as email.
            //var message = new EmailMessage("Betreff", "Body", to: "markusstaub1@gmail.com");
            //message.Attachments
            //message.BodyFormat = EmailBodyFormat.PlainText;
            //await Email.ComposeAsync(message);

            var email = new List<string>();
            email.Add("abc@123.com");


            var partNr = editor.Text;
            var subject = title.Text;
            var descpriptionValue = description.Text;
            //var email1 = emailadress.Text;

            string selectedErrrortype = Convert.ToString(errorlocation.SelectedItem);
            string selectedErrorlocation = Convert.ToString(errorTypePicker.SelectedItem);

            string amountValue = amount.Text;

            var body = "Art: " + selectedErrorlocation + "\r\n"
                      + "Problemkreis: " + selectedErrrortype + "\r\n" + "\r\n"
                      + "ArtikelNr: " + partNr + "\t" + "           Menge: " + amountValue + "\r\n"
                      + "Kurztext: " + subject + "\r\n" + "\r\n"
                      + "Beschreibung: " + "\r\n" + descpriptionValue;


            //Betreff              Body   email an...
            await SendEmail("QS_APP: " + partNr + " " + subject, body, email);


        }

        void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            // Delete the file.
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
            editor.Text = string.Empty;
        }

        async void OnPictureClicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Bitte Foto aussuchen"
            });

            var stream = await result.OpenReadAsync();
            // var imagePath = await result.OpenReadAsync();
            //  resultImage.Source = ImageSource.FromStream(() => stream);

            imagePath = result.FullPath;

        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipient
                    //Attachments=,

                };

                if (imagePath != null)
                {
                    message.Attachments.Add(new EmailAttachment(imagePath));
                }
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        private void editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (editor.Text != String.Empty)
            {
                lblArtikelNr.IsVisible = true;
            }
            else
            lblArtikelNr.IsVisible = false;

        }

        private void receipt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (receipt.Text != String.Empty)
            {
                lblReceipt.IsVisible = true;
            }
            else
            lblReceipt.IsVisible = false;
        }

        private void amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (amount.Text != String.Empty)
            {
                lblAmount.IsVisible = true;
            }
            else
            lblAmount.IsVisible = false;
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (title.Text != String.Empty)
            {
                lblTitle.IsVisible = true;
            }
            else
            lblTitle.IsVisible = false;
        }

        private void description_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (description.Text != String.Empty)
            {
                lblDescription.IsVisible = true;
            }
            else
            lblDescription.IsVisible = false;
        }

        private void errorlocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (errorlocation.SelectedIndex != -1)
            {
                lblArt.IsVisible = true;
            }
            else
            lblArt.IsVisible = false;
        }

        private void setErrorTypList()
        {
            var selection = errorlocation.SelectedItem.ToString();
            if (selection.Contains("SG Intern") | selection.Contains("Biel Intern"))
            {
                FillErrorTypPickerIntern();
            }
            else if (selection.Contains("SG Extern") | selection.Contains("Biel Extern"))
            {
                FillErrorTypPickerExtern();
            }
        }


        private void FillErrorTypPickerIntern()
        {
            errorTypePicker.Items.Add("Mangelhafte Handhabung Intern");
            errorTypePicker.Items.Add("Mangelhafte Ausführung Intern");
        }

        private void FillErrorTypPickerExtern()
        {
            errorTypePicker.Items.Add("Mangelhafte Handhabung Extern");
            errorTypePicker.Items.Add("Mangelhafte Ausführung Extern");
        }

        private void errorTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (errorTypePicker.SelectedIndex != -1)
            {
                lblProblemkreis.IsVisible = true;
            }
            else
            lblProblemkreis.IsVisible = false;
        }
    }
}