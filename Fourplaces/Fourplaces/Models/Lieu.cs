using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace Fourplaces.Models
{
    public class Lieu
    {
        public Guid Id { get; set; }

        public int IdApi { get; set; }

        public int ImageId { get; set; }

        public string Nom { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        public Image Image { get; set; }

        public Position Position { get; set; }

        public byte[] ImageBytes { get; set; }

        public Lieu() { }

        public Lieu(string nom, string description, string imageSource, Position position)
        {
            Nom = nom;
            Description = description;
            ImageSource = imageSource;
            Position = position;
        }

        public Lieu(int imageId, string nom, string description, string imageSource, Position position)
        {
            Nom = nom;
            Description = description;
            ImageSource = imageSource;
            ImageId = imageId;
            Position = position;
        }

    }
}