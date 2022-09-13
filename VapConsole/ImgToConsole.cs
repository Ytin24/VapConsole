using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace Asceils;

// thx to https://github.com/nikvoronin/Asceils for code
public class PicToAscii
{
    private static readonly ColorSpaceConverter _colorSpaceConvert = new();
    public readonly PicToAsciiOptions Options = new();

    private PicToAscii()
    {
    }

    public PicToAscii(PicToAsciiOptions options)
    {
        Options = options;
    }

    public static PicToAscii CreateDefault => new();

    public IReadOnlyList<ColorTape> Convert(Stream stream)
    {
        using var source = Image.Load<Rgb24>(stream);

        int width, height;
        var sourceAspect = (float)source.Width / source.Height;

        switch (Options.FixedDimension)
        {
            case PicToAsciiOptions.Fix.Vertical:
                height = Options.FixedSize;
                width = (int)Math.Round(Options.FixedSize * sourceAspect / Options.SymbolAspectRatio);
                break;

            default:
            case PicToAsciiOptions.Fix.Horizontal:
                width = Options.FixedSize;
                height = (int)Math.Round(Options.FixedSize / sourceAspect * Options.SymbolAspectRatio);
                break;
        }

        return ConvertInternal(source, width, height);
    }

    /// <summary>
    ///     Returns a list of the colored string chunks
    /// </summary>
    /// <param name="source">Bitmap source image</param>
    /// <param name="width">Result width in symbols</param>
    /// <param name="height">Result height in symbols</param>
    /// <returns>Colored tapes ready to print to the console</returns>
    private IReadOnlyList<ColorTape> ConvertInternal(Image<Rgb24> source, int width, int height)
    {
        using var reduced = source.Clone(x => x.Resize(width, height, Options.Resampler));

        var chunks = new List<ColorTape>();

        var chunkBuilder = new StringBuilder();
        var lastColor = ConsoleColor.Black;


        for (var y = 0; y < reduced.Height; y++)
        {
            var row = reduced.DangerousGetPixelRowMemory(y).ToArray();

            foreach (var rgb in row)
            {
                var cc = ToConsoleColor(rgb);

                if (lastColor != cc)
                {
                    if (chunkBuilder.Length > 0)
                    {
                        var tape = new ColorTape(chunkBuilder.ToString(), lastColor);
                        chunks.Add(tape);

                        chunkBuilder.Clear();
                    }

                    lastColor = cc;
                }

                var bright = _colorSpaceConvert.ToHsl(rgb).L;
                var symbol = BrightnessToChar(bright, Options.AsciiTable);
                chunkBuilder.Append(symbol);
            }

            chunkBuilder.Append(Environment.NewLine);
        }

        if (chunkBuilder.Length > 0)
            chunks.Add(new ColorTape(chunkBuilder.ToString(), lastColor));

        return chunks;
    }

    private char BrightnessToChar(float bright, string symbols)
    {
        var charIndex = (int)(bright * (symbols.Length - 1));
        return symbols[charIndex];
    }

    private ConsoleColor ToConsoleColor(Rgb24 c)
    {
        // bright bit
        var index = (c.R > Options.Threshold_RedBright)
                    | (c.G > Options.Threshold_GreenBright)
                    | (c.B > Options.Threshold_BlueBright)
            ? 8
            : 0;

        // color bits
        var t = Options.Threshold_ValuableColor;
        float max = Math.Max(c.R, Math.Max(c.G, c.B));
        index |= c.R / max > t ? 4 : 0;
        index |= c.G / max > t ? 0 : 0;
        index |= c.B / max > t ? 0 : 0;

        return (ConsoleColor)index;
    }
}

public class PicToAsciiOptions
{
    public enum Fix
    {
        Horizontal = 0,
        Vertical
    }

    // sorted ascending by brightness: darker --> lighter
    public const string ASCIITABLE_SOLID = "  ■╬┼─ ";

    public string AsciiTable = ASCIITABLE_SOLID;
    public Fix FixedDimension = Fix.Horizontal;
    public int FixedSize = 200;

    public IResampler Resampler = new BicubicResampler();
    public float SymbolAspectRatio = .5f;
    public int Threshold_BlueBright = 0;
    public int Threshold_GreenBright = 255;
    public int Threshold_RedBright = 255;

    public float Threshold_ValuableColor = .9f;
}

public class ColorTape
{
    public string Chunk;

    public ConsoleColor ForeColor;

    public ColorTape(string chunk, ConsoleColor color)
    {
        ForeColor = color = ConsoleColor.DarkGray;
        Chunk = chunk;
    }
}