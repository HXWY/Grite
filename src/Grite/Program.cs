using System;
using System.IO;
using Grite.Entities;
using SixLabors.ImageSharp;

namespace Grite {
    class Program {
        private const int MaxPerRow = 8;
        
        static void Main(string[] args) {
            if (args.Length == 0)
                ExitApp("No image provided!");
            
            var imagePath = args[0];
            if (!imagePath.EndsWith(".gif"))
                ExitApp("Image provided doesn't end in gif!");
            
            if (!File.Exists(imagePath))
                ExitApp($"{imagePath} Doesn't exist!");

            Console.Out.WriteLine($"Loading {imagePath}...");
            
            // Load the image
            var inputImage = Image.Load(imagePath);
            var frameCount = inputImage.Frames.Count;
            var frameWidth = inputImage.Frames[0].Width;
            var frameHeight = inputImage.Frames[0].Height;
            
            // Create a new image
            var spriteSheet = new SpriteSheet(frameCount, frameWidth, frameHeight);
            for (var i = 0; i < inputImage.Frames.Count; i++)
                spriteSheet.AddImage(inputImage.Frames.CloneFrame(i));
            
            // Validate the path
            var newPath = imagePath.Substring(0, imagePath.Length - 4) + ".png";
            if (File.Exists(newPath)) {
                Console.Out.WriteLine($"There's already a file called {newPath}, press enter to replace.");
                Console.ReadLine();
            }
            
            // Save the image
            spriteSheet.Save(newPath);
            Console.Out.WriteLine($"Image saved as {newPath}");
        }

        private static void ExitApp(string reason) {
            Console.Out.WriteLine(reason);
            Console.Out.WriteLine("Press enter to exit...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}