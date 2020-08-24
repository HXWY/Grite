using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Grite.Entities {
    public class SpriteSheet {
        private const int MaxFramesPerRow = 10;
        
        // Provided image details
        private readonly int _frameCount;
        private readonly int _frameWidth;
        private readonly int _frameHeight;
        
        // New Image
        private readonly Image _image;
        private int _row;
        private int _column;

        public SpriteSheet(int frameCount, int frameWidth, int frameHeight) {
            _frameCount = frameCount;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            
            CalculateImageSize(out var width, out var height);
            _image = new Image<Rgba32>(width, height, new Rgba32(0, 0, 0, 0));
        }

        /// <summary>
        /// Calculates the image size based on the frame count, width and height of the image.
        /// </summary>
        /// <param name="width">The output image width.</param>
        /// <param name="height">The output image height.</param>
        private void CalculateImageSize(out int width, out int height) {
            width = Math.Min(MaxFramesPerRow, _frameCount) * _frameWidth;
            var rows = (int) Math.Ceiling((float) _frameCount / MaxFramesPerRow);
            height = rows * _frameHeight;
        }
        
        /// <summary>
        /// Adds a new image to the spritesheet.
        /// </summary>
        /// <param name="image">The image to add.</param>
        public void AddImage(Image image) {
            _image.Mutate(img => {
                img.DrawImage(image, GetNextPosition(), 1f);
            });
        }

        /// <summary>
        /// Gets where the next image should be located. Then moves the counters.
        /// </summary>
        /// <returns>The point where the next image should be added to.</returns>
        private Point GetNextPosition() {
            var horizontal = _column * _frameWidth;
            var vertical = _row * _frameHeight;

            if (++_column >= MaxFramesPerRow) {
                _column = 0;
                _row++;
            }

            return new Point(horizontal, vertical);
        }

        /// <summary>
        /// Saves the image to the provided file path.
        /// </summary>
        /// <param name="path">Where to save the file.</param>
        public void Save(string path) => _image.Save(path);
    }
}