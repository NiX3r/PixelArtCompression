using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelArtCompression
{
    class CompressionBitmap
    {
        public static Bitmap Compress(Bitmap bitmap, int diviation, String outputFile)
        {

            try
            {
                int[,] array = new int[bitmap.Width, bitmap.Height];
                int maxBound = diviation;
                int xLength = 999999, yLength = 999999;
                int tempXLength = 0, tempYLength = 0;
                int tempXStart = 0;
                Color border = Color.Black;

                // Find border color of pixel art
                Color startColor = bitmap.GetPixel(0, 0);
                bool gotBorder = false;
                for (int y = 1; y < bitmap.Height; y++)
                {
                    for (int x = 1; x < bitmap.Width; x++)
                    {
                        // Fond difference between start back color and border color
                        if (bitmap.GetPixel(x, y) != startColor)
                        {
                            // Get color move by .25% of bitmap size
                            border = bitmap.GetPixel(x + (bitmap.Width / 250), y + (bitmap.Height / 250));
                            gotBorder = true;
                            break;
                        }
                    }
                    if (gotBorder)
                        break;
                }

                // Find black borders from bitmap
                for (int y = 1; y < bitmap.Height; y++)
                {
                    for (int x = 1; x < bitmap.Width; x++)
                    {
                        Color bmPixel = bitmap.GetPixel(x, y);
                        if (bmPixel != Color.White && bmPixel != Color.Empty)
                        {
                            if (bmPixel.R >= (border.R - maxBound) && bmPixel.R <= (border.R + maxBound) &&
                                bmPixel.G >= (border.G - maxBound) && bmPixel.G <= (border.G + maxBound) &&
                                bmPixel.B >= (border.B - maxBound) && bmPixel.B <= (border.B + maxBound))
                            {
                                array[x, y] = 1;
                                tempXStart++;
                            }
                            else
                            {
                                array[x, y] = 0;
                            }
                        }
                        else
                        {
                            array[x, y] = 0;
                        }
                    }
                }

                // Get smallest "bix pixel"
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // Found start of "big pixel"
                        if (array[x, y] == 1 && array[x - 1, y] == 0 && array[x - 1, y - 1] == 0 && array[x, y - 1] == 0)
                        {
                            for (int ty = y; ty < bitmap.Height; ty++)
                            {
                                if (tempXLength == 0)
                                {
                                    for (int tx = x; tx < bitmap.Width; tx++)
                                    {
                                        if (array[tx, y] == 0)
                                            break;
                                        tempXLength++;
                                    }
                                }

                                if (array[x, ty] == 0)
                                {
                                    Debug.WriteLine($"Found big pixel X: {x} Y: {y} XL: {tempXLength} and YL: {tempYLength}");
                                    //Program.GetUI().writeLog($"Found big pixel X: {x} Y: {y} XL: {tempXLength} and YL: {tempYLength}");
                                    if ((xLength > tempXLength && yLength > tempYLength) ||
                                        (xLength > tempXLength && yLength == tempYLength) ||
                                        (xLength == tempXLength && yLength > tempYLength))
                                    {
                                        xLength = tempXLength;
                                        yLength = tempYLength;
                                    }
                                    tempXLength = tempYLength = 0;
                                    break;
                                }
                                tempYLength++;

                            }
                        }
                    }
                }

                Debug.WriteLine($"Smallest pixel X: {xLength} and Y: {yLength}\nBorder R: {border.R} G: {border.G} B: {border.B}");
                Program.GetUI().writeLog($"Smallest bix pixel length X: {xLength} and Y: {yLength} | Border color R: {border.R} G: {border.G} B: {border.B}");
                // Compress bitmap
                Bitmap bmp = new Bitmap(bitmap.Width / xLength, bitmap.Height / yLength);

                // Color whole bitmap to white
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        Color c = bitmap.GetPixel(x * xLength + (xLength / 3), y * yLength + (yLength / 3));
                        bmp.SetPixel(x, y, c);
                    }
                }
                bmp.Save(outputFile);


                return bmp;
            }
            catch (Exception ex)
            {
                Program.GetUI().writeLog(ex.Message);
                return bitmap;
            }

        }
    }
}
